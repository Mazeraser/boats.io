using System;
using UnityEngine;
using Codebase.Infrastructure.Animator;

namespace Codebase.Mechanics.Life_system
{
    public class BoatLifeComponent : MonoBehaviour, ILive
    {
        [SerializeField]
        private int _maxHealth = 100;
        
        private int _currentHealth;

        public int HP
        {
            get { return _currentHealth; }
            private set
            {
                _currentHealth = value;
                if (_currentHealth <= 0)
                    Death();
            }
        }

        [ContextMenu("Kill")]
        public void Kill()
        {
            GetDamage(_maxHealth);
        }
        public void GetDamage(int damage)
        {
            HP = _currentHealth - damage;
        }

        public void Death()
        {
            Debug.Log("Boat is death");
            GetComponent<IAnimator>()?.Death(()=>{Destroy(gameObject);});
        }

        private void Start()
        {
            _currentHealth = _maxHealth;
        }
    }
}