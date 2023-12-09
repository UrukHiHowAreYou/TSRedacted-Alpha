using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class FPShooterController : NetworkBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [Tooltip("Look sensitivity multipliers")]
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    // [SerializeField] private Transform hitpoint;

    [Tooltip("Prefab and spawn point for Grenade bullet")]
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [Tooltip("Prefab and spawn point for Lazer bullet")]
    [SerializeField] private Transform pfLazerProjectile;
    [SerializeField] private Transform muzzleFlashPosition;
    
    [Tooltip("VFX for gunshot and hitting objects")]
    [SerializeField] private Transform vfxMuzzleFlash;
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;

    [Tooltip("Bullet Hole setup")]
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] GameObject bulletHoleContainer;
    [SerializeField] float bulletHoleDestroyDelay = 5f;


    [SerializeField] private Light flashlight;
    
    // Add in SFX here
    public AudioClip hitmarkerAudioClip;
    public AudioClip grenadelaunchAudioClip;
    public AudioClip gunshot02AudioClip;
    public AudioClip hitwallAudioClip;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;


    private void Awake() {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update() {

        if (!IsOwner) { 
            // Debug.Log("FPShooterController_"+OwnerClientId+": Ve are not ze owner. Please stand down.");
            return; 
        };
        // Debug.Log("FPShooterController_"+OwnerClientId+": YAHA! Ve have taken control! It iz time for initiate attack!");

        // --- Hi Ryan, this casts a ray at the centre of the screen and adds a debug sphere to check ---
        // --- it also updates the mouse position with the target ---
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
        // ------- Add raycast hitscan code
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) {
            if (debugTransform != null) {
                debugTransform.position = raycastHit.point;
            }
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
            
        } else {
            // --- Hi Ryan, this sets the sensitivity to normal if not aiming ---
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
        }

        // Hi Ryan, this conditional detects whether shoot has been fired
        if (starterAssetsInputs.shoot) {
            
            SoundManager.Instance.PlayGunshotSound(transform.position);
            // Instantiate(vfxMuzzleFlash, muzzleFlashPosition.position, Quaternion.identity);
            Vector3 aimDir_shoot = (mouseWorldPosition - muzzleFlashPosition.position).normalized;
            Instantiate(pfLazerProjectile, muzzleFlashPosition.position, Quaternion.LookRotation(aimDir_shoot, Vector3.up));
            if(hitTransform != null) {
                // Hit something
                // Debug.Log(hitTransform.gameObject);
                // Debug.Log("EHEHEH = "+hitTransform.gameObject.layer);
                // Debug.Log("hohoho = "+hitTransform.transform.gameObject.layer);
                if (hitTransform.GetComponent<BulletTarget>() != null) {
                    // Hit target
                    // using debugTransform.position instead of transform.position
                    Instantiate(vfxHitGreen, debugTransform.position, Quaternion.identity);
                    // plays hit marker sfx at player position with volume of 1
                    SoundManager.Instance.PlayHitMarkerSound(transform.position);
                }
                else if (hitTransform.gameObject.name == "Structure_Prefab"){
                    // Debug.Log("OOH MAMA DAS A REAL ONE");
                    // Hit something else
                    // using debugTransform.position instead of transform.position
                    Instantiate(vfxHitRed, debugTransform.position, Quaternion.identity);
                    // plays wall-hit marker sfx at player position with volume of 1 at the wall position
                    SoundManager.Instance.PlayHitWallSound(hitTransform.position);
                } else {
                    // Debug.Log("OOh mate you missed ya muffin");
                }
                // Hi Ryan, this is the code for spawning a bullethole
                GameObject spawnedObject_bulletHole = Instantiate(bulletHolePrefab, raycastHit.point, Quaternion.identity);
                Quaternion targetRotation_bulletHole = Quaternion.LookRotation(ray.direction);

                // rotate the bullethole to match raycast hit point on wall
                spawnedObject_bulletHole.transform.rotation = targetRotation_bulletHole;
                spawnedObject_bulletHole.transform.SetParent(bulletHoleContainer.transform);
                // randomly rotate the bullethole around z axis (purely visual)
                spawnedObject_bulletHole.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
                // destroy bullethole after designated time (default 5 seconds)
                Destroy(spawnedObject_bulletHole, bulletHoleDestroyDelay);
            }
            
            
            // this stops firing after 1 shot
            starterAssetsInputs.shoot = false;
            Debug.Log("Stopped shooting, so shooting = "+starterAssetsInputs.shoot);
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
            
            // this plays the grenade launcher sfx from the Sound Manager
            SoundManager.Instance.PlayGrenadeLaunchSound(transform.position);

            // this creates a new bullet prefab
            Transform spawnedGrenadeTransform = Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            spawnedGrenadeTransform.GetComponent<NetworkObject>().Spawn(true);
            // this stops firing after 1 shot
            // ------ if this isn't set to false, the projectiles will constantly collide with themselves in front of the player
            // ------ a delay would need to be added for this to fire continuously
            starterAssetsInputs.secondaryFire = false;
        }

        if (starterAssetsInputs.flashlight) {

            Debug.Log("flashy flashy now now");
            // toggle flashlight 
            flashlight.enabled = !flashlight.enabled;
            // stop flashlight input
            starterAssetsInputs.flashlight = false;
        }

    }


}