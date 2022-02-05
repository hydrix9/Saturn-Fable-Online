#define debug
#define ENABLE_VOXELPLAY
using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
//using Voxeland5;

public partial class Server : NetworkIO {


    //basically the listen process. Mother class NetworkIO uses this to continously enter into handling data on a listen loop, hence 'override'. The start point into handling.
    public override void HandleData(int conn, byte[] receiveBytes)
    {
        if (receiveBytes.Length <= 0)
            return;

        if (logAll)
            Ext.DebugByte(receiveBytes, "server received: ");

#if debug && UNITY_EDITOR
        debugEstimatedDataIn += receiveBytes.Length + 60; //count bytes length plus a udp packet header which averages 20-40 bytes ///actually, WebRTC header is like 60 bytes
        if(opCodeToDebugMsg.ContainsKey(receiveBytes[0]))
            opCodeToDebugMsg[receiveBytes[0]].dataIn += receiveBytes.Length + 60; //add metric for this individual opCode

        if (packetLossPer > 0)
        {
            if (UnityEngine.Random.value < packetLossPer / 100)
            {
                droppedPacketsCounter++;
                return;
            }
        }

        if (delaySec > 0) //if delay is enabled
        {
            Timing.CallDelayed(delaySec, () => { ReceiveRaw(conn, receiveBytes, false, true); });
            return;
        }

        TryDebugMalformResize(receiveBytes); //try artifically malforming packet by resizing array
#endif
        //Logger.Log("handling data");

        // Logger.Log("received");
        // Tools.DebugByte(receiveBytes);

        //main network code code...
        switch (receiveBytes[0])
        {
            case (netcodes.join):
                AddPlayer(conn, ref receiveBytes);
                break;
            case (netcodes.joinAsDisplayName):
                HandleJoinAsDisplayName(ref conn, ref receiveBytes);
                break;
            case (netcodes.playerQuit):
                RemovePlayer(conn);
                break;
            case (netcodes.inputs):
                HandleInputs(ref conn, ref receiveBytes);
                break;
            case (netcodes.pos3d):
                HandlePos(ref conn, ref receiveBytes);
                break;
            case (netcodes.setTarget):
                HandleSetTarget(ref conn, ref receiveBytes);
                break;
            case (netcodes.voxels):
                HandleVoxeland(ref receiveBytes, ref conn);
                break;
            case (netcodes.ability):
                HandleAbility(ref receiveBytes, ref conn);
                break;
            case (netcodes.chat):
                HandleChat(ref receiveBytes, ref conn);
                break;
            case (netcodes.shops):
                HandleShops(ref receiveBytes, ref conn);
                break;
            case (netcodes.infoplz):
                HandleAskOfPlayer(ref receiveBytes, ref conn);
                break;
            case (netcodes.ping):
                HandlePing(ref receiveBytes, ref conn);
                break;
            case (netcodes.yourid): //in this case actually asking for their id
                HandleYourID(ref receiveBytes, ref conn);
                break;
            case (netcodes.askSwapItem):
                HandleAskSwapItemPlaces(ref receiveBytes, ref conn);
                break;
            case (netcodes.sellItem):
                HandleAskSellItem(ref receiveBytes, ref conn);
                break;
            case (netcodes.tryBuy):
                HandleTryBuy(ref receiveBytes, ref conn);
                break;
            case (netcodes.tryBuyBulk):
                HandleTryBuyBulk(ref receiveBytes, ref conn);
                break;
            case (netcodes.playerAccountData):
                HandleUsername(ref receiveBytes, ref conn);
                break;
            case (netcodes.reliableMessage):
                HandleReliableMessage(ref receiveBytes, ref conn);
                break;
            case (netcodes.confirmReceived):
                HandleConfirmMessage(ref receiveBytes, ref conn);
                break;
            case (netcodes.resetTalents):
                HandleResetTalents(ref receiveBytes, ref conn);
                break;
            case (netcodes.premiumShopBought):
                HandleBoughtPremiumItem(ref receiveBytes, ref conn); //update the user's premium items through 3rd party APIs
                break;
            case (netcodes.factionMap):
                HandleRequestFactionMap(ref receiveBytes, ref conn);
                break;
            case (netcodes.monetizationStart):
                HandleMonetizationStart(ref receiveBytes, ref conn);
                break;
            case (netcodes.rewardAdStart):
                HandleRewardAdStart(ref receiveBytes, ref conn);
                break;
            case (netcodes.rewardAdFinish):
                HandleRewardAdFinish(ref receiveBytes, ref conn);
                break;
#if ENABLE_VOXELPLAY
            case (netcodes.requestChunks):
                VoxelPlayServer.instance.HandleRequestChunks(receiveBytes, conn);
                break;
            case (netcodes.requestChunk):
                VoxelPlayServer.instance.HandleRequestChunk(ref receiveBytes, conn);
                break;
            case (netcodes.requestVoxel):
                VoxelPlayServer.instance.HandleRequestVoxel(ref receiveBytes, ref conn);
                break;
            case (netcodes.placeVoxel):
                VoxelPlayServer.instance.HandlePlaceVoxel(ref receiveBytes, ref conn);
                break;
            case (netcodes.breakVoxel):
                VoxelPlayServer.instance.HandleBreakVoxel(ref receiveBytes, ref conn);
                break;
#endif
            default:
                break;

        } //end switch


        if (delaySec <= 0) //don't want to do this if using CallDelayed
            receiveBytes = null; //dispose
    } //end HandleData


    public float delaySec = 0;
    public float packetLossPer = 1;
    [SerializeField]
    public int droppedPacketsCounter = 0;

    //for debug
    //robot just specifies that yes, eventually in AddPlayer you must ignore this IPEndPoint because it is invalid...
    //delayed is what tells whether it has been delayed already..
    public void ReceiveRaw(int conn, byte[] receiveBytes, bool robot = false, bool delayed = false)
    {
        if (receiveBytes.Length <= 0)
            return;

        if (logAll)
            Ext.DebugByte(receiveBytes, "server received: ");
#if debug && UNITY_EDITOR

        debugEstimatedDataIn += receiveBytes.Length + 60; //count bytes length plus a udp packet header which averages 20-40 bytes ///actually, WebRTC header is like 60 bytes
        if (opCodeToDebugMsg.ContainsKey(receiveBytes[0]))
            opCodeToDebugMsg[receiveBytes[0]].dataIn += receiveBytes.Length + 60; //add metric for this individual opCode
#endif
        if (packetLossPer > 0)
        {
            if (UnityEngine.Random.value < packetLossPer / 100)
            {
                droppedPacketsCounter++;
                return;
            }
        }

        if (delaySec > 0 && !delayed)
        {
            Timing.CallDelayed(delaySec, () => { ReceiveRaw(conn, receiveBytes, robot, true); });
            return;
        }

        //main network code code...
        switch (receiveBytes[0])
        {
            case (netcodes.join):
                AddPlayer(conn, ref receiveBytes, robot);
                break;
            case (netcodes.joinAsDisplayName):
                HandleJoinAsDisplayName(ref conn, ref receiveBytes);
                break;
            case (netcodes.playerQuit):
                RemovePlayer(conn);
                break;
            case (netcodes.inputs):
                HandleInputs(ref conn, ref receiveBytes);
                break;
            case (netcodes.pos3d):
                HandlePos(ref conn, ref receiveBytes);
                break;
            case (netcodes.setTarget):
                HandleSetTarget(ref conn, ref receiveBytes);
                break;
            case (netcodes.infoplz):
                HandleAskOfPlayer(ref receiveBytes, ref conn);
                break;
            case (netcodes.ping):
                HandlePing(ref receiveBytes, ref conn);
                break;
            case (netcodes.voxels):
                HandleVoxeland(ref receiveBytes, ref conn);
                break;
            case (netcodes.ability):
                HandleAbility(ref receiveBytes, ref conn);
                break;
            case (netcodes.chat):
                HandleChat(ref receiveBytes, ref conn);
                break;
            case (netcodes.shops):
                HandleShops(ref receiveBytes, ref conn);
                break;
            case (netcodes.yourid): //in this case actually asking for their id
                HandleYourID(ref receiveBytes, ref conn);
                break;
            case (netcodes.askSwapItem):
                HandleAskSwapItemPlaces(ref receiveBytes, ref conn);
                break;
            case (netcodes.sellItem):
                HandleAskSellItem(ref receiveBytes, ref conn);
                break;
            case (netcodes.tryBuy):
                HandleTryBuy(ref receiveBytes, ref conn);
                break;
            case (netcodes.tryBuyBulk):
                HandleTryBuyBulk(ref receiveBytes, ref conn);
                break;
            case (netcodes.playerAccountData):
                HandleUsername(ref receiveBytes, ref conn);
                break;
            case (netcodes.reliableMessage):
                HandleReliableMessage(ref receiveBytes, ref conn);
                break;
            case (netcodes.confirmReceived):
                HandleConfirmMessage(ref receiveBytes, ref conn);
                break;
            case (netcodes.resetTalents):
                HandleResetTalents(ref receiveBytes, ref conn);
                break;
            case (netcodes.premiumShopBought):
                HandleBoughtPremiumItem(ref receiveBytes, ref conn); //update the user's premium items through 3rd party APIs
                break;
            case (netcodes.factionMap):
                HandleRequestFactionMap(ref receiveBytes, ref conn);
                break;
            case (netcodes.monetizationStart):
                HandleMonetizationStart(ref receiveBytes, ref conn);
                break;
            case (netcodes.rewardAdStart):
                HandleRewardAdStart(ref receiveBytes, ref conn);
                break;
            case (netcodes.rewardAdFinish):
                HandleRewardAdFinish(ref receiveBytes, ref conn);
                break;
#if ENABLE_VOXELPLAY
            case (netcodes.requestChunks):
                VoxelPlayServer.instance.HandleRequestChunks(receiveBytes, conn);
                break;
            case (netcodes.requestChunk):
                VoxelPlayServer.instance.HandleRequestChunk(ref receiveBytes, conn);
                break;
            case (netcodes.requestVoxel):
                VoxelPlayServer.instance.HandleRequestVoxel(ref receiveBytes, ref conn);
                break;
            case (netcodes.placeVoxel):
                VoxelPlayServer.instance.HandlePlaceVoxel(ref receiveBytes, ref conn);
                break;
            case (netcodes.breakVoxel):
                VoxelPlayServer.instance.HandleBreakVoxel(ref receiveBytes, ref conn);
                break;
#endif

            default:
                break;

        } //end switch
    } //end ReceiveRaw



    public void HandleUsername(ref byte[] data, ref int conn)
    {
        string decoded = Encoding.Unicode.GetString(data, 1, data.Length - 1); //decode string with args
        if (decoded == null)
            return;

        string[] args = decoded.Split(delimeters_accounts.args);

        if (args.Length != 6) //should be exactly 6 for user_id + game_auth_token + id_token + api_platform_name + username + usernick
            return;

        if (args[0].Length > 256) //user_id
            return; //malicious

        if (args[1].Length > 256) //game_auth_token
            return; //malicious

        if (args[2].Length > 256) //api_platform_name
            return; //malicious

        if (args[3].Length > 256) //username
            return; //malicious

        if (args[4].Length > 256) //usernick
            return; //malicious

        if (args[5].Length > 256) //usernick
            return; //malicious

        ulong user_id;
        if(ulong.TryParse(args[0], out user_id))
            AccountClient.instance.FetchAccountData_Nag(conn, user_id, args[1], args[2], args[3], args[4], args[5], false, false); //already subscribed to onEntityData delegate for when this returns data..

    } //end HandleUsername
     
    /// <summary>
    /// called from HandleData after client sends what character they picked from their character selection screen
    /// </summary>
    public void HandleJoinAsDisplayName(ref int conn, ref byte[] data)
    {
        string[] args = Encoding.Unicode.GetString(data, 1, data.Length - 1).Split(delimeters_accounts.args);
        if (args.Length != 7)
            return; //malformed packet

        string displayName = args[1];
        char classCharCode = args[2][0];
        ulong source_userID;
        if (!ulong.TryParse(args[3], out source_userID))
            return; //malformed packet
        string game_auth_token = args[4];
        string id_token = args[5];
        string api_platform_name = args[6];

        PickCharacter(conn, source_userID, displayName, classCharCode, game_auth_token, id_token, api_platform_name); //spawn this player using results already retrieved from AccountServer
    } //end HandleJoinAsDisplayName

    public void HandleRequestFactionMap(ref byte[] data, ref int conn)
    {
        SendReliably(ref GameMode.instance.serializedFactionMap, conn);
    } //end func HandleRequestFactionMap

    public void AddPlayer(int conn, ref byte[] data, bool robot = false)
    {
        if(data.Length != 2)
        {
            Logger.LogWarning("received malformed packed with improper length on AddPlayer. Unable to proceed.");
            return;
        }

        CreatePlayer(conn, data[1], null, null, robot);
    }


    public void RemovePlayer(int conn, bool robot = false)
    {
        Logger.Log("RemovePlayer on server");

        /*
        //TODO: add error checking to prevent hack induced crashes..
        if (!points.ContainsKey(conn))
        {
            //Logger.LogError("couldn't remove nonexistant player ");
            return;
        }
        */

#if oldMethod
        if (!robot)
            onSendData -= (ref byte[] data) => { client.Send(data, data.Length, conn); }; //deregister
#endif
        if (!robot)
            onSendData -= (ref byte[] data) => { SendPlayer(data, conn); }; //deregister



        if (points.ContainsKey(conn))
        {
            points[conn].Destroy();
        }

        Player results;
        points.TryRemove(conn, out results);
        players.RemoveAll(entry => entry == null || entry.point == conn); //purge null entries and remove from list

        AccountClient.instance.PurgeAccountData(conn); //remove references in dictionaries

    } //end func RemovePlayer

    
    public void HandleSetTarget(ref int conn, ref byte[] data)
    {
        if (data.Length < 3)
            return;
        int target = data[1] | data[2] << 8;
        if (!EntityExists(target))
            return;

        if(points.ContainsKey(conn))
            points[conn].Set<int>(SyncCasting.currentTarget, target);
    }

    public void HandleAskOfPlayer(ref byte[] data, ref int conn)
    {
        if (data.Length != 3)
            return; //malformed packet

        //SendPlayer()
        if (!points.ContainsKey(conn))
            return;

        int id = data[1] | (data[2] << 8);

        if (!EntityExists(id))
            return; //don't know wtf they are talking about

        points[conn].needsInfoOn.Add(idToEntity[id].renderInstance); //queue them to receive full update on next packet

    } //end HandleAskOfPlaer

    public void HandlePing(ref byte[] data, ref int conn)
    {
        if (data.Length != 9)
            return; //malformed packet

        //merely tack on another DateTime to the end and re-send it
        byte[] sends = new byte[data.Length + 8];
        Array.Copy(data, 0, sends, 0, data.Length);

        byte[] encoded = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
        Array.Copy(encoded, 0, sends, 9, 8);

        SendPlayer(ref sends, ref conn); //forward ping

    } //end func HandlePing

    public void HandleInputs(ref int conn, ref byte[] data)
    {
        //Logger.Log("received movement " + Ext.ByteToString(data));
        if (data.Length != 9) //opcode, inputnum, code, rot(2), + 12 bytes for Vector3
            return;
        if (!points.ContainsKey(conn))
            return;

        Player target = points[conn];
        target.lastInputNum = data[1] | (data[2] << 8); //set last request num for pinging back later

        if (!target.gameObject.activeSelf) //player is disabled
            return;

        //want to apply the input and code the same order we would on Client

        target.nm.HandleCode(data[3]); //apply inputs, mostly for syncing animations
        target.nm.HandleRotation(data[4], data[5]); //apply rotation

        if (target.ContainsSyncField(SyncSpeed.stamina))
        {
            target.GetComponent<SyncSpeed>().CalcUpdates_Server(); //update stamina and velocity
        }

        //handle according to configured method
        if (setOrHandleInput)
            target.nm.SetCurrentInput(Vector3DeserializeNormalized(ref data, 6));
        else
            target.nm.HandleInput(Vector3DeserializeNormalized(ref data, 6));

        //TODO: add something to drop the player if the point doesn't exist?

    }

    public static void ApplyInput(NetworkMovement nm, ref byte[] data, float overrideSpeed = -1)
    {
        nm.HandleCode(data[3]); //apply inputs, mostly for syncing animations
        nm.HandleRotation(data[4], data[5]); //apply rotation
        nm.HandleInput(Vector3DeserializeNormalized(ref data, 6), overrideSpeed);
    }

    //used by client when client is authorative over their own rotation
    public static void ApplyInput_NoRot(NetworkMovement nm, ref byte[] data)
    {
        nm.HandleCode(data[3]); //apply inputs, mostly for syncing animations
        //nm.HandleRotation(data[3], data[4]); //apply rotation
        nm.HandleInput(Vector3DeserializeNormalized(ref data, 6));
    }
    //used by client when client is authorative over their own rotation
    public static void ApplyInput_NoRot(NetworkMovement nm, ref byte[] data, ref float overrideSpeed)
    {
        nm.HandleCode(data[3]); //apply inputs, mostly for syncing animations
        //nm.HandleRotation(data[3], data[4]); //apply rotation
        nm.HandleInput(Vector3DeserializeNormalized(ref data, 6), overrideSpeed);
    }

    const int rotPos = 2;
    public void HandlePos(ref int conn, ref byte[] data)
    {
        if (data.Length != 16)
            return;

        if (!clientSidePos) //possibly a hack, because client side pos is disabled and have received it anyway
            return;

        if (points.ContainsKey(conn))
            return;

        ApplyPos(points[conn].nm, ref data);
    }

    public static void ApplyPos(NetworkMovement nm, ref byte[] data)
    {
        nm.HandleCode(data[1]);
        nm.HandlePos(ref data, 4, false);
        nm.HandleRotation(ref data, rotPos);
    }

    //used when player asks for their id
    void HandleYourID(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        SendPlayer(new byte[] { netcodes.yourid, (byte)points[conn].id, (byte)(points[conn].id >> 8) }, conn); //tell them their id
    }


    bool CheckAbilitySanity(ref int casted)
    {

        return true;
    }

    void HandleAbility(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        if (data.Length != 31)
        { //TODO: set this properly
            Logger.LogWarning("deformed packet in HandleAbility from " + points[conn].id);
            return;
        }

        int casted = data[1] | data[2] << 8;
        int target = data[3] | data[4] << 8;
        if (!Spell.SpellExists(casted))
        {
            Logger.LogWarning("player tried to cast nonexistant spell");
            return;
        }
        Spell.GetSpell(casted).TryStartCast(EntityExists(target) ? GetEntity(target).e : null, points[conn], Vector3Deserialize(ref data, 7), Vector3Deserialize(ref data, 19));
    }

    void HandleShops(ref byte[] data, ref int conn)
    {
        if (data.Length < 3)
            return;

        int id = data[1] | (data[2] << 8);

        if (!points.ContainsKey(conn))
            return;

        if (!EntityExists(id) || idToEntity[id].e == null)
        { //if doesn't exist or isn't an entity
            Logger.LogWarning("player " + points[conn].id + " asked for shop from entity id " + id + " but that entity does not exist");
            return;
        }

        SendAllShops(points[conn], idToEntity[id].e); //send player all permission viewable shops on target

    } //end func HandleShops

    void HandleShop(ref byte[] data, ref int conn)
    {
        if (data.Length < 5)
            return;

        int id = data[1] | (data[2] << 8);
        int shop = data[3] | data[4] << 8;

        if (points.ContainsKey(conn))
            return;
        SyncObject e;
        if(!TryGetEntity(id, out e))
        {
            Logger.LogWarning(points[conn].displayName + " asked for entity " + id + " which doesn't exist");
            return;
        }

        SendShop(points[conn], e.e.GetFirstShop(shop));

    }

    void HandleVoxeland(ref byte[] data, ref int conn)
    {
#if VOXELAND
        CoordDir tempCord = default(CoordDir);
        Vector3 tempV;
        tempV = Vector3Deserialize(ref data, 1);
        tempCord.x = Mathf.RoundToInt(tempV.x);
        tempCord.y = Mathf.RoundToInt(tempV.y);
        tempCord.z = Mathf.RoundToInt(tempV.z);
        Voxeland.current.Alter(points[conn], tempCord, Voxeland.current.brush, data[14] - 1 < 0 ? Voxeland.EditMode.dig : Voxeland.EditMode.add, data[14] - 1);
#endif
    }


    //list of list of players where index is room and int is conn
    List<int>[] subbedChatChannels;
    public const int chatMaxLength = 301;

    void HandleChat(ref byte[] data, ref int conn)
    {
        if (data.Length > (chatMaxLength * 2) + 4) //2 bytes per char (unicode) plus 4- netcode (1) channel (2) and msgType (1)
            return; //anomolous amount of data...

        if (data.Length < 9)
        {
            Logger.LogWarning("received chat msg too short");
            return;
        }
        if (!points.ContainsKey(conn))
            return;


        HandleChatFrom(ref data, points[conn]);
    } //end func HandleChat

    //pt 2 of handleChat
    //forceNonCommand- prevent recursive command acll with /commmand/command/command/command
    public void HandleChatFrom(ref byte[] data, Player target, bool forceNonCommand = false)
    {
        if (data.Length < 9)
            return; //no data

        int targetID = data[1] | (data[2] << 8); //argument supplied in some chat cases...like whisper
        int msgType = data[3];
        int formatCode = data[4] | (data[5] << 8) | (data[6] << 16) | (data[7] << 24);

        string decoded = DecodeChat(ref data); //apply special formatting

        ChatFormatCodes.GetFrom(ref decoded, ref formatCode);

        if (decoded.Length == 0)
            return;

        if (decoded[0] == '/' && !forceNonCommand) //if prefixed with / and not doing recursion protection
            TryCommand(target, ref msgType, ref targetID, ref decoded);
        else
        { //just a normal message
            BadWordFilter.CleanProfanity(ref decoded);

            //actually prefix data with display name and rank, and make sure to replace all malicious/accidental inclusion of our delimeter
            decoded = target.displayName + delimeters.s_args + target.rank + delimeters.s_args + decoded.Replace(delimeters.s_args, "");

            //re-encode data with display name and rank
            data = EncodeChat(msgType, targetID, formatCode, decoded);

            switch (msgType) {

                case ChatMsgTypes.whisper:
                    if (player != null && targetID == player.point)
                    { //if they wispered the server owner..
                        if (onlineGUI != null) //if server host's chat exists
                            onlineGUI.HandleChatMsg(targetID, msgType, formatCode, ref decoded, this);
                    } else
                    if (points.ContainsKey(targetID)) //if player by id exists
                        SendPlayer(data, targetID); //send to the whisper target
                    break;

                case ChatMsgTypes.normal:

                    if ((targetID <= 0 || targetID >= subbedChatChannels.Length))
                    { //if oob
                        Logger.LogWarning("player " + target.displayName + " (" + target.name + ") id " + target.id + " tried to send to unknown chat channel " + targetID);
                        return;
                    }



                    SendChatAllOnChannel(targetID, msgType, formatCode, decoded, ref data);
                    break;

                default:
                    break; //fell out of switch, received malicious msg type?

            } //end switch

        } //end handle non-command msg

    } //end HandleChatFrom

    public void SendChatAllOnChannel(int channel, int msgType, int formatCode, string decoded, ref byte[] data)
    {
        List<int> channelPlayers = subbedChatChannels[channel];
        for (int i = 0; i < channelPlayers.Count; i++)
        {
            if(channelPlayers[i] == selfFakePointID)
            {
                if (onlineGUI != null) //if server host's chat exists
                    onlineGUI.HandleChatMsg(channel, msgType, formatCode, ref decoded, this); //also receive it locally
            } else
               SendPlayer(data, channelPlayers[i]); //forward msg on channel
        } //end loop over all channels
    }

    public void HandleResetTalents(ref byte[] data, ref int conn)
    {
        if(points.ContainsKey(conn) && points[conn] != null)
        {
            points[conn].e.ResetTalents();
        }
    } //end HandleResetTalents

    //user bought an item from the premium shop through 3rd party and is asking us to update their premium inventory through that 3rd party API
    public void HandleBoughtPremiumItem(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        PremiumItemConfig.UpdateFor_Server(points[conn].source_userID, points[conn].source_username, conn); //ask for items from Kong API or whever through AccountServer (which has the secret api_key) and wait for response with items...
    } //end func HandleBoughtPremiumItem

    public void HandleAskSwapItemPlaces(ref byte[] data, ref int conn)
    {
        if (data.Length != 15)
            return; //malformed packet

        int shopOwner1 = data[1] | (data[2] << 8);
        int shopOwner2 = data[3] | (data[4] << 8);
        int shopID1 = data[5] | (data[6] << 8);
        int shopID2 = data[7] | (data[8] << 8);
        int shopIndex1 = data[9];
        int shopIndex2 = data[10];
        int itemIndex1 = data[11] | (data[12] << 8);
        int itemIndex2 = data[13] | (data[14] << 8);

        if (!points.ContainsKey(conn))
            return; //asking point player doesn't exist

        if (!EntityExists(shopOwner1) || !EntityExists(shopOwner2))
            return; //one of the shop owners doesn't exist

        Entity owner1 = idToEntity[shopOwner1].e;
        Entity owner2 = idToEntity[shopOwner2].e;
        Entity askingPlayer = points[conn];

        if (owner1 == null || owner2 == null || askingPlayer == null)
            return; //owners existed, but as SyncObject, not as entity

        if (askingPlayer != owner1 && askingPlayer != owner2)
            return; //fail sanity, player should own at least one of the shops they're trying to swap items in

        List<ShopInstance> shopsOfType1 = owner1.GetShopsOfType(shopID1);
        List<ShopInstance> shopsOfType2 = owner2.GetShopsOfType(shopID2);

        if (shopIndex1 >= shopsOfType1.Count || shopIndex2 >= shopsOfType2.Count)
            return; //shop index OOB

        ShopInstance shop1 = shopsOfType1[shopIndex1];
        ShopInstance shop2 = shopsOfType2[shopIndex2];

        if (itemIndex1 >= shop1.items.Length || itemIndex2 >= shop2.items.Length)
            return; //item index OOB

        //check permissions and try move item
        if(ShopInstance.SwapItemPlaces(askingPlayer, shop1.items[itemIndex1], shop2.items[itemIndex2], shop1, shop2))
        { //success

        } else
        { //failed
            //try refresh their shops in case something happened...
            askingPlayer.p.TryAddNeedsShop(shop1);
            askingPlayer.p.TryAddNeedsShop(shop2);

        }

    } //end func HandleAskSwapItemPlaces


    public void HandleAskSellItem(ref byte[] data, ref int conn)
    {
        if (data.Length != 13)
            return; //malformed packet

        int shopOwner1 = data[1] | (data[2] << 8);
        int shopOwner2 = data[3] | (data[4] << 8);
        int shopID1 = data[5] | (data[6] << 8);
        int shopID2 = data[7] | (data[8] << 8);
        int shopIndex1 = data[9];
        int shopIndex2 = data[10];
        int itemIndex1 = data[11] | (data[12] << 8);

        if (!points.ContainsKey(conn))
            return; //asking point player doesn't exist

        if (!EntityExists(shopOwner1) || !EntityExists(shopOwner2))
            return; //one of the shop owners doesn't exist

        Entity owner1 = idToEntity[shopOwner1].e;
        Entity owner2 = idToEntity[shopOwner2].e;
        Entity askingPlayer = points[conn];

        if (owner1 == null || owner2 == null || askingPlayer == null)
            return; //owners existed, but as SyncObject, not as entity

        if (askingPlayer != owner1 && askingPlayer != owner2)
            return; //fail sanity, player should own at least one of the shops they're trying to swap items in

        List<ShopInstance> shopsOfType1 = owner1.GetShopsOfType(shopID1);
        List<ShopInstance> shopsOfType2 = owner2.GetShopsOfType(shopID2);

        if (shopIndex1 >= shopsOfType1.Count || shopIndex2 >= shopsOfType2.Count)
            return; //shop index OOB

        ShopInstance shop1 = shopsOfType1[shopIndex1];
        ShopInstance shop2 = shopsOfType2[shopIndex2];

        if (itemIndex1 >= shop1.items.Length)
            return; //item index OOB

        //check permissions and try move item
        if (VendorShop.TrySellItemTo(shop1, shop2, shop2.items[itemIndex1]))
        { //success

        }
        else
        { //failed

        }

        askingPlayer.p.TryAddNeedsShop(shop1);
        askingPlayer.p.TryAddNeedsShop(shop2);

    } //end func HandleAskSwapItemPlaces



    void HandleTryBuy(ref byte[] data, ref int conn)
    {
        if (data.Length < 11)
            return;

        if (!points.ContainsKey(conn))
            return;

        int owner = data[1] | (data[2] << 8);
        int shopID = data[3] | data[4] << 8;
        int itemID = data[5] | (data[6] << 8);
        int count = data[7] | (data[8] << 8);
        int index = data[9] | (data[10] << 8);

        if (!Shop.ShopExists(shopID))
            return;

        if (!EntityExists(owner))
            return;


        Entity e = idToEntity[owner].e;
        if (e == null)
            return;
        ShopInstance shop = e.GetFirstShop(shopID);

        if (index < 0 || index >= shop.items.Length)
            return;

        if (shop.items[index].item == null)
            return;

        if(shop.items[index].item.id != itemID)
        {
            //item indexes should not be shifting when you buy items... instead, the item should revert to a null/empty shopItemInstance
            //either that, or a malicious client or malformed packet
            Debug.LogWarning("mismatch item index during Server.TryBuy"); 
            return;
        }

        if (shop != null)
            shop.shop.TryBuy(e, points[conn], shop, shop.items[index], count);
    }

    const int bulkBuyLimit = 1000; //shouldn't need to bulk buy more than that num different items, and shouldn't have any consequences since it won't charge buyer
                                   /*
                                   void HandleTryBuyBulk(ref byte[] data, ref int conn)
                                   {

                                       int owner = data[3] | (data[4] << 8);
                                       int shopID = data[5] | data[6] << 8;

                                       if (!Shop.ShopExists(shopID))
                                           return;

                                       Entity e = idToEntity[owner].e;

                                       int count = 0;
                                       Shop shop = null;
                                       for (int i = 7; i < data.Length && count < bulkBuyLimit; i += 4, count++)
                                       {

                                       }
                                   }
                                   */

    void HandleTryBuyBulk(ref byte[] data, ref int conn)
    {
        if (data.Length < 5)
            return;

        int buyerID = data[1] | (data[2] << 8);
        int ownerID = data[3] | (data[4] << 8);

        if (!EntityExists(buyerID) || !EntityExists(ownerID))
            return;

        Entity buyer = idToEntity[buyerID].e;
        Entity owner = idToEntity[ownerID].e;

        if (owner == null || buyer == null)
            return; //wasn't an entity

        int itemID;
        int shopID = 0;
        ShopInstance shop = null;
#pragma warning disable CS0168 // The variable 'item' is declared but never used
        Item item; 
#pragma warning restore CS0168 // The variable 'item' is declared but never used
        List<ShopInstance> updatedShops = new List<ShopInstance>(); //keep track of what gets updated so we can trigger updates in batch rather than after each buy
        for (int i = 5, count = 0; i < data.Length && count < bulkBuyLimit; i += 4, count++)
        {
            if (i + 3 >= data.Length)
                return;

            itemID = data[i] | (data[i + 1] << 8);
            shopID = data[i + 2] | (data[i + 3] << 8);

            if (!Shop.ShopExists(shopID) || !WorldFunctions.ItemExists(itemID)) //sanity check the shop and item
                return;
            shop = owner.GetFirstShop(shopID);
            if (shop != null)
            {
                if(shop.shop.TryBuy(owner, buyer, shop, shop.GetFirst(itemID), 1)) { //if successfully bought (1 item)
                    if (!updatedShops.Contains(shop))
                        updatedShops.Add(shop); //add to list of modified shops
                }
            }
        } //end loop over items

        for(int i = 0; i < updatedShops.Count; i++)
        {
            owner.TriggerOnRefreshShop(shop); //send updates to client
        }

    } //end func HandleBuyBulk

    void HandleMonetizationStart(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        if(MonetizationConfig.instance != null)
            MonetizationConfig.instance.OnMonetized_Server(points[conn]);
    } //end func HandleMonetizationStart

    void HandleRewardAdStart(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        if (data.Length != 9) //netcode + (long) 8 bytes, representing DateTime.UtcNow
            return; //malformed packet

        long ticks = BitConverter.ToInt64(data, 1);

        if (ticks >= DateTime.MaxValue.Ticks || ticks <= 0)
            return; //protect against malicious crash-inducing ticks

        DateTime started = new DateTime(ticks);

        AdLoader.OnAskRewardAdStart_Server(started, points[conn]);
    } //end func HandleRewardAdStart

    void HandleRewardAdFinish(ref byte[] data, ref int conn)
    {
        if (!points.ContainsKey(conn))
            return;

        if (data.Length != 17) //netcode + (long) 8 bytes, representing DateTime.UtcNow, + (ulong) 8 bytes, represending req_id
            return; //malformed packet

        long ticks = BitConverter.ToInt64(data, 1);
        ulong req_id = BitConverter.ToUInt64(data, 9);

        if (ticks >= DateTime.MaxValue.Ticks || ticks <= 0)
            return; //protect against malicious crash-inducing ticks

        DateTime timestamp = new DateTime(ticks);

        AdLoader.OnAskRewardAdFinish_Server(timestamp, req_id, points[conn]);
    } //end func HandleRewardAdFinish


} //end class
