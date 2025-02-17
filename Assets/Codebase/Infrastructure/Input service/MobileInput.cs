using UnityEngine;
using UnityEngine.UI;
using System;

namespace Codebase.Infrastructure.InputService
{
    public class MobileInput : MonoBehaviour, IInput
    {
        public static event Action<int> SetConditionByButton;
        
        [SerializeField] 
        private Joystick _joystick;
        [SerializeField] 
        private Button[] _conditionButtons;

        private void Start()
        {
            for(int i=0;i<_conditionButtons.Length;i++)
                _conditionButtons[i].onClick.AddListener(() => { SetConditionByButton?.Invoke(i);});
        }

        public Vector2 Velocity
        {
            get { return new Vector2(_joystick.Horizontal, _joystick.Vertical); }
        }

        public void Activate()
        {
            _joystick.gameObject.SetActive(true);
            foreach(var button in _conditionButtons)
                button.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _joystick.gameObject.SetActive(false);
            foreach(var button in _conditionButtons)
                button.gameObject.SetActive(false);
        }
    }
}