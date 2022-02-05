using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class SyncGameStats : SyncComponent
{
    public override bool resetToDefaultValues_OnGameRestart => false; //call base.ResetFieldsToDefaults when game restarts

    public static SyncGameStats instance = null;

    private SyncFloat16Field_1024 countdownNextGameField = default;

    public static bool isInCountdownToNextMatch => instance.countdownNextGameField.value != 0; //set to countdown duration when begins ticking

    private void Awake()
    {
        if(instance != null)
        {
            //Logger.LogError("more than one SyncGameStats in scene");
        }
        instance = this; //set singleton


    } //end function Awake

    //codepoint constants
    public static readonly string
        countdownNextGame = "countdown";

    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        countdownNextGameField = new SyncFloat16Field_1024(countdownNextGame, false, 0, parent);
        syncFields = new SyncField[]
        {
            countdownNextGameField,
        };


        //set GUI stuff from here just in case it's a client which means waiting for it to instantiate...
        if (context != null && context.player != null)
            OnlineGUI.From(context.player).GetComponentInChildren<GameStatsView>().Set(this); //setup GUI on whatever context that created us
    }

    public void TryBeginCountdownNextGame(float duration)
    {
        if(!isInCountdownToNextMatch)
            Timing.RunCoroutine(ICountdownNextGame(duration));
    }
    
    IEnumerator<float> ICountdownNextGame(float duration)
    {
        countdownNextGameField.value = duration;

        while (countdownNextGameField.value > 0)
        {
            yield return Time.deltaTime;
            countdownNextGameField.value = Mathf.Max(0, countdownNextGameField.value - Time.deltaTime); //subtract deltaTime but clamp to 0 to avoid weird errors
        }

        TriggerNextGame();
    } //end ICountdownNextGame

    void TriggerNextGame()
    {
        if(Server.isServer)
        {
            Server.instance.StartNextGame();
        }
    }

} //end class SyncGameStats