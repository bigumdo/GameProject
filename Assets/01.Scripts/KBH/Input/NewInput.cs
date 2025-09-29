using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Object = UnityEngine.Object;

namespace Kbh
{    
    public static class NewInput
    {
        /* Path */
        private const string INPUT_ASSET_ADDRESS = "Setting/InputControl";
        private static InputActionAsset _inputAsset = null;

        /* Input Objects */
        private static PlayerInput PlayerObject { get; set; } = new PlayerInput();
        public static PlayerInput ConnectPlayer(MonoBehaviour playerMono)
        {
            PlayerObject.SetEnable(playerMono);
            return PlayerObject;
        }
        public static void DisconnectPlayer(MonoBehaviour playerMono)
        {
            PlayerObject.SetDisable(playerMono);
        }



        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            var operation = Addressables.LoadAssetAsync<InputActionAsset>(INPUT_ASSET_ADDRESS);
            operation.Completed += op =>
            {
                _inputAsset = op.Result;
                _inputAsset = Object.Instantiate(_inputAsset);

                PlayerObject.Init(_inputAsset.FindActionMap("Player"));
                _inputAsset.Enable();
            };
        }


        public class PlayerInput
        {
            public bool IsEnabled => _playerMono != null;
            private MonoBehaviour _playerMono = null;


            public event Action<CallbackContext> OnTouchEvt;
            public Vector2 PointerPosition { get; private set; }


            private InputActionMap _inputActionMap = null;
            private InputAction _touchAction;
            private InputAction _touchPositionAction;


            public void Init(InputActionMap inputActionMap)
            {
                _inputActionMap = inputActionMap;
                _touchAction = inputActionMap.FindAction("Touch");
                _touchPositionAction = inputActionMap.FindAction("TouchPosition");

                _inputActionMap.Enable();
                SetEnable(null);
            }

            private void HandleGetTouchPosition(CallbackContext context)
            {
                var screenPosition = context.ReadValue<Vector2>();
                PointerPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            }

            public void SetEnable(MonoBehaviour playerMono)
            {
                _playerMono = playerMono;
                if (_inputActionMap != null)
                {
                    _touchAction.performed += OnTouchEvt;
                    _touchPositionAction.performed += HandleGetTouchPosition;
                    _inputActionMap.Enable();
                }
            }

            public void SetDisable(MonoBehaviour playerMono)
            {
                if (_playerMono == playerMono)
                {
                    _playerMono = null;
                    _touchAction.performed -= OnTouchEvt;
                    _touchPositionAction.performed -= HandleGetTouchPosition;

                    _inputActionMap.Disable();
                }
            }
        }
    }
}