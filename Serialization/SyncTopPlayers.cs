using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// agario-like list of top players and their scores
/// </summary>
public class SyncTopPlayers : SyncComponent
{

    #region singleton
    //Singleton code
    // s_Instance is used to cache the instance found in the scene so we don't have to look it up every time.
    public static SyncTopPlayers s_Instance = null;
    // This defines a static instance property that attempts to find the manager object in the scene and
    // returns it to the caller.
    public static SyncTopPlayers instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(SyncTopPlayers)) as SyncTopPlayers;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
                GameObject obj = new GameObject("SyncTopPlayers");
                s_Instance = obj.AddComponent(typeof(SyncTopPlayers)) as SyncTopPlayers;
                Logger.Log("Could not locate an SyncTopPlayers object. Server was Generated Automaticly.");
            }
            return s_Instance;
        }
    }
    #endregion

    public override bool resetToDefaultValues_OnGameRestart => false; //NOTE- change this to true?

    public const string
        names = "topNames",
        scores = "topScores",
        ids = "topIDs"
    ;

    public int numToSync = 10; //how many players are on this leaderboard
    public const int maxCharLength = 32;

    private int[] _scores = new int[0]; //cache ref
    private string[] _names = new string[0]; //cache ref
    private int[] _ids = new int[0]; //cache ref

    private SyncStringFixedMultiField_UTF16 namesField;
    private SyncInt32MultiField scoresField;
    private SyncInt16MultiField idsField; //need to keep track of the IDs just in case there is two "Guest" names on score list, for example

    //TODO- show "out of X" stat for current player count? So the player knows how many players they're beating

#if UNITY_EDITOR
    #region debugstuff
    public bool debugPopulate;
    public int[] debugScores;
    public string[] debugNames;
    public int[] debugIDs;

    private void OnGUI()
    {
        if(debugPopulate)
        { //if pressed debug button in GUI
            debugPopulate = false; //reset button
            //populate with debug data
            for(int i = 0; i < debugScores.Length; i++)
            {
                scoresField.Set(debugScores[i], i);
                namesField.Set(debugNames[i], i);
                idsField.Set(debugIDs[i], i);
            }
        }   
    } //end func OnGUI

    #endregion
#endif


    public override void SetDefaults(SyncObject parent, NetworkIO context, out SyncField[] syncFields)
    {
        namesField = new SyncStringFixedMultiField_UTF16(names, false, new string[numToSync], maxCharLength, parent);
        idsField = new SyncInt16MultiField(ids, false, new int[numToSync], parent);
        scoresField = new SyncInt32MultiField(scores, false, new int[numToSync], parent);

        syncFields = new SyncField[]
        {
            namesField,
            idsField,
            scoresField,
        };

        _names = namesField.value; //cache
        _scores = scoresField.value; //cache 
        _ids = scoresField.value;

        if (parent != null)
        {

        }
    } //end SetDefaults

    /// <summary>
    /// try to insert and bump high scores
    /// </summary>
    public void TryUpsertNow(Entity target, int newScore)
    {
        //check if they're already on the top score, otherwise, try to bump the high scores
        for(int i = 0; i < _ids.Length; i++)
        {
            if(target.id == _ids[i])
            { //if already on top score
                scoresField.Set(newScore, i); //update at index
                return;
            }
        } //end loop over _ids

        //else try bump the high scores down
        //doesn't really matter what index they reached, just bump the bottom player off no matter what (if there is more than 10 players)
        int lowest = 0;
        int lowestIndex = -1; //temp keep track of where lowest is
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] < lowest)
            {
                lowest = scores[i];
                lowestIndex = i;
            }
        }

        if(lowestIndex > -1)
        { //if there is a lowest player
            //try replace their entry
            if(newScore > lowest)
            { //if they belong on the scoreboard (instead of the current lowest)
                idsField.Set(target.id, lowestIndex);
                namesField.Set(target.displayName, lowestIndex);
                scoresField.Set(newScore, lowestIndex);
            }
        } else
        {
            //no entries.. so place them as #1 (anywhere)
            idsField.Set(target.id, 0);
            namesField.Set(target.displayName, 0);
            scoresField.Set(newScore, 0);
        }

        //don't need to sort or anything (would be difficult to manage with multiple arrays)
        //client will populate these values into an array of structs, then sort them by the highest value for display

    } //end func TryUpsertNow

    /// <summary>
    /// remove from leaderboard if they're on it
    /// </summary>
    public void TryRemove(Entity target)
    {
        for (int i = 0; i < _ids.Length; i++)
        { //loop over current leaderboard
            if (_ids[i] == target.id)
            { //if found match
                //remove from leaderboard
                idsField.Set(0, i);
                namesField.Set("", i);
                scoresField.Set(0, i);
                return;
            }
        }
    } //end func TryRemove

    /// <summary>
    /// find the player that has the highest score that isn't on the leaderboard, and place them on it
    /// </summary>
    public void TryFillLowest(Func<Entity, int> getScore)
    {
        if (_ids == null || ids.Length == 0 || scores == null || scores.Length == 0)
            return;

        List<Player> players = Server.instance.players; //cache
        Player player; //cache

        int highestScore = 0;
        Player highestPlayer = null;
        int score = 0; //cache

        //find lowest high score on scoreboard
        int lowest = 0;
        int lowestIndex = -1; //temp keep track of where lowest is
        int emptyIndex = -1; //check for empty spaces in scoreboard

        for (int i = 0; i < scores.Length && i < _ids.Length; i++)
        {
            //try set lowest record
            if (scores[i] < lowest)
            {
                lowest = scores[i];
                lowestIndex = i;
            }

            if (_ids[i] == 0)
                emptyIndex = i; //keep track of this empty index

        }
       


        for (int i = 0, l = players.Count; i < l; i++)
        {
            player = players[i];
            score = getScore(player); //cache

            if (player != null && (emptyIndex != -1 || score > lowest) && !_ids.Contains(player.id))
            { //if at least beats the lowest high score (or is an empty space) and isn't on the scoreboard already
                if(score > highestScore)
                {
                    highestPlayer = player;
                    highestScore = score;
                }
            }
        }

        //now try and fill any empty position, or bump the lowest player off
        if (highestPlayer == null)
            return; //didn't find any candidates for scoreboard (must not be on scoreboard already or be able to fill an empty slot)

        if(emptyIndex != -1)
        { //if can simply fill an empty space
            idsField.Set(highestPlayer.id, emptyIndex);
            namesField.Set(highestPlayer.displayName, emptyIndex);
            scoresField.Set(highestScore, emptyIndex);
            return;
        } else
        {
            //have to bump off the lowest
            idsField.Set(highestPlayer.id, lowestIndex);
            namesField.Set(highestPlayer.displayName, lowestIndex);
            scoresField.Set(highestScore, lowestIndex);

        }

    } //end func TryFillLowest


    //client will populate these values into an array of structs, then sort them by the highest value for display
    //instantiate the score objects on startup, then just set them to empty if the score is vacant

} //end class SyncTopPlayers