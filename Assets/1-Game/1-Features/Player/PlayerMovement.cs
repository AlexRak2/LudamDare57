using System;
using Game.Input;
using UnityEngine;
using LD57.UI;
using LD57;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;
        public LayerMask GroundLayer;

        public Transform PlayerCam;
        public Transform Orientation;
        public Camera Cam;

        [Header("Movement")] 
        public float WalkSpeed = 3;
        public float RunSpeed = 6;
        public float MaxSpeedAllowed = 40;
        public float LandSprintAmount = 0.5f;
        public float Gravity = 6;

        [SerializeField] private float _groundCheckDistance = 1;
        [Range(0, .5f)] [SerializeField] private float _frictionForceAmount = 0.175f;
        private readonly float _threshold = 0.01f;

        [SerializeField, Range(10, 80)] private float _maxSlopeAngle = 35f;

        [Header("Jump")] 
        public int MaxJumps;
        [Min(.25f), SerializeField] private float _jumpCooldown = .25f;
        [SerializeField] private float _jumpForce = 550f;
        [Range(0, 1), SerializeField] private float _controlAirborne = .5f;

        private bool _readyToJump = true;

        [Header("Camera")] 
        [Range(20, 89.7f)] public float MaxCameraAngle = 89.7f;
        public float SensitivityX = 4;
        public float SensitivityY = 4;
        public float AimingSensitivityMultiplier = .4f;

        [Space(10)] 
        public float FovSpeed = 2;
        public float FallVelocityThreshold = -8;

        [SerializeField, Range(70, 110)] private float _normalFov = 80;
        [SerializeField, Range(70, 110)] public float SprintFov = 90;
        [SerializeField, Range(0f, 110f)] public float FallFOV = 110;

        private float _currentFov;

        private Rigidbody _rb;
        private Collider _playerCollider;

        private float _xRotation;
        private float _desiredX;
        private bool _grounded;
        public bool IsGrounded => _grounded;

        private float _loseSpeedDeceleration;
        private float _acceleration = 4500;
        private float _currentSpeed = 20;
        public float GetCurrentSpeed => _currentSpeed;

        private Vector3 _normalVector = Vector3.up;

        private Vector2 _moveInput;
        private Vector2 _lookInput;
        public Vector3 MoveInput => _rb.linearVelocity;
        private bool _hasJumped = false;
        public int JumpCount;

        private bool _cancellingGrounded;

        private bool _isFrozen;
        public static bool IsFrozen => Instance._isFrozen;
        public static Rigidbody Rigidbody => Instance._rb;
        public static void Freeze(bool freeze)
        {
            Instance._isFrozen = freeze;
            Instance.enabled = !freeze;
            Instance._rb.isKinematic = freeze;
            Instance._moveInput = Vector3.zero;
            Instance._rb.linearVelocity = Vector3.zero;
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _rb = GetComponent<Rigidbody>();
            _playerCollider = GetComponent<Collider>();

            InputManager.ProcessOnJump += Jump;
        }

        private void OnDisable()
        {
            InputManager.ProcessOnJump -= Jump;
        }

        private void Start()
        {
            JumpCount = MaxJumps;
            _currentFov = _normalFov;
        }

        private void FixedUpdate()
        {
            if(LD57.GameManager.Instance.SettingsManager.IsSettingsCanvasActive) return;

            if (_rb.linearVelocity.magnitude > MaxSpeedAllowed)
                _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, MaxSpeedAllowed);
        }

        private void Update()
        {
            if(LD57.GameManager.Instance.SettingsManager.IsSettingsCanvasActive) return;

            _moveInput = InputManager.MoveDir;
            _lookInput = InputManager.LookDir;

            CheckGroundedWithRaycast();
            Movement(true);
            Look();
            HandleMovementSpeed();
            HandleCameraFov();
        }

        public void HandleMovementSpeed()
        {
            if (InputManager.IsSprinting)
            {
                _currentSpeed = RunSpeed;
            }
            else
            {
                _currentSpeed = WalkSpeed;
            }

            if (_rb.linearVelocity.sqrMagnitude < 0.0001f)
            {
                _currentSpeed = WalkSpeed;
            }
        }

        public void Movement(bool move)
        {
            Vector2 relativeVelocity = FindVelRelativeToLook();
            float xRel = relativeVelocity.x, yRel = relativeVelocity.y;

            FrictionForce(_moveInput.x, _moveInput.y, relativeVelocity);

            if (_moveInput.x > 0 && xRel > _currentSpeed || _moveInput.x < 0 && xRel < -_currentSpeed)
                _moveInput.x = 0;
            if (_moveInput.y > 0 && yRel > _currentSpeed || _moveInput.y < 0 && yRel < -_currentSpeed)
                _moveInput.y = 0;

            float multiplier = (!_grounded) ? _controlAirborne : 1;

            if (_rb.linearVelocity.sqrMagnitude < 0.02f)
                _rb.linearVelocity = Vector3.zero;

            if (!move)
            {
                if (_grounded)
                    _rb.linearVelocity = Vector3.zero;
                return;
            }

            _rb.AddForce(Orientation.forward * _moveInput.y * _acceleration * Time.deltaTime * multiplier);
            _rb.AddForce(Orientation.right * _moveInput.x * _acceleration * Time.deltaTime * multiplier);

            if (!_grounded)
            {
                _rb.AddForce(Physics.gravity * Gravity);
            }
        }

        public void Jump(bool value)
        {
            if(LD57.GameManager.Instance.SettingsManager.IsSettingsCanvasActive) return;
            
            if (JumpCount <= 0) return;

            JumpCount--;
            _readyToJump = false;
            _hasJumped = true;
            _cancellingGrounded = false;

            _rb.AddForce(Vector3.up * _jumpForce * 1.5f, ForceMode.Impulse);
            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        private void ResetJump() => _readyToJump = true;

        public void Look()
        {
            float sensM = 1;

            float mouseX = (_lookInput.x * Time.fixedDeltaTime) * SensitivityX * sensM;
            float mouseY = (_lookInput.y * Time.fixedDeltaTime) * SensitivityY * sensM;

            Vector3 rot = PlayerCam.localRotation.eulerAngles;
            _desiredX = rot.y + mouseX;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -MaxCameraAngle, MaxCameraAngle);

            PlayerCam.localRotation = Quaternion.Euler(_xRotation, _desiredX, PlayerCam.localRotation.z);
            Orientation.localRotation = Quaternion.Euler(0, _desiredX, 0);
        }

        private void FrictionForce(float x, float y, Vector2 mag)
        {
            if (!_grounded || _hasJumped) return;

            if (Math.Abs(mag.x) > _threshold && Math.Abs(x) < 0.05f || (mag.x < -_threshold && x > 0) ||
                (mag.x > _threshold && x < 0))
                _rb.AddForce(_acceleration * Orientation.right * Time.deltaTime * -mag.x * _frictionForceAmount);

            if (Math.Abs(mag.y) > _threshold && Math.Abs(y) < 0.05f || (mag.y < -_threshold && y > 0) ||
                (mag.y > _threshold && y < 0))
                _rb.AddForce(_acceleration * Orientation.forward * Time.deltaTime * -mag.y * _frictionForceAmount);

            Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
            if (flatVelocity.magnitude > _currentSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * _currentSpeed;
                _rb.linearVelocity = new Vector3(limitedVelocity.x, _rb.linearVelocity.y, limitedVelocity.z);
            }
        }

        public Vector2 FindVelRelativeToLook()
        {
            float lookAngle = Orientation.eulerAngles.y;
            Vector3 velocity = _rb.linearVelocity;
            Vector3 localVel = Quaternion.Euler(0, -lookAngle, 0) * velocity;

            return new Vector2(localVel.x, localVel.z);
        }

        public Vector3 GetVelocity => _rb.linearVelocity;
        public Vector3 GetRelativeVelocity => Orientation.InverseTransformDirection(_rb.linearVelocity);

        private bool IsFloor(Vector3 v)
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < _maxSlopeAngle;
        }

        private void CheckGroundedWithRaycast()
        {
            Vector3 origin = transform.position;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _groundCheckDistance, GroundLayer))
            {
                Vector3 normal = hit.normal;

                if (IsFloor(normal) && _readyToJump)
                {
                    JumpCount = MaxJumps;
                    _hasJumped = false;
                    _grounded = true;
                    _cancellingGrounded = false;
                    _normalVector = normal;
                    CancelInvoke(nameof(StopGrounded));
                }
            }
            else
            {
                if (_grounded || !_cancellingGrounded)
                {
                    _cancellingGrounded = true;
                    Invoke(nameof(StopGrounded), Time.deltaTime * 3f);
                }
            }
        }

        private void StopGrounded() => _grounded = false;

        private void HandleCameraFov()
        {
            _currentFov = Mathf.Lerp(Cam.fieldOfView, InputManager.IsSprinting ? SprintFov : _normalFov,
                    FovSpeed * Time.deltaTime);

            Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, _currentFov, FovSpeed * Time.deltaTime);
        }
    }
}
