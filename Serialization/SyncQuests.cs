using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SyncQuests : SyncComponent
{

    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts, and fairly recalcualte using CalcDefault

    #region params
    public int maxQuests;
    public int maxDailyQuests; //this is the hard limit used for initialization/ syncing width, not the limit we show to clients.. (might want to give "more daily quests" as a perk of premium or something"

    #endregion

    #region selfRefs
    SyncInt16MultiField questsField;
    SyncInt16MultiField questProgressField;
    SyncInt16MultiField questAmountsField; //amount needed
    SyncInt16MultiField questTargetsField; //array of targeted item/entity ids for these quests

    SyncInt16MultiField dailyQuestsField;
    SyncInt16MultiField dailyQuestProgressField;
    SyncInt16MultiField dailyQuestAmountsField; //amount needed
    SyncInt16MultiField dailyQuestTargetsField; //array of targeted item/entity ids for these quests

    SyncFloat16Field_1024 questRewardBonus;

    OnlineGUI parentGUI;
    Entity owner;
    #endregion

    public delegate void OnFinishQuest(int index);
#pragma warning disable CS0067 // The event 'SyncQuests.onFinishQuest' is never used
    /// <summary>
    /// params- (int index)
    /// </summary>
    public event OnFinishQuest onFinishQuest;
#pragma warning restore CS0067 // The event 'SyncQuests.onFinishQuest' is never used

    //codepoint constants
    public static readonly string
        quests = "quests",
        questProgress = "questProgress",
        questAmounts = "questAmounts", //amount needed
        questTargets = "questTargets",
        dailyQuests = "dailyQuests",
        dailyQuestProgress = "dailyQuestProgress",
        dailyQuestAmounts = "dailyQuestAmounts", //amount needed
        dailyQuestTargets = "dailyQuestTargets",
        rewardBonus = "questRewardBonus"
    ;


    private void Awake()
    {
        //subscribe to when we login to a new day to refresh dailies
        SyncAnalytics syncAnalytics = GetComponent<SyncAnalytics>();
        if (syncAnalytics != null && Server.isServer)
            syncAnalytics.onPlayAdditionalDay += RefreshDailyQuests_Server; //when we get account data from server, if it is older than a day... generate some new daily quests

    } //end func Awake

    private void Start()
    {
        if (owner != null && owner.isPlayer && owner.p.isLocal)
            parentGUI = owner.p.c.onlineGUI;
    }

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        questsField = new SyncInt16MultiField(quests, false, new int[maxQuests], parent, OnUpdateQuestIDs, accountCodes.questField);
        questProgressField = new SyncInt16MultiField(questProgress, false, new int[maxQuests], parent, null, accountCodes.questProgressField);
        questTargetsField = new SyncInt16MultiField(questTargets, false, new int[maxQuests], parent, null, accountCodes.questTargetField);
        questAmountsField = new SyncInt16MultiField(questAmounts, false, new int[maxQuests], parent, null, accountCodes.questAmountsField);

        dailyQuestsField = new SyncInt16MultiField(dailyQuests, false, new int[maxDailyQuests], parent, OnUpdateDailyQuestIDs, accountCodes.dailyQuestField);
        dailyQuestProgressField = new SyncInt16MultiField(dailyQuestProgress, false, new int[maxDailyQuests], parent, null, accountCodes.dailyQuestProgress);
        dailyQuestTargetsField = new SyncInt16MultiField(dailyQuestTargets, false, new int[maxDailyQuests], parent, null, accountCodes.dailyQuestTargets);
        dailyQuestAmountsField = new SyncInt16MultiField(dailyQuestAmounts, false, new int[maxDailyQuests], parent, null, accountCodes.dailyQuestAmounts);


        syncFields = new SyncField[]
        {
            questsField,
            questProgressField,
            questTargetsField,
            questAmountsField,

            dailyQuestsField,
            dailyQuestProgressField,
            dailyQuestTargetsField,
            dailyQuestAmountsField
        };

        if (parent != null)
        {
            this.owner = parent.e;

            questProgressField.onUpdate += OnUpdateQuestProgress;
            dailyQuestProgressField.onUpdate += OnUpdateDailyQuestProgress;

        } //end if parent exists

    } //end SetDefaults

    //when adding or removing a quest
    void OnUpdateQuestIDs(ref int[] newValues)
    {
        //loop through and unsubscribe all just in case
        for (int i = 0; i < newValues.Length; i++)
        {
            if (newValues[i] > 0)
            { //if exists
                Quest.GetQuest(newValues[i]).OnFinishQuest_Unsubscribe(owner, i, false); //unsubscribe to progress just in case
            }
        } //end for

        //resubscribe all
        for (int i = 0; i < newValues.Length; i++)
        {
            if (newValues[i] > 0)
            { //if exists
                  Quest.GetQuest(newValues[i]).OnStartQuest_Subscribe(owner, i, false); //subscribe to possible progress
            }
        } //end for 
    } //end OnUpdateQuestIDs

    //when adding or removing a quest
    void OnUpdateDailyQuestIDs(ref int[] newValues)
    {
        //loop through and unsubscribe all just in case
        for (int i = 0; i < newValues.Length; i++)
        {
            if (newValues[i] > 0)
            { //if exists
                Quest.GetQuest(newValues[i]).OnFinishQuest_Unsubscribe(owner, i, true); //unsubscribe to progress just in case
            }
        } //end for 
        
        //resubscribe all
        for (int i = 0; i < newValues.Length; i++)
        {
            if (newValues[i] > 0)
            { //if exists
                Quest.GetQuest(newValues[i]).OnStartQuest_Subscribe(owner, i, true); //subscribe to possible progress
            }
        } //end for 
    } //end OnUpdateDailyQuestIDs

    private void RefreshDailyQuests_Server()
    {
        if (!GameMode.instance.dailyQuestsEnabled) //if daily quests are disabled for this game mode
            return;

        int count = dailyQuestsField.value.Length;

        int newQuestID;
        int newQuestTarget;
        int newQuestAmount;

        for (int i = 0; i < count; i++)
        {
            RemoveQuest_Server(i, true); //remove old
            DailyQuestsConfig.instance.GetRandom(out newQuestID, out newQuestAmount, out newQuestTarget);
            AddQuest_Server(newQuestID, newQuestTarget, newQuestAmount, true); //add quest
        }

    } //end func RefreshDailyQuests

    void OnUpdateQuestProgress(ref int[] newValues)
    {
        int id;
        for (int i = 0; i < newValues.Length; i++)
        {
            id = questsField.value[i];
            if (id != 0 && newValues[i] >= questAmountsField.value[i])
            {
                FinishQuest_ClientAndServer(id, i, false);
                if (parentGUI != null)
                {
                    //show reward popup or something for finishing this quest
                    parentGUI.GetComponentInChildren<QuestPopups>().PlayFor(newValues[i], i, false);
                }
                if (Server.isServer)
                    RemoveQuest_Server(i, true);
            } //end if finished quest

        } //end for loop

    } //end func OnUpdateQuestProgress
    void OnUpdateDailyQuestProgress(ref int[] newValues)
    {

        int id;
        for(int i = 0; i < newValues.Length; i++)
        {
            id = dailyQuestsField.value[i];
            if(id != 0 && newValues[i] >= dailyQuestAmountsField.value[i]) {
                FinishQuest_ClientAndServer(id, i, true);
                if (parentGUI != null)
                {
                    //show reward popup or something for finishing this quest
                    parentGUI.GetComponentInChildren<QuestPopups>().PlayFor(newValues[i], i, true);
                }
                if (Server.isServer)
                    RemoveQuest_Server(i, true);
            } //end if finished quest
        } //end for loop

    } //end func OnUpdateDailyQuestProgress


    //
    /// <summary>
    /// add a random quest of type with supplied possible targets and possible amounts of those targets
    /// </summary>
    /// <param name="questType">like kill count, retrieve items, specific implementations of those classes...</param>
    /// <param name="possibleTargets">what to target</param>
    /// <param name="possibleAmounts">how much to target</param>
    /// <param name="dailyQuest">whether a regular (false) or daily (true) quest</param>
    /// <returns>return true if suceceded...</returns>
    public bool AddRandomQuest_Server(int questType, int[] possibleTargets, int[] possibleAmounts, bool dailyQuest)
    {
        return AddQuest_Server(questType, possibleTargets.Random(), possibleAmounts.Random(), dailyQuest);
    } //end func AddRandomQuest

    //questTarget- what object/item/entity/mode to obtain/kill/win/whatever
    //return true if they have space...
    public bool AddQuest_Server(int id, int questTarget, int amount, bool dailyQuest)
    {
        int newQuestIndex;
        if(!dailyQuest) {
            //is regular quest
            newQuestIndex = GetIndex_RegularQuest(0); //find first empty index
            if (newQuestIndex == -1)
                return false; //no space
            questsField.Set(id, newQuestIndex); //actual quest id
            questTargetsField.Set(questTarget, newQuestIndex); //what entity/item/whatever
            questAmountsField.Set(amount, newQuestIndex); //should do this before progress so that it doesn't trip "finished"  during progress's onUpdate due to amount == 0
            questProgressField.Set(0, newQuestIndex); //ensure progress for this index reset...

        } else
        {
            //is daily quest
            newQuestIndex = GetIndex_DailyQuest(0); //find first empty index
            if (newQuestIndex == -1)
                return false; //no space
            dailyQuestsField.Set(id, newQuestIndex); //actual quest id
            dailyQuestTargetsField.Set(questTarget, newQuestIndex); //what entity/item/whatever
            dailyQuestAmountsField.Set(amount, newQuestIndex); //should do this before progress so that it doesn't trip "finished" during progress's onUpdate  due to amount == 0
            dailyQuestProgressField.Set(0, newQuestIndex); //ensure progress for this index reset
        }

        return true;
    } //end func AddQuest

    /// <summary>
    /// try add progress to quest, return true if quest is now finished
    /// </summary>
    public void TryIncrementProgress_Server(int id, ref int index, int amount, bool dailyQuest = false)
    {
        TryIncrementProgress_Server(ref id, ref index, ref amount, dailyQuest ? dailyQuestsField : questsField, dailyQuest ? dailyQuestProgressField : questProgressField);
    } //end func IncrementProgress_RegularQuest


    private void TryIncrementProgress_Server(ref int id, ref int index, ref int amount, SyncInt16MultiField quests, SyncInt16MultiField questProgress)
    {
        if (!Server.isServer)
            return; //clients should receive their updates through subscribing to SyncField..

        if (!Quest.QuestExists(id))
            return;

        questProgress.Set(questProgress.value[index] + amount, index);

    } //end func IncrementProgress

    private void FinishQuest_ClientAndServer(int id, int index, bool isDaily)
    {
        Quest quest = Quest.GetQuest(id);
        List<shopItemInstance> rewards = quest.GetRewards(owner, index, isDaily);

        if (Server.isServer)
        {
            quest.OnFinishQuest_Unsubscribe(owner, index, isDaily); //don't need to keep progressing this quest...

            if (!Inventory.CanLoadAllItems(owner, rewards))
            { //items do not fit in inventory, cannot proceed
              //should give the owner a dialog to claim reward either now or after this method returns
                return;
            }

            if(rewards != null && rewards.Count > 0)
                foreach (shopItemInstance item in rewards)
                {
                    Inventory.TryLoadItemFirstAvailable(owner, item);
                }

            quest.OnFinishQuest_AnythingSpecial_Server(owner, index, isDaily);
        }

        quest.OnFinishQuest_AnythingSpecial_Client(owner, index, isDaily);
    } //end func FinishQuest

    public void RemoveQuest_Server(int index, bool daily)
    {
        if(!daily)
        { //regular quests
            questsField.Set(0, index);
            questProgressField.Set(0, index);
            questTargetsField.Set(0, index);
            questAmountsField.Set(0, index);
        } else
        { //daily quests
            dailyQuestsField.Set(0, index);
            dailyQuestProgressField.Set(0, index);
            dailyQuestTargetsField.Set(0, index);
            dailyQuestAmountsField.Set(0, index);

        }
    } //end func RemoveQuest

    /// <summary>
    /// return index or -1 of regular quest in this user's current quests
    /// </summary>
    public int GetIndex_RegularQuest(int id)
    {
        return GetQuestIndex(questsField, ref id);
    }
    /// <summary>
    /// return index or -1 of regular quest in this user's daily quests
    /// </summary>
    public int GetIndex_DailyQuest(int id)
    {
        return GetQuestIndex(dailyQuestsField, ref id);
    }

    static int GetQuestIndex(SyncInt16MultiField target, ref int id)
    {
        return Array.IndexOf(target.value, id);
    }


} //end class SyncQuests