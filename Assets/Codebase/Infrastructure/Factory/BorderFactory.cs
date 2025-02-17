using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Infrastructure.Factory
{
    public class BorderFactory : MonoBehaviour, IFactory<GameObject>
    {
        [SerializeField] 
        private GameObject _wallPrefab;
        
        private List<GameObject> _entities = new List<GameObject>();
        public List<GameObject> Entities
        {
            get { return _entities; }
        }

        private void Start()
        {
            CreateProduct(_wallPrefab);
        }
        
        public GameObject CreateProduct(GameObject prefab)
        {
            float wallPos = (float)(transform.localScale.x/2f);
            
            GameObject leftWall = Instantiate(_wallPrefab,
                new Vector3(0f,-1*wallPos,0f),
                Quaternion.identity);
            GameObject rightWall = Instantiate(_wallPrefab,
                new Vector3(0f,wallPos,0f),
                Quaternion.identity);
            GameObject topWall = Instantiate(_wallPrefab,
                new Vector3(wallPos,0f,0f),
                Quaternion.identity);
            GameObject botWall = Instantiate(_wallPrefab,
                new Vector3(-1*wallPos,0f,0f),
                Quaternion.identity);
            
            leftWall.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            rightWall.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            topWall.transform.localScale = new Vector3(1, transform.localScale.y, 1);
            botWall.transform.localScale = new Vector3(1, transform.localScale.y, 1);
            
            _entities.Add(leftWall);
            _entities.Add(rightWall);
            _entities.Add(topWall);
            _entities.Add(botWall);

            return leftWall;
        }
    }
}