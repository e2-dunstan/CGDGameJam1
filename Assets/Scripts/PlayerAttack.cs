using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCooldownDuration = 0.5f;
    [SerializeField] GameObject webProjectile;
    [SerializeField] Transform attackPosition;
    [SerializeField] float attackRange;
    [SerializeField] int damageAmount;

    float attackCooldownTimer = 0;

    void Update()
    {
        if(attackCooldownTimer <= 0)
        {
            if(Input.GetKey(KeyCode.Space))
            {
                if(Input.GetKey(KeyCode.S))
                {
                    WebProjectile projectile = Instantiate(webProjectile, attackPosition.position, Quaternion.identity).GetComponent<WebProjectile>();
                    projectile.SpawnProjectile(new Vector2(3, 0), 0.25f);

                    attackCooldownTimer = attackCooldownDuration;
                }
                else
                {
                    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange);
                    for(int i = 0; i < enemiesToDamage.Length; i++)
                    {
                        if(enemiesToDamage[i].CompareTag("Enemy"))
                        {
                            enemiesToDamage[i].GetComponent<Health>().TakeDamage(damageAmount);
                        }
                    }
                }

                attackCooldownTimer = attackCooldownDuration;
            }
        }
        else
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
