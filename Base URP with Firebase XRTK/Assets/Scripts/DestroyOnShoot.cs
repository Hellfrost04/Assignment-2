using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnShoot : MonoBehaviour
{
    public GameObject destructibleObject;
    public void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
        Destroy(destructibleObject);
    }
}
