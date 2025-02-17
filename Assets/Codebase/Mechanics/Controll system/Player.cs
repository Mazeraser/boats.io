using System;
using Codebase.Infrastructure.Animator;
using Codebase.Mechanics.MoveSystem;
using Codebase.Infrastructure.InputService;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Codebase.Mechanics.ControllSystem
{
    [RequireComponent(typeof(Move))]
    public class Player : MonoBehaviour,IControllable
    {
        private IInput _inputAction;
        [SerializeField] 
        private GameObject _mobileInput;

        private void Awake()
        {
            if (Application.isMobilePlatform)
            {
                GameObject inputUI = Instantiate(_mobileInput);
                _inputAction = inputUI.GetComponent<IInput>();
            }
            else
                _inputAction = new KeyboardInput();
            _inputAction.Activate();
        }

        private void OnDisable()
        {
            _inputAction.Deactivate();
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().color =
                new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1);
        }

        private void FixedUpdate()
        {
            ControllMove(_inputAction.Velocity);
        }

        public void ControllMove(Vector2 direction)
        {
            GetComponent<IMovable>().Turn(direction);
        }
    }
}