using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : MonoBehaviour
{
    private void Awake()
    {
        //‘”‚ğ‰Â•Ï’·‚É‚·‚é‚È‚ç
        //UiSeedBehavior.IncreaseTotal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            UiSeedBehavior.ObtaineSeed();
            Destroy(gameObject);
        }
        // if (other.transform.root.CompareTag("RotateObject"))
        // {
        //     Destroy(gameObject);
        // }
    }
}
