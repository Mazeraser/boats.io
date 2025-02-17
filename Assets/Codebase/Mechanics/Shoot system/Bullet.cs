using System;
using UnityEngine;
using System.Collections;
using Codebase.Mechanics.Life_system;

namespace Codebase.Mechanics.Shoot_system
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _timeDestroy = 3f;
        [SerializeField]
        private float _speed = 50f;
        [SerializeField] 
        private int _dmg = 10;
        
        private Rigidbody2D _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.velocity = transform.up * _speed;
            StartCoroutine(DestroyBullet(_timeDestroy));
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.GetComponent<ILive>()?.GetDamage(_dmg);
            Destroy(gameObject);
        }

        private IEnumerator DestroyBullet(float timer)
        {
            yield return new WaitForSeconds(timer);
            Destroy(gameObject);
        }
    }
}