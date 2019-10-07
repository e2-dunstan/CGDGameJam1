using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
	void OnTriggerEnter2D(Collider2D other)
	{

		if (other.CompareTag("Player"))
		{
            other.gameObject.GetComponent<PlayerCombat>().TakeDamage(1);
			Destroy(this.gameObject);
		}

	}
}
