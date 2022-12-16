using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{   
    public static int shots;

    void Start()
    {   
        shots = 0;
        StartCoroutine(SelfDestruct());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
        {   
            shots +=1;
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

