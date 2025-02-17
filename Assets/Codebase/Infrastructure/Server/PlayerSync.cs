using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Codebase.Infrastructure.Animator;
using Codebase.Mechanics.Condition_system;
using TMPro;

namespace Codebase.Infrastructure.Server
{
    /// <summary>
    /// Класс для работы игрока с сервером
    /// </summary>
    public class PlayerSync: MonoBehaviourPunCallbacks, IPunObservable
    {

        //List of the scripts that should only be active for the local player
        public MonoBehaviour[] _localScripts;
        //List of the GameObjects that should only be active for the local player
        public GameObject[] _localObjects;

        [SerializeField] 
        private TMP_Text _playerNick;
        
        [SerializeField]
        private float _visionRadius=2f;

        private IAnimator[] _prevNearestObjects;
        private IAnimator[] _nearestObjects;
        
        Vector3 _latestPos;
        Quaternion _latestRot;
        
        void Start()
        {
            if (photonView.IsMine)
            {
                //Player is local
                _nearestObjects = SendEnvironmentToPlayer();
            }
            else
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
        
        private IAnimator[] SendEnvironmentToPlayer()
        {
            //Так как клиент может прислать любое число, нужно быть на чеку, чтобы число было адекватным.
            float screenProportion = Mathf.Clamp(Screen.width/Screen.height, 0.25f, 4f);
            float dividableProportion = Mathf.Sqrt(screenProportion);
            //радиусы/дистанции видимости по осям:
            float visionRadiusX = _visionRadius * (dividableProportion);
            float visionRadiusY = _visionRadius / (dividableProportion);

            Collider2D[] environment = Physics2D.OverlapBoxAll(transform.position,new Vector2(visionRadiusX,visionRadiusY),0); //Некая функция сканирования зоны
            //Сканироваться должна зона которая больше заданных рамок
            List<IAnimator> selectedElements = new List<IAnimator>();

            foreach (var element in environment) {

                if (element.CompareTag("Environment")) {
                    selectedElements.Add(element.gameObject.GetComponent<IAnimator>());
                }
                else {
                    continue;
                }
            }

            return selectedElements.ToArray();
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
                if (_nearestObjects != null)
                    _prevNearestObjects = _nearestObjects;
                _nearestObjects = SendEnvironmentToPlayer();
                foreach (var nearest in _nearestObjects)
                {
                    nearest.Born();
                }

                foreach (var environment in _prevNearestObjects)
                {
                    if(!_nearestObjects.Contains(environment))
                        environment.Death();
                }
            }
        }
        
        void OnDrawGizmosSelected()
        {
            float screenProportion = Mathf.Clamp(Screen.width/Screen.height, 0.25f, 4f);
            float dividableProportion = Mathf.Sqrt(screenProportion);
            
            float visionRadiusX = _visionRadius * (dividableProportion);
            float visionRadiusY = _visionRadius / (dividableProportion);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(visionRadiusX, visionRadiusY, 1));
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