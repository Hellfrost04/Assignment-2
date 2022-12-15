using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

