using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
public class ProjectileController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0f)] private float minEffectiveVelocity = 10f;

    [Header("Prefabs")]
    [SerializeField] private GameObject explosionPrefab;

    private Rigidbody rb;
    private TrailRenderer trail;

    public float Velocity { get; set; }
    public float Damage { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        
        Velocity = 0f;
        Damage = 0f;
    }

    private void Update()
    {
        UpdateTrail();
    }

    public void Shoot()
    {
        rb.AddForce(Velocity * transform.forward, ForceMode.Impulse);
    }

    private void UpdateTrail()
    {
        float linearVelocity = rb.velocity.magnitude;
        if (linearVelocity <= minEffectiveVelocity)
        {
            if (trail.enabled)
                trail.enabled = false;
        }
        else
        {
            if (!trail.enabled)
                trail.enabled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float linearVelocity = rb.velocity.magnitude;
        if (linearVelocity <= minEffectiveVelocity)
            return;

        CreateExplosionEffect();

        BreakableItemController breakableItemController = collision.gameObject.GetComponent<BreakableItemController>();
        if (breakableItemController != null)
            breakableItemController.Damage(Damage);
    }

    private void CreateExplosionEffect()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
