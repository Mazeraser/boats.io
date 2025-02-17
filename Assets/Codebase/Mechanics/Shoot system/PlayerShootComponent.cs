using System;
using Codebase.Infrastructure.InputService;
using Photon.Pun;
using UnityEngine;

namespace Codebase.Mechanics.Shoot_system
{
    public class PlayerShootComponent : MonoBehaviour
    {
        [SerializeField] 
        private GameObject _bulletPrefab;
        [SerializeField] 
        private Transform _muzzle;

        private Shoot _input;

        private void Awake()
        {
            _input = new Shoot();
        }
        private void OnEnable()
        {
            _input.Enable();
        }
        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Application.isMobilePlatform ? Input.GetTouch(0).position : Input.mousePosition) - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ-90f);
            if (_input.Main.Fire.WasPerformedThisFrame())
                PhotonNetwork.Instantiate(_bulletPrefab.name, _muzzle.position, transform.rotation);
        }
    }
}