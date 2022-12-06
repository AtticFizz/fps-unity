using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class CollectibleItemController : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Entered");
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        Debug.Log("Exited");
        outline.enabled = false;
    }


}
