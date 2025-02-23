using System;
using UnityEngine;
using Codebase.Infrastructure.Animator;

namespace Codebase.Mechanics.Feed_system
{
    /// <summary>
    /// Класс единки
    /// </summary>
    public class Food : MonoBehaviour
    {
        public delegate void Eated(Transform objectTransform, IAnimator animator);
        public Eated EatedDelegate;
        
        [SerializeField]
        private float _calory;
        public float Calory
        {
            get
            {
                return _calory;
            }
        }

        public void Eat()
        {
            EatedDelegate?.Invoke(transform, GetComponent<IAnimator>());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.name.Contains("Block"))
            {
                Eat();
            }
        }
    }
}