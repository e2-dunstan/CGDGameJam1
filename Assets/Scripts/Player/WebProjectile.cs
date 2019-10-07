using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : Projectile
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);

        

            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().InflictDamage(projectileDamage);
                Destroy(this.gameObject);
            }
        
    }
}
