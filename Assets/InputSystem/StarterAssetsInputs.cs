using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
    {

        public static StarterAssetsInputs Instance { get; private set; }
        private const string PLAYER_PREFS_BINDINGS = "InputBindings";

        public enum PlayerActions
		{
			Shoot,
			SecondaryFire,
            Flashlight,
			Aim,
			Jump,
			Forward,
			Left,
			Right,
			Backward,
			Start,
            Sprint
        }

        [Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool secondaryFire;
        public bool flashlight;
		public bool start;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		private PlayerInput playerInput;

        private void Awake()
        {
            Instance = this;
            playerInput = GetComponent<PlayerInput>();
            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            {
                playerInput.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            }
        }

		public string GetBindingText(PlayerActions playerAction)
        {
            string actionToRebind;
            int bindingIndex;
            switch (playerAction)
            {
                default:
                case PlayerActions.Shoot:
                    actionToRebind = "Shoot";
                    bindingIndex = 0;
                    break;
                case PlayerActions.SecondaryFire:
                    actionToRebind = "SecondaryFire";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Flashlight:
                    actionToRebind = "Flashlight";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Aim:
                    actionToRebind = "Aim";
                    bindingIndex = 0;
                    break;
                //case PlayerActions.Forward:
                //    actionToRebind = "Forward";
                //    bindingIndex = 4;
                //    break;
                //case PlayerActions.Left:
                //    actionToRebind = "Left";
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Right:
                //    actionToRebind = "Right";
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Backward:
                //    actionToRebind = "Backward";
                //    bindingIndex = 0;
                //    break;
                case PlayerActions.Jump:
                    actionToRebind = "Jump";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Start:
                    actionToRebind = "SecondStartaryFire";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Sprint:
                    actionToRebind = "Sprint";
                    bindingIndex = 0;
                    break;
            }
            return playerInput.actions.FindAction(actionToRebind).GetBindingDisplayString(bindingIndex);

        }

        public void RebindBinding(PlayerActions playerAction, Action onActionRebound)
		{

            InputAction inputAction;
            string actionToRebind;
            int bindingIndex;
            switch (playerAction)
            {
                default:
                case PlayerActions.Shoot:
                    actionToRebind = "Shoot";
                    bindingIndex = 0;
                    break;
                case PlayerActions.SecondaryFire:
                    actionToRebind = "SecondaryFire";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Flashlight:
                    actionToRebind = "Flashlight";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Aim:
                    actionToRebind = "Aim";
                    bindingIndex = 0;
                    break;
                //case PlayerActions.Forward:
                //    inputAction = playerInputActions.Player.Forward;
                //    bindingIndex = 4;
                //    break;
                //case PlayerActions.Left:
                //    inputAction = playerInputActions.Player.;
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Right:
                //    inputAction = playerInputActions.Player;
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Backward:
                //    inputAction = playerInputActions.Player;
                //    bindingIndex = 0;
                //    break;
                case PlayerActions.Jump:
                    actionToRebind = "Jump";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Start:
                    actionToRebind = "Start";
                    bindingIndex = 0;
                    break;
                case PlayerActions.Sprint:
                    actionToRebind = "Sprint";
                    bindingIndex = 0;
                    break;
            }

            inputAction = playerInput.actions.FindAction(actionToRebind);
            inputAction.Disable();
            inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
                Debug.Log("completed the rebinding");
                callback.Dispose();
                inputAction.Enable();
                onActionRebound();
                Debug.Log(inputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputAction.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
            Debug.Log("Moving") ;

        }
		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}
		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
            Debug.Log("Jump");
		}
        
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
        
		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}
        
		public void OnShoot(InputValue value)
		{
            ShootInput(value.isPressed);
		}
        
		public void OnSecondaryFire(InputValue value)
		{
			SecondaryFireInput(value.isPressed);
		}
        
        public void OnFlashlight(InputValue value)
		{
			FlashlightInput(value.isPressed);
		}

		public void OnStart(InputValue value)
		{
			Debug.Log("OnStart triggered with value: " + value);
			StartInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
            Debug.Log("moveDirection: " + newMoveDirection);
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
            Debug.Log("jumpState: " + newJumpState);
        }

        public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void SecondaryFireInput(bool newSecondaryFireState)
		{
			secondaryFire = newSecondaryFireState;
		}

        public void FlashlightInput(bool newFlashlightState)
		{
			flashlight = newFlashlightState;
		}

		public void StartInput(bool newStartState)
		{
			Debug.Log("StartInput triggered with value: " + newStartState);
			start = newStartState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        public void MakeCursorVisible(bool cursorVisibility)
        {
			Cursor.visible = cursorVisibility;
        }
    }
	
}