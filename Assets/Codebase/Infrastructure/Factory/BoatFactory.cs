using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace Codebase.Infrastructure.Factory
{
    public class BoatFactory : MonoBehaviourPunCallbacks, IFactory<GameObject>
    {
        public static event Action<int> PlayerEnterTheGameEvent;
        
        private List<GameObject> _players = new List<GameObject>();
        private List<GameObject> _bots = new List<GameObject>();
        public List<GameObject> Entities
        {
            get { return _players; }
        }

        [SerializeField] 
        private GameObject _playerPrefab;
        [SerializeField] 
        private GameObject _botPrefab;
        [SerializeField] 
        private int _arenaSize;
        [SerializeField]
        [Tooltip("Зона, в которой не должно быть игроков")]
        private float _freeRadius;
        [SerializeField] 
        private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] 
        private int _boatCount;

        public GameObject CreateProduct(GameObject prefab)
        {
            Vector3 position = new Vector3(0,0,0);
                
            GameObject player = PhotonNetwork.Instantiate(prefab.name,position, Quaternion.identity);
            _players.Add(player);
            PlayerEnterTheGameEvent?.Invoke(_bots.Count);
            return player;
        }

        public async UniTask<GameObject> CreateBot(GameObject prefab)
        {
            Vector3 position = await FindSpawnPosition();
            GameObject bot = PhotonNetwork.Instantiate(prefab.name, position, Quaternion.identity);
            _bots.Add(bot);
            return bot;
        }

        private void DestroyBot()
        {
            Destroy(_bots[0]);
            _bots.RemoveAll(item => item == null);
        }

        private async UniTask<Vector3> FindSpawnPosition()
        {
            Vector3 position = new Vector3(0,0,0);
            bool flag = true;
            while (flag)
            {
                position= new Vector3(Random.Range(-1*(_arenaSize/2-1), _arenaSize/2), Random.Range(-1*(_arenaSize/2-1), _arenaSize/2), 0);
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, _freeRadius);
                foreach (var environment in hitColliders)
                {
                    if (!environment.CompareTag("Player"))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
            }
            return position;
        }

        private async void Start()
        {
            Vector3 playerPosition = await FindSpawnPosition();
            GameObject player = CreateProduct(_playerPrefab);
            player.transform.position = playerPosition;
            _virtualCamera.Follow = player.transform;
        }

        private async void Update()
        { 
            if (_players.Count + _bots.Count < _boatCount)
                await CreateBot(_botPrefab);
            else if (_players.Count + _bots.Count > _boatCount && _boatCount!=0) 
                DestroyBot();
        }
        
        void OnGUI()
        {
            if (PhotonNetwork.CurrentRoom == null)
                return;

            if (GUI.Button(new Rect(5, 5, 125, 25), "Leave Room"))
            {
                PhotonNetwork.LeaveRoom();
            }

            GUI.Label(new Rect(135, 5, 200, 25), PhotonNetwork.CurrentRoom.Name);
            
            //Show the list of the players connected to this Room
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
                string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
                GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
            }//Show the list of the players connected to this Room
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                //Show if this player is a Master Client. There can only be one Master Client per Room so use this to define the authoritative logic etc.)
                string isMasterClient = (PhotonNetwork.PlayerList[i].IsMasterClient ? ": MasterClient" : "");
                GUI.Label(new Rect(5, 35 + 30 * i, 200, 25), PhotonNetwork.PlayerList[i].NickName + isMasterClient);
            }
        }

        public override void OnLeftRoom()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }
    }
}