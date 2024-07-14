using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Transform firePoint; // The point from which bullets are fired
    public GameObject bulletPrefab; // Your bullet prefab
    public float fireRate = 1f; // Bullets per second
    private float nextFireTime = 0.5f;

    private void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
/*        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            // Call the method to calculate the next fire time and fire the bullet
            FireRate();
        }*/
    }
    public void FireRate()
    {

        nextFireTime = Time.time + 1f / fireRate;
        Fire();
    }
    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // This line ensures the bullet is active. It's usually redundant as instantiated objects are active by default.
        bullet.SetActive(true);
    }


}
