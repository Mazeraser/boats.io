using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Codebase.Infrastructure.InputService
{
    public class KeyboardInput : IInput
    {

        private Main _inputActions;

        public KeyboardInput()
        {
            _inputActions = new Main();
        }

        public void Activate()
        {
            _inputActions.Enable();
        }
        public void Deactivate()
        {
            _inputActions.Disable();
        }

        public Vector2 Velocity 
        { 
            get { return _inputActions.Game.Move.ReadValue<Vector2>(); } 
        }
    }
}
    
