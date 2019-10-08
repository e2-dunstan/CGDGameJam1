using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : Projectile
{
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.InflictDamage(projectileDamage);
            enemy.StunEnemy();

            Destroy(this.gameObject);
        }
        else if(!other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
        
    }
}
