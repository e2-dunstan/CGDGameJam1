using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private Player player;

    private void Awake()
    {
        player = Player.Instance();
    }

    void OnTriggerEnter2D(Collider2D other)
	{

		if (other.CompareTag("Player"))
		{
            other.gameObject.GetComponent<PlayerCombat>().InflictDamage(1);

            if (player.CurrentPlayerState == Player.PlayerState.WEBBING)
            {
                player.WebManager.ToggleSwinging();
            }
            
            Destroy(this.gameObject);
		}

	}
}
