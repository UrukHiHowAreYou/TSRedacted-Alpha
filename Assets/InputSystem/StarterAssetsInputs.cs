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
        private PlayerInputActions playerInputActions;
        private const string PLAYER_PREFS_BINDINGS = "InputBindings";

        public enum PlayerActions
		{
			Shoot,
			SecondaryFire,
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
            //if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            //{
            //    playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            //}
            playerInput = GetComponent<PlayerInput>();
            playerInputActions = new PlayerInputActions();
        }

		public string GetBindingText(PlayerActions playerAction)
        {
            //string actionToRebind;
            //int bindingIndex;
            switch (playerAction)
            {
                default:
                case PlayerActions.Shoot:
                    return playerInputActions.Player.Shoot.bindings[0].ToDisplayString();
                    //actionToRebind = "Shoot";
                    //bindingIndex = 0;
                    break;
                //case PlayerActions.SecondaryFire:
                //    actionToRebind = "SecondaryFire";
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Aim:
                //    actionToRebind = "Aim";
                //    bindingIndex = 3;
                //    break;
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
                //case PlayerActions.Jump:
                //    actionToRebind = "Jump";
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Start:
                //    actionToRebind = "SecondStartaryFire";
                //    bindingIndex = 0;
                //    break;
                //case PlayerActions.Sprint:
                //    actionToRebind = "Sprint";
                //    bindingIndex = 0;
                //    break;
            }
            //return playerInput.actions.FindAction(actionToRebind).GetBindingDisplayString(bindingIndex);

        }

        public void RebindBinding(PlayerActions playerAction, Action onActionRebound)
		{

            playerInputActions.Player.Disable();
            InputAction inputAction;
            int bindingIndex;
            switch (playerAction)
            {
                default:
                case PlayerActions.Shoot:
                    Debug.Log("The Shoot Case");
                    inputAction = playerInputActions.Player.Shoot;
                    bindingIndex = 0;
                    break;
                case PlayerActions.SecondaryFire:
                    inputAction = playerInputActions.Player.SecondaryFire;
                    bindingIndex = 0;
                    break;
                case PlayerActions.Aim:
                    inputAction = playerInputActions.Player.Aim;
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
                    inputAction = playerInputActions.Player.Jump;
                    bindingIndex = 0;
                    break;
                case PlayerActions.Start:
                    inputAction = playerInputActions.Player.Start;
                    bindingIndex = 0;
                    break;
                case PlayerActions.Sprint:
                    inputAction = playerInputActions.Player.Sprint;
                    bindingIndex = 0;
                    break;
            }
            //foreach (var item in
            Debug.Log("calling PerformInteractiveRebinding on inputAction "+ inputAction+" oon binding "+bindingIndex);
            inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => {
                Debug.Log("completed the rebinding");
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                Debug.Log(playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            }).Start();
            //{
			//	Debug.Log(item);   
            //}
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
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

		public void OnStart(InputValue value)
		{
			Debug.Log("OnStart triggered with value: " + value);
			StartInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
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