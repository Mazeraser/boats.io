using System;
using Codebase.Infrastructure.InputService;
using Codebase.Mechanics.Life_system;
using Codebase.Mechanics.Shoot_system;
using UnityEngine;

namespace Codebase.Mechanics.Condition_system
{
    /// <summary>
    /// Класс, отвечающий за состояние игрока
    /// </summary>
    public class PlayerConditionSystem : MonoBehaviour
    {
        private enum PlayerConditions
        {
            attack=0,
            defend=1,
            third=2,
        }
        private PlayerConditions _condition;

        public int Condition
        {
            get
            {
                return (int)_condition;
            }
        }

        private Conditions _input;

        [SerializeField] 
        private GameObject _gun;
        [SerializeField] 
        private SpriteRenderer _bound;

        private void Awake()
        {
            if (!Application.isMobilePlatform)
                _input = new Conditions();
        }
        private void OnEnable()
        {
            _input?.Enable();
        }
        private void OnDisable()
        {
            _input?.Disable();
        }

        private void Start()
        {
            SetCondition(0);
            if (Application.isMobilePlatform)
                MobileInput.SetConditionByButton += SetCondition;
            else
            {
                _input.Main.First.performed += context => SetCondition(0);
                _input.Main.Second.performed += context => SetCondition(1);
                _input.Main.Third.performed += context => SetCondition(2);
            }
        }

        private void OnDestroy()
        {
            if (Application.isMobilePlatform)
                MobileInput.SetConditionByButton -= SetCondition;
        }

        public void SetCondition(int id)
        {
            _condition = (PlayerConditions)id;
            switch ((int)_condition)
            {
                case 0:
                    GetComponent<BoatLifeComponent>().enabled = true;
                    _gun.SetActive(true);
                    _bound.color = Color.yellow;
                    break;
                case 1:
                    GetComponent<BoatLifeComponent>().enabled = false;
                    _gun.SetActive(false);
                    _bound.color = Color.cyan;
                    break;
                case 2:
                    _bound.color = Color.green;
                    break;
            }
        }
    }
}