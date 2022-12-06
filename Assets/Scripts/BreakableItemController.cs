using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItemController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Min(0.1f)] private float durability = 1f;
    [SerializeField, Min(0.0001f)] private float breakParticleSizeMultiplier = 0.0001f;

    [Header("Prefabs")]
    [SerializeField] private GameObject breakPrefab;

    public void Damage(float amount)
    {
        durability -= amount;
        if (durability <= 0f)
        {
            CreateBreakingEffect();
            Destroy(gameObject);
        }

    }

    private void CreateBreakingEffect()
    {
        var effect = Instantiate(breakPrefab, transform.position, Quaternion.identity);

        var renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
            if (renderer == null)
                return;
        }

        var main = effect.gameObject.GetComponent<ParticleSystem>().main;
        main.startColor = renderer.material.color;
        main.startSize = breakParticleSizeMultiplier * transform.localScale.magnitude;
    }



}
