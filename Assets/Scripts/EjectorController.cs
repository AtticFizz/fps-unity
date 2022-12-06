using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EjectorController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0.01f)] private float ejectionForce = 0.2f;
    [SerializeField, Min(0)] private int magSize = 1;
    [SerializeField, Min(0f)] private float ejectionSpread = 0.1f;

    [Header("Prefab Parts")]
    [SerializeField] private Transform ejectionPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject bulletCasePrefab;

    private int calls;

    private void Awake()
    {
        calls = 0;
    }

    public void CallEject() // NO LONGER WORKS AS INTENDED WITH MAG SIZE
    {
        calls++;

        if (calls < magSize)
            return;

        for (int i = 0; i < magSize; i++)
        {
            EjectCase();
        }

        calls = 0;
    }

    private void EjectCase()
    {
        var rndOffset = new Vector3(Random.Range(0, ejectionSpread), Random.Range(0, ejectionSpread), Random.Range(0, ejectionSpread));
        var bulletCase = Instantiate(bulletCasePrefab);
        var rb = bulletCase.GetComponent<Rigidbody>();

        bulletCase.transform.position = ejectionPoint.position + rndOffset;
        bulletCase.transform.rotation = ejectionPoint.rotation * Quaternion.Euler(rndOffset);
        rb.AddForce(ejectionForce * ejectionPoint.transform.forward, ForceMode.Impulse);
    }
}
