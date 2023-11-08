using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class FPShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    // [SerializeField] private Transform hitpoint;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;

    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    
    // Add in SFX here
    public AudioClip hitmarkerAudioClip;
    public AudioClip gunshot02AudioClip;
    public AudioClip hitwallAudioClip;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;


    private void Awake() {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update() {

        
        // --- Hi Ryan, this casts a ray at the centre of the screen and adds a debug sphere to check ---
        // --- it also updates the mouse position with the target ---
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
        // ------- Add raycast hitscan code
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            // ------- Add raycast hitscan code -- this bugs for some reason and raycastHit.point works instead.
            // NB can just use debugTransform.position
            hitTransform = raycastHit.transform;
            // hitpoint.position = raycastHit.point
        }

        // --- Hi Ryan, this updates the character rotation to match the aim direction --- 
        Vector3 worldAimTarget = mouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;
        // --- this ensures a smooth transition of the rotation --- 
        transform.forward =  Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        // --- Hi Ryan, this tests if Aim is pressed ---
        if (starterAssetsInputs.aim) {
            // --- this lowers the sensitivity when aiming ---
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);

            // Hi Ryan, this would activate aiming animation on aim, if it wasn't set to on by default
            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));
            
        } else {
            // --- Hi Ryan, this sets the sensitivity to normal if not aiming ---
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);

            // Hi Ryan, this would deactivate aiming animation, if it wasn't set to on by default
            // animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        // Hi Ryan, this conditional detects whether shoot has been fired
        if (starterAssetsInputs.shoot) {
            
            SoundManager.Instance.PlayGunshotSound(transform.position);
            if(hitTransform != null) {
                // Hit something
                Debug.Log(hitTransform.gameObject);
                Debug.Log("EHEHEH = "+hitTransform.gameObject.layer);
                Debug.Log("hohoho = "+hitTransform.transform.gameObject.layer);
                if (hitTransform.GetComponent<BulletTarget>() != null) {
                    // Hit target
                    // using debugTransform.position instead of transform.position
                    Instantiate(vfxHitGreen, debugTransform.position, Quaternion.identity);
                    // plays hit marker sfx at player position with volume of 1
                    SoundManager.Instance.PlayHitMarkerSound(transform.position);
                }
                else if (hitTransform.gameObject.name == "Structure_Prefab"){
                    Debug.Log("OOH MAMA DAS A REAL ONE");
                    // Hit something else
                    // using debugTransform.position instead of transform.position
                    Instantiate(vfxHitRed, debugTransform.position, Quaternion.identity);
                    // plays wall-hit marker sfx at player position with volume of 1 at the wall position
                    SoundManager.Instance.PlayHitWallSound(hitTransform.position);
                } else {
                    //
                    Debug.Log("OOh mate you missed ya muffin");
                }
            }
            
            
            // this stops firing after 1 shot
            starterAssetsInputs.shoot = false;
        }

        // Hi ADAM, this conditional detects whether pause has been pressed
        if (starterAssetsInputs.start) {
            GameManager.Instance.TogglePauseGame();
            starterAssetsInputs.start = false;
        }

        // Hi Ryan, this conditional detects whether secondary fire has been fired
        if (starterAssetsInputs.secondaryFire) {
            
            // Hi Ryan, this triggers shooting with projectile
            // this gets the aim direction from the aiming position and spawn point of the bullet 
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            
            // this creates a new bullet prefab
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            
            // this stops firing after 1 shot
            // ------ if this isn't set to false, the projectiles will constantly collide with themselves in front of the player
            // ------ a delay would need to be added for this to fire continuously
            starterAssetsInputs.secondaryFire = false;
        }

    }


}