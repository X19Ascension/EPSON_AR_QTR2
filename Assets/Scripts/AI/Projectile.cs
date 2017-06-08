using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public float ProjectileSpeed;
    public double ProjectileLifetime;

    public GameObject Sender;

    // Update is called once per frame
    void Update()
    {
        if (ProjectileLifetime < 0)
            Destroy(this.gameObject);
        else
            ProjectileLifetime -= Time.deltaTime;
    }
}