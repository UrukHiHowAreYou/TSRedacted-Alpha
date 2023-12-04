using Cinemachine;
using Unity.Netcode;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : NetworkBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float MoveSpeedBase = 4.0f;
        private float MoveSpeed = 4.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float SprintSpeedBase = 7f;
        private float SprintSpeed =7f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float SpeedChangeRate = 10.0f;

        [Tooltip("Aim sensitivity")]
        [SerializeField] private float Sensitivity = 1f;

        [Range(0, 1)] public float FootstepAudioVolume = 0.7f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float JumpTimeout = 0.3f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] private bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        [SerializeField] private float TopClamp = 90.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        [SerializeField] private float BottomClamp = -35.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        [SerializeField] private float CameraAngleOverride = 0.0f;

        //[Tooltip("For locking the camera position on all axis")]
        //[SerializeField] private bool LockCameraPosition = false;
        private static bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDDirection;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _followCamera;
        [SerializeField] private bool _rotateOnMove = false;

        private static float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            GetComponent<PlayerInput>().enabled = true;
            //GetComponent<CharacterController>().enabled = true;
            _controller = GetComponent<CharacterController>();

            _input = StarterAssetsInputs.Instance;
            _hasAnimator = TryGetComponent(out _animator);
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            // get a reference to our main camera
            //_mainCamera = this.gameObject.transform.Find("MainCamera").gameObject; //FindGameObjectWithTag("MainCamera");
            _mainCamera.GetComponent<Camera>().enabled = true;
            _mainCamera.GetComponent<AudioListener>().enabled = true;
            //_followCamera = this.gameObject.transform.Find("PlayerFollowCamera 1").gameObject;
            _followCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;

            //subscribe to the onGame(Un)Paused Event
            GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
            GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;
            GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;


            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            UnityEngine.Cursor.visible = false;
        }

        private void Start()
        {
            if (!IsOwner)
            {
                Debug.Log("TPS; I am NOT the owner: " + this.name);
                return;
            }

            Debug.Log("TPS; I am the owner: " + this.name);

        }

        public void GameManager_OnGamePaused(object sender, System.EventArgs e) { _input.cursorInputForLook = false; _input.SetCursorState(false); _input.MakeCursorVisible(true); }
        public void GameManager_OnGameUnPaused(object sender, System.EventArgs e) { _input.cursorInputForLook = true; _input.SetCursorState(true); _input.MakeCursorVisible(false); }
        public void GameManager_OnStateChanged(object sender, System.EventArgs e)
        {
            if (GameManager.Instance.IsGamePlaying())
            {
                SetMoveSpeed(1f);
                Debug.Log("GamePlaying MoveSpeed: " + MoveSpeed);
        
            }
            else
            {
                SetMoveSpeed(0f);
                Debug.Log("GameStopped MoveSpeed: " + MoveSpeed);
            }
        
        }

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }
            _hasAnimator = TryGetComponent(out _animator);

            GroundedCheck();
            JumpAndGravityServerRPC(Grounded, _input.jump);
            _input.jump = false;
            MoveServerAuth();
        }

        private void LateUpdate()
        {
            if (!IsOwner) return;
            CameraRotation();
        }



        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDDirection = Animator.StringToHash("Direction");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * Sensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * Sensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void MoveServerAuth()
        {
            Vector2 localPlayerMovement = _input.move;
            bool localPlayerSprint = _input.sprint;
            bool localPlayerAnalogMovement = _input.analogMovement;
            GameObject localPlayerMainCamera = _mainCamera;

            Debug.Log("movement: " + localPlayerMovement);

            MoveServerRpc(localPlayerMovement, localPlayerSprint, localPlayerAnalogMovement, _controller.velocity.x, _controller.velocity.z, _controller.velocity, _animationBlend, _mainCamera.transform.eulerAngles);
            
        }


 
        [ServerRpc(RequireOwnership = false)]
        private void MoveServerRpc(Vector2 move, bool sprint, bool analogMovement, float controllerX, float controllerZ, Vector3 controllerVelocity, float animationBlend, Vector3 mainCameraTransformAngles, ServerRpcParams serverRpcParams = default)
        {


            //Debug.Log(serverRpcParams.Receive.SenderClientId);  
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed = sprint ? SprintSpeed : MoveSpeed;
            float speed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(controllerX, 0.0f, controllerZ).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = analogMovement ? move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                speed = Mathf.Round(speed * 1000f) / 1000f;
            }
            else
            {
                speed = targetSpeed;
            }

            animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (animationBlend < 0.01f) animationBlend = 0f;


            Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving

            if (move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  mainCameraTransformAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            //transform.position += targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
            _controller.Move(targetDirection.normalized * (speed * Time.deltaTime) +
               new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            

            // update animator if using character
            if (_hasAnimator)
            {   
                // Hi Ryan, this gets the velocity of the player character as a Vector3
                Vector3 horizontalVelocity = controllerVelocity;
                // add it to a new vector3, minus the y velocity
                horizontalVelocity = new Vector3(controllerX, 0, controllerZ);
                // get the magnitude of the player character as a single float
                float horizontalSpeed = horizontalVelocity.magnitude;

                // Debug.Log("Horizontal Rotation (global?) = "+_controller.transform.eulerAngles.y);
                // Debug.Log("Horizontal Rotation (local) = "+_controller.transform.localRotation.eulerAngles.y);

                // This gets the direction the character is moving based on character controller velocity
                Vector3 movementDirection = horizontalVelocity.normalized;

                // This gets the local forward direction of the character
                Vector3 localForward = transform.forward;
                // Get the local right direction of the character
                Vector3 localRight = transform.right;

                // This calculates the angle between the movement direction and local forward
                float movementAngle = Vector3.Angle(movementDirection, localForward);

                // This determines if the character is moving left or right with a positive or negative value which it then multiples with the angle
                float angleSign = Mathf.Sign(Vector3.Dot(movementDirection, localRight));
                float adjustedAngle = movementAngle * angleSign;


                _animator.SetFloat(_animIDSpeed, horizontalSpeed, 0.1f, Time.deltaTime);
                _animator.SetFloat(_animIDDirection, (adjustedAngle/50), 0.25f, Time.deltaTime);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude, 0.1f, Time.deltaTime);

            }




        }
        [ServerRpc]
        private void JumpAndGravityServerRPC(bool grounded, bool inputJump)
        {

            Grounded = grounded;
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (inputJump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                inputJump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                SoundManager.Instance.PlayStepSound(transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                SoundManager.Instance.PlayLandingSound(transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void SetSensitivity(float newSensitivity)
        {
            Sensitivity = newSensitivity;
        }

        public void SetMoveSpeed(float multiplyer)
        {
            MoveSpeed = MoveSpeedBase * multiplyer;
            SprintSpeed = SprintSpeedBase * multiplyer;
        }
    }
}