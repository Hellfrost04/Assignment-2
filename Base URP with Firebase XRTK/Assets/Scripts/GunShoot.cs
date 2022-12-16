using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public Transform bulletOrigin;
    public GameObject bulletPrefab;

    public void Fire()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.position, transform.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(bulletOrigin.forward * 150f);
    }
}
