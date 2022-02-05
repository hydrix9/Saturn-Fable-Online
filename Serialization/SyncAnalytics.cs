using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using MEC;

public class SyncAnalytics : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //whether to call base.ResetFieldsToDefaults when game restarts

    public delegate void del_OnPlayAdditionalDay();
    /// <summary>
    /// takes parameter (int hist_last32days)
    /// </summary>
    public event del_OnPlayAdditionalDay onPlayAdditionalDay;

    //codepoint constants
    public static readonly string
        last_play = "last_play",
        history_32 = "hist_32";

    SyncDateTimeField_Lossless last_play_field;
    SyncInt32Field history_32_field;

    Player owner;

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        last_play_field = new SyncDateTimeField_Lossless(last_play, false, DateTime.MinValue, parent, OnUpdateLast_play, accountCodes.last_play_field);
        history_32_field = new SyncInt32Field(history_32, false, 0, parent, OnUpdateHistory32, accountCodes.history_32_field);

        syncFields = new SyncField[]
        {
            last_play_field,
            history_32_field
        };

        if (parent != null)
        {
            owner = parent.p;
        }

    } //end func SetDefaults

#pragma warning disable CS0414 // The field 'SyncAnalytics.initlast_play' is assigned but its value is never used
    bool initlast_play = false; //whether we set initial value yet
#pragma warning restore CS0414 // The field 'SyncAnalytics.initlast_play' is assigned but its value is never used

    //this should probably only get called after initial login and receiving an update from the AccountServer
    void OnUpdateLast_play(ref DateTime newValue)
    {
        if (newValue == DateTime.MinValue)
            return; //was default value and means nothing... this is probably during the initial constructor

        //should only be at this point now if receiving update to this field from AccountServer after initializing the field on first login
        //because field will be DateTime.Min defaultValue, and only InitFirstLogin will set it otherwise
        //so newValue should be the value of the previous initFirstLogin

        if (DateTime.UtcNow.Date != newValue.Date)
        { //is a different day
            double daysAgo = (DateTime.UtcNow - newValue).TotalDays;
            /* actually don't need this because .Date != newValue.Date should mean logged in on a different date, like Mon 25th, Tue 26th
            int finalDaysAgo; //what value to actually use
            //if they logged in < 18 hours ago... just give them the benefit of the doubt and say 1 day
            if (daysAgo > 0.75f && daysAgo < 1)
                finalDaysAgo = 1;
            else
                finalDaysAgo = (int)daysAgo;
            */
            //call delegate and shift history
            OnPlayAdditionalDay((int)daysAgo);
            last_play_field.value = DateTime.UtcNow; //update "last_play" to today now that we received the bonus..
        }

    } //end func OnUpdatelast_play

#pragma warning disable CS0414 // The field 'SyncAnalytics.initHist32' is assigned but its value is never used
    bool initHist32 = false; //whether we set inital value yet
#pragma warning restore CS0414 // The field 'SyncAnalytics.initHist32' is assigned but its value is never used

    void OnUpdateHistory32(ref int newValue)
    {
        if (newValue == 0)
            return; //was default value and means nothing...



    } //end func OnUpdateHistory32


    /// <summary>
    /// call this on first login when response from AccountServer indicates never played before
    /// </summary>
    public void TryInitFirstLogin()
    {
        if (last_play_field.value != DateTime.MinValue && last_play_field != default) //make sure can only init if value is already DefaultValue
            return;

        last_play_field.value = DateTime.UtcNow; //init
        //setting this value will also call this.OnUpdatelast_play and therefore IncrementHistoryCounter

        OnPlayAdditionalDay(1); //first ever login counts as "streak" too

    } //end InitFirstLogin

    void OnPlayAdditionalDay(int amount)
    {
        onPlayAdditionalDay?.Invoke();
        //move history counters by num different days
        IncrementHistoryCounter(amount);
    }

    void IncrementHistoryCounter(int amount)
    { 
        //this will push off the back of the array, removing outdated values, and write today to array.. 
        //even works for first run
        history_32_field.value = (history_32_field.value << amount) + 1;

        //count consecutive days...
        //bit shift the mask as long as final bit is 1... or basically bitshift until hit a 0
        int bitShift = history_32_field.value; //init
        int consecutive = 0;
        for(int i = 0; i < 32; i++)
        {
            if ((bitShift & 1) == 1) //if final bit in mask is set
                consecutive++; //counts toward consecutive
            else
                break; //string of consecutive logins broken
        }
        DailyPlayBonusConfig.instance.TryAddRewardsTo(owner, consecutive);

        if (owner != null)
            Logger.Log(owner.name + "logged in for " + consecutive + " consecutive days");

    } //end IncrementHistoryCounter
} //end class SyncAnalytics