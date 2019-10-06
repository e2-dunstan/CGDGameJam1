using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : Projectile
{
    public bool isPlayerProjectile = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerProjectile == true)
        {
            if (other.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<Enemy>().InflictDamage(projectileDamage);
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.GetComponent<PlayerCombat>().TakeDamage(1);
                Destroy(this.gameObject);
            }
        }
    }
}
