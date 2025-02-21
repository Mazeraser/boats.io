using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Codebase.Infrastructure.Server
{
    public class ServerGameLobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] 
        private TMP_Text _activePlayersCount;
        [SerializeField] 
        private TMP_Text _serverAddressField;
        
        List<RoomInfo> _createdRooms = new List<RoomInfo>();

        private void Start()
        {
            PhotonNetwork.JoinLobby();
        }

        private void Update()
        {
            _serverAddressField.text = "Server address: " + PhotonNetwork.ServerAddress;
            _activePlayersCount.text = "Players online: " + PhotonNetwork.CountOfPlayers;
        }

        // Photon Methods
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
        }
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            //After we connected to Master server, join the Lobby
            PhotonNetwork.JoinLobby(TypedLobby.Default);
        }
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("We have received the Room list");
            _createdRooms = roomList;
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log($"OnCreateRoomFailed: {message}");
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            PhotonNetwork.LoadLevel("Arena");
        }
        
        //Lobby methods
        public void CreateRoom(TMP_InputField roomName)
        {
            string name = roomName.text == "" ? "room_" + _createdRooms.Count +"_"+PhotonNetwork.CloudRegion : roomName.text;
            PhotonNetwork.CreateRoom(name,new RoomOptions{MaxPlayers = 20, IsVisible = true});
        }
    }
}