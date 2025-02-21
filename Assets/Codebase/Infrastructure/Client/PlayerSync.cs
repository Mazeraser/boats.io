using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Codebase.Infrastructure.Server;
using Codebase.Mechanics.Condition_system;
using ExitGames.Client.Photon;
using TMPro;

namespace Codebase.Infrastructure.Client
{
    /// <summary>
    /// Класс для работы игрока с сервером
    /// </summary>
    public class PlayerSync: MonoBehaviourPun, IPunObservable, IOnEventCallback
    {
        public const byte SendEnvironmentToPlayerCode = 1;
        public const byte SetEnvironmentCode = 2;

        //List of the scripts that should only be active for the local player
        public MonoBehaviour[] _localScripts;
        //List of the GameObjects that should only be active for the local player
        public GameObject[] _localObjects;

        [SerializeField] 
        private TMP_Text _playerNick;
        
        [SerializeField]
        private float _visionRadius=2f;

        [SerializeField] 
        private float _updateEnvironmentTime = 1.5f;
        private float _timer;

        private List<GameObject> _prevNearestObjects = new List<GameObject>();
        private List<GameObject> _nearestObjects = new List<GameObject>();
        
        Vector3 _latestPos;
        Quaternion _latestRot = Quaternion.identity;

        void Start()
        {
            if (!photonView.IsMine)
            {
                //Player is Remote, deactivate the scripts and object that should only be enabled for the local player
                for (int i = 0; i < _localScripts.Length; i++)
                {
                    _localScripts[i].enabled = false;
                }
                for (int i = 0; i < _localObjects.Length; i++)
                {
                    _localObjects[i].SetActive(false);
                }
            }
            else
                SendEnvironmentToPlayer();
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                transform.position = Vector3.Lerp(transform.position, _latestPos, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, _latestRot, Time.deltaTime * 5);
            }
            else
            {
                photonView.RPC(nameof(SyncNick), RpcTarget.All, PhotonNetwork.NickName);
                photonView.RPC(nameof(SyncCondition), RpcTarget.All, GetComponent<PlayerConditionSystem>().Condition);

                _timer += Time.deltaTime;
                if (_timer >= _updateEnvironmentTime)
                {
                    SendEnvironmentToPlayer();
                    _timer = 0;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _latestPos = (Vector3)stream.ReceiveNext();
                _latestRot = (Quaternion)stream.ReceiveNext();
            }
        }
        
        
        private void SendEnvironmentToPlayer()
        {
            Debug.Log("Setting environment start...");
            float screenProportion = Mathf.Clamp(Screen.width / Screen.height, 0.25f, 4f);
            float dividableProportion = Mathf.Sqrt(screenProportion);

            float visionRadiusX = _visionRadius * dividableProportion;
            float visionRadiusY = _visionRadius / dividableProportion;

            object[] eventData = new object[]
            {
                transform.position.x, transform.position.y, transform.position.z,
                visionRadiusX, visionRadiusY,
                photonView.ViewID
            };

            PhotonNetwork.RaiseEvent(SendEnvironmentToPlayerCode, eventData, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
        }

        private void OnEnable() 
        {
            Debug.Log("[PlayerSync] Подписываемся на события...");
            PhotonNetwork.AddCallbackTarget(this);
        }
        private void OnDisable() 
        {
            Debug.Log("[PlayerSync] Отписываемся от событий...");
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            if(photonEvent.Code < 200)
                Debug.Log($"[PlayerSync] Получено событие {photonEvent.Code}");
            if (photonEvent.Code == SetEnvironmentCode)
            {
                Debug.Log("Setting environment end...");
                object[] data = (object[])photonEvent.CustomData;

                int viewID = (int)data[0]; // Получаем ID игрока

                // Проверяем, что событие предназначено этому игроку
                if (photonView.ViewID != viewID)
                    return;

                Debug.Log("Получены объекты окружения!");

                for (int i = 1; i < data.Length; i += 3) // 3 элемента на 1 объект
                {
                    string prefabName = (string)data[i];
                    int posX = (int)data[i + 1];
                    int posY = (int)data[i + 2];

                    Debug.Log($"Создаю объект: {prefabName} на позиции ({posX}, {posY})");

                    _nearestObjects.Add(Instantiate(Resources.Load<GameObject>(prefabName), new Vector3(posX, posY, 0), Quaternion.identity));
                }
            }
        }
        
        [PunRPC]
        private void SyncNick(string nickName)
        {
            _playerNick.text = nickName;
        }
        [PunRPC]
        private void SyncCondition(int condition)
        {
            GetComponent<PlayerConditionSystem>().SetCondition(condition);
        }
    }
}