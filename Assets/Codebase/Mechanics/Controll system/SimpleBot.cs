using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Codebase.Mechanics.MoveSystem;
using Cysharp.Threading.Tasks;
using Codebase.Infrastructure.Animator;

namespace Codebase.Mechanics.ControllSystem
{
    [RequireComponent(typeof(Move))]
    public class SimpleBot : MonoBehaviour, IControllable
    {
        [SerializeField]
        private float _moveTime;

        private bool _changeDirection = true;
        private Vector2 _moveDirection;

        private async void FixedUpdate()
        {
            if (_changeDirection)
            {
                _moveDirection = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
                _changeDirection = false;
                await UniTask.WaitForSeconds(_moveTime);
                _changeDirection = true;
            }
            ControllMove(_moveDirection);
        }

        public void ControllMove(Vector2 direction)
        {
            GetComponent<IMovable>().Turn(direction);
        }
    }
}