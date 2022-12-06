using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class GunController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(1)] private int burstSize = 1;
    [SerializeField, Min(0f)] private float bulletSpread = 0f;
    [SerializeField] private float damage = 0f;
    [SerializeField, Min(0f)] private float initialBulletVelocity = 0f;
    [SerializeField, Min(0f)] private float cooldownTime = 1f;
    [SerializeField, Min(1)] private int magazineCapacity = 1;
    [SerializeField, Min(0)] private int magazineInitialFill = 1;

    [Header("Prefabs")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private List<Transform> explosionPrefabs;

    [Header("Prefab Parts")]
    [SerializeField] private List<Transform> barrelPoints;
    [SerializeField] public Transform sightsPoint;
    [SerializeField] public Transform holdPoint;

    [Header("Events")]
    [SerializeField] private UnityEvent onShoot;

    private Rigidbody rb;
    private Transform barrelPoint;

    private Timer cooldownTimer;
    private int currentBarrelIndex;
    private int magazineCurrentFill;

    public int MagazineCapacity => magazineCapacity;
    public int MagazineCurrentFill => magazineCurrentFill;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        barrelPoint = barrelPoints[0];

        cooldownTimer = new Timer();
        currentBarrelIndex = 0;
        magazineCurrentFill = magazineInitialFill;
    }

    public void Shoot()
    {
        if (cooldownTimer.Runtime() < cooldownTime)
            return;

        magazineCurrentFill--;

        cooldownTimer.Start();
        onShoot.Invoke();

        CreateExplosionEffect();
    }

    public void ShootSingle()
    {
        for (int i = 0; i < barrelPoints.Count; i++)
        {
            barrelPoint = barrelPoints[i];
            CreateBurst();
        }
    }

    public void ShootSequentelially()
    {
        currentBarrelIndex = (currentBarrelIndex + 1) % barrelPoints.Count;
        barrelPoint = barrelPoints[currentBarrelIndex];
        CreateBurst();
    }

    private void CreateBurst()
    {
        for (int i = 0; i < burstSize; i++)
        {
            CreateBullet();
        }
    }

    private void CreateBullet()
    {
        var rndOffset = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread));

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = barrelPoint.position + 0.1f * rndOffset.normalized;
        bullet.transform.rotation = barrelPoint.transform.rotation * Quaternion.Euler(rndOffset);

        var bulletController = bullet.GetComponent<ProjectileController>();
        bulletController.Velocity = initialBulletVelocity;
        bulletController.Damage = damage;
        bulletController.Shoot();
    }

    private void CreateExplosionEffect()
    {
        for (int i = 0; i < explosionPrefabs.Count; i++)
        {
            Instantiate(explosionPrefabs[i], barrelPoint.position, barrelPoint.rotation);
        }
    }
}
