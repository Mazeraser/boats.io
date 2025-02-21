using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Codebase.Infrastructure.Client
{
    /// <summary>
    /// Лобби для подключения к комнатам
    /// </summary>
    public class ClientGameLobby : MonoBehaviourPunCallbacks
    {
        string _playerName;
        [SerializeField]
        private GameObject _roomList;
        [SerializeField] 
        private GameObject _roomLabel;
        
        List<RoomInfo> _createdRooms = new List<RoomInfo>();
        [SerializeField]
        private TMP_Text _selectedRoomNameField;
        [SerializeField] 
        private TMP_Text _activePlayersCount;
        [SerializeField] 
        private TMP_Text _serverAddressField;
        
        private int _roomCounter = 0;

        private void Start()
        {
            PhotonNetwork.JoinLobby();
            RoomLabel.RoomSelectedEvent += SetSelectedRoom;
        }

        private void Update()
        {
            _serverAddressField.text = "Server address: " + PhotonNetwork.ServerAddress;
            _activePlayersCount.text = "Players online: " + PhotonNetwork.CountOfPlayers;
        }

        private void OnDestroy()
        {
            RoomLabel.RoomSelectedEvent -= SetSelectedRoom;
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
            foreach (var room in _createdRooms)
            {
                GameObject label = Instantiate(_roomLabel,_roomList.transform);
                label.GetComponent<RoomLabel>().SetRoomInfo(room);
            }
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log($"OnCreateRoomFailed: {message}");
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            PhotonNetwork.LoadLevel("Arena");
        }
        
        //Lobby methods
        private void SetSelectedRoom(string roomName)
        {
            _selectedRoomNameField.text = roomName;
        }
        
        public void JoinRoom(TMP_Text playerNameField)
        {
            _playerName = playerNameField.text.Length<=1 ? "Player_"+Random.Range(0,2000):playerNameField.text;
            PhotonNetwork.NickName = _playerName==""?"Player":_playerName;

            if (PhotonNetwork.CountOfRooms == 0)
                Debug.LogError("Rooms doesn't exist, please, create room on server");
            else
                PhotonNetwork.JoinRoom(_createdRooms[Random.Range(0,_createdRooms.Count)].Name);
        }

        public void JoinSelectedRoom(TMP_Text playerNameField)
        {
            _playerName = playerNameField.text.Length<=1 ? "Player_"+Random.Range(0,2000):playerNameField.text;
            PhotonNetwork.NickName = _playerName==""?"Player":_playerName;

            PhotonNetwork.JoinRoom(_selectedRoomNameField.text);
        }

        private void CreateRoom()
        {
            string roomName = "room_"+_roomCounter;
            
            PhotonNetwork.CreateRoom(roomName,new RoomOptions{MaxPlayers = 20, IsVisible = true});
        }

        public void RefreshList()
        {
            if (PhotonNetwork.IsConnected)
            {
                //Re-join Lobby to get the latest Room list
                PhotonNetwork.JoinLobby(TypedLobby.Default);
            }
            else
            {
                //We are not connected, estabilish a new connection
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }
}