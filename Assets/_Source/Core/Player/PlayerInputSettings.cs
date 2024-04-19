using System;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Player
{
    public class PlayerInputSettings : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private const string WasdBindingKey = "Wasd";
        private const string ArrowsBindingKey = "Arrows";

        private InputType _currentInputType = InputType.Wasd;
        
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.defaultActionMap = WasdBindingKey;
            if (!PlayerPrefs.HasKey(ChangeInput.InputSettingsKey))
            {
                PlayerPrefs.SetInt(ChangeInput.InputSettingsKey, (int)InputType.Wasd);
                PlayerPrefs.Save();
            }

            _currentInputType = (InputType)PlayerPrefs.GetInt(ChangeInput.InputSettingsKey);
            ChangeInputType(_currentInputType);
        }

        public void ActivateArrows() => ChangeInputType(InputType.Arrows);
        public void ActivateWasd() => ChangeInputType(InputType.Wasd);

        public void ChangeInputType(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.Wasd:
                    _playerInput.SwitchCurrentActionMap(WasdBindingKey);
                    break;
                case InputType.Arrows:
                    _playerInput.SwitchCurrentActionMap(ArrowsBindingKey);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputType), inputType, null);
            }
            _currentInputType = inputType;
            PlayerPrefs.SetInt(ChangeInput.InputSettingsKey, (int)inputType);
        }
    }

    public enum InputType
    {
        Wasd = 0,
        Arrows = 1,
    }
}
