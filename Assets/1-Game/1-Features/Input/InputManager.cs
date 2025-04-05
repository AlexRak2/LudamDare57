using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class InputManager : MonoBehaviour
    {
        private InputControl _inputControl;

        public static InputAction MoveAction;
        public static InputAction LookAction;
        public static InputAction JumpAction;
        public static InputAction SprintAction;
        public static InputAction RightAction;
        public static InputAction LeftAction;
        

        public static Vector2 MoveDir { get; private set; }
        public static Vector2 LookDir { get; private set; }

        public static bool Jumping;
        public static bool IsSprinting;
        public static bool IsAiming;

        public static event Action<bool> ProcessOnJump;


        private void Awake()
        {
            _inputControl = new InputControl();
        }

        private void OnEnable()
        {
            MoveAction = _inputControl.Player.Move;
            LookAction = _inputControl.Player.Look;
            JumpAction = _inputControl.Player.Jump;
            SprintAction = _inputControl.Player.Sprint;
            RightAction = _inputControl.Player.RightAction;
            LeftAction = _inputControl.Player.LeftAction;

            MoveAction.Enable();
            LookAction.Enable();
            JumpAction.Enable();
            SprintAction.Enable();
            RightAction.Enable();
            LeftAction.Enable();
        }

        private void OnDisable()
        {
            MoveAction.Disable();
            LookAction.Disable();
            JumpAction.Disable();
            SprintAction.Disable();
            RightAction.Disable();
            LeftAction.Disable();
        }

        private void Update()
        {
            MoveDir = MoveAction.ReadValue<Vector2>();
            LookDir = LookAction.ReadValue<Vector2>();
            IsSprinting = SprintAction.IsPressed();

            Jumping = JumpAction.WasPressedThisFrame();
            if (Jumping)
                OnJump();


        }

        private void OnJump() 
        {
            ProcessOnJump?.Invoke(true);
        }
    }
}