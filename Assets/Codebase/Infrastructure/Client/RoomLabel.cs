using TMPro;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

namespace Codebase.Infrastructure.Client
{
    /// <summary>
    /// Класс, описывающий комнату
    /// </summary>
    public class RoomLabel : MonoBehaviour
    {
        public static Action<string> RoomSelectedEvent;
        
        [SerializeField] 
        private TMP_Text _roomNameText;
        [SerializeField] 
        private TMP_Text _playerCountText;
        [SerializeField] 
        private Button _selectRoom;

        public string RoomName
        {
            get { return _roomNameText.text; }
        }

        public void SetRoomInfo(RoomInfo info)
        {
            _roomNameText.text = info.Name;
            _playerCountText.text = info.PlayerCount + "/" + info.MaxPlayers;
            Debug.Log(_roomNameText.text+" "+info.PlayerCount + "/" + info.MaxPlayers);
        }

        private void Start()
        {
            _selectRoom.onClick.AddListener(() => { RoomSelectedEvent?.Invoke(RoomName); });
        }
    }
}