using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codebase.Mechanics.MoveSystem
{
    public class Move : MovableParent, IMovable
    {
        [SerializeField]
        private float _moveSpeed;
        public float Speed
        {
            get { return _moveSpeed; }
        }

        public void Turn(Vector2 direction)
        {
            Move(direction, _moveSpeed);
        }
    }
}