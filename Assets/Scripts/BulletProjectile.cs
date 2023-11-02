using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform vfxHitGreen;
    [SerializeField] private Transform vfxHitRed;
    
    [SerializeField] private float grenadeTimer = 1.5f;

    private Rigidbody bulletRigidbody;

    public AudioClip explosionAudioClip;
    public AudioClip metalbouncAudioClip;


    public float speed = 30f;
    public bool destroyOnCollision = true;

    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody>();    
    }

    private void Start() {
        bulletRigidbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        grenadeTimer -= Time.deltaTime;
        if (grenadeTimer < 0)
        {
            Destroy(this.gameObject);
            SoundManager.Instance.PlayExplosionSound(transform.position);
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
        }
    }

    // -- Hi Ryan, this checks for collision of the projectile. --
    // NOTE: The projectile (or the target) MUST have "is trigger" checked for this to work.
    private void OnTriggerEnter(Collider other) {
        // Debug.Log("YO FAM DIS COLLIDED YEH NOW DESTROY ALREADY");
        if (other.GetComponent<BulletTarget>() != null) {
            // Hit target
            Instantiate(vfxHitGreen, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayExplosionSound(transform.position, 2);
            
            Destroy(gameObject);
        } else {
            // Hit something else
            Instantiate(vfxHitRed, transform.position, Quaternion.identity);
            // plays metal bounce sfx at player position with volume of 2
            AudioSource.PlayClipAtPoint(metalbouncAudioClip, transform.position, 3 * bulletRigidbody.velocity.magnitude);
        }
        if(destroyOnCollision) {
            Destroy(gameObject);
        }
    }
}
