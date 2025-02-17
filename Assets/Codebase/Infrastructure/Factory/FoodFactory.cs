using Codebase.Mechanics.Feed_system;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Photon.Pun;

namespace Codebase.Infrastructure.Factory
{
    public class FoodFactory : MonoBehaviour, IFactory<Food>
    {
        [SerializeField]
        private GameObject _foodPrefab;
        [SerializeField] 
        private int _foodLimit;
        [SerializeField] 
        private int _arenaSize;
        
        private List<Food> _entities = new List<Food>();
        public List<Food> Entities
        {
            get{ return _entities; }
        }

        public Food CreateProduct(GameObject prefab)
        {
            GameObject entity = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
            entity.GetComponent<Food>().EatedDelegate +=ReplaceFood;
            ReplaceFood(entity.transform);
            return entity.GetComponent<Food>();
        }

        private void ReplaceFood(Transform foodTransform)
        {
            foodTransform.position = new Vector3(Random.Range(-1 * (_arenaSize / 2 - 1), _arenaSize / 2),
                    Random.Range(-1 * (_arenaSize / 2 - 1), _arenaSize / 2), 
                    0);
        }

        private async void Update()
        {
            if (Entities.Count < _foodLimit)
            {
                await GenerateFood(Mathf.Clamp(_foodLimit-Entities.Count,0,5));
            }
        }

        private async UniTask GenerateFood(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                _entities.Add(CreateProduct(_foodPrefab));
            }
        }
    }
}