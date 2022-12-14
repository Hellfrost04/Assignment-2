using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destructible"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}

