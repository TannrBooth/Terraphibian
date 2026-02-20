using UnityEngine;

namespace Terraphibian
{
    [CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]
    public class PlayerController : InputController
    {
        private PlayerInputActions _inputActions;
        private bool _isJumping;
        private bool _isMelee;
        private bool _meleePressed; // pressed this frame

        private void OnEnable()
        {
            _inputActions = new PlayerInputActions();
            _inputActions.Gameplay.Enable();
            _inputActions.Gameplay.Jump.started += JumpStarted;
            _inputActions.Gameplay.Jump.canceled += JumpCanceled;
            _inputActions.Gameplay.Melee.started += MeleeStarted;
            _inputActions.Gameplay.Melee.canceled += MeleeCanceled;
        }

        private void OnDisable()
        {
            _inputActions.Gameplay.Disable();
            _inputActions.Gameplay.Jump.started -= JumpStarted;
            _inputActions.Gameplay.Jump.canceled -= JumpCanceled;
            _inputActions = null;
        }

        private void JumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _isJumping = true;
        }
        private void JumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _isJumping = false;
        }
        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return _isJumping;
        }

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            float x = _inputActions.Gameplay.Move.ReadValue<Vector2>().x;
            if (Mathf.Abs(x) < 0.1f)
                return 0f;
            
            return Mathf.Sign(x);
        }

        public override float RetrieveUpDownInput(GameObject gameObject)
        {
            return _inputActions.Gameplay.Move.ReadValue<Vector2>().y;
        }
        
        private void MeleeStarted(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _isMelee = true;
            _meleePressed = true;
        }
        private void MeleeCanceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            _isMelee = false;
        }
        public override bool RetrieveMeleeInput(GameObject gameObject)
        {
            return _isMelee;
        }

        public override bool RetrieveMeleeInputDown(GameObject gameObject)
        {
            if (_meleePressed)
            {
                _meleePressed = false;
                return true;
            }
            return false;
        }

    }
}
