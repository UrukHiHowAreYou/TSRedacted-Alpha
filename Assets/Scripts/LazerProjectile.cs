using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerProjectile : MonoBehaviour
{
    // [SerializeField] private Transform vfxHitGreen;
    // [SerializeField] private Transform vfxHitRed;
    
    [SerializeField] private float lazerTimer = 0.2f;

    private Rigidbody lazerRigidbody;

    // public AudioClip explosionAudioClip;
    // public AudioClip metalbouncAudioClip;

    public float speed = 30f;
    public bool destroyOnCollision = true;

    private void Awake()
    {
        lazerRigidbody = GetComponent<Rigidbody>();    
    }

    private void Start() {
        lazerRigidbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        lazerTimer -= Time.deltaTime;
        if (lazerTimer < 0)
        {
            Destroy(this.gameObject);
        }
    }

    // -- Hi Ryan, this checks for collision of the projectile. --
    // NOTE: The projectile (or the target) MUST have "is trigger" checked for this to work.
    private void OnTriggerEnter(Collider other) {
        // Debug.Log("YO FAM DIS COLLIDED YEH NOW DESTROY ALREADY");
        Destroy(gameObject);
        Destroy(lazerRigidbody);

        if (other.GetComponent<BulletTarget>() != null) {
            // Hit target
            Destroy(gameObject);
        } else {
            Destroy(gameObject);
        }
        if(destroyOnCollision) {
            Destroy(gameObject);
        }
    }
}
