using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;

namespace Codebase.Infrastructure.Factory
{
    public class EnvironmentFactory : MonoBehaviour, IFactory<GameObject>
    {
        [SerializeField]
        private GameObject _environmentPrefab;
        [SerializeField] 
        private int _arenaSize;
        [SerializeField] 
        private int _environmentLimit;
        
        [Space] 
        [SerializeField] 
        private float _minSize;
        [SerializeField]
        private float _maxSize;
        
        private List<GameObject> _entities = new List<GameObject>();
        public List<GameObject> Entities
        {
            get { return _entities; }
        }

        public GameObject CreateProduct(GameObject prefab)
        {
            Vector3 position = new Vector3(Random.Range(-1*(_arenaSize/2-1), _arenaSize/2), Random.Range(-1*(_arenaSize/2-1), _arenaSize/2), 0);
            GameObject entity = Instantiate(prefab, position, Quaternion.identity);
            float entityScale = Random.Range(_minSize, _maxSize);
            entity.transform.localScale = new Vector3(entityScale,entityScale,1);
            return entity;
        }

        private void Start()
        {
            gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        private void Update()
        {
            if (Entities.Count < _environmentLimit)
            {
                _entities.Add(CreateProduct(_environmentPrefab));
            }
        }
    }
}