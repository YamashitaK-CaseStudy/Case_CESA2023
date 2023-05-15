using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : MonoBehaviour
{
    private void Awake()
    {
        UiSeedBehavior.IncreaseTotal();
    }

    private void OnDestroy()
    {
        UiSeedBehavior.ObtaineSeed();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        // if (other.transform.root.CompareTag("RotateObject"))
        // {
        //     Destroy(gameObject);
        // }
    }
}
