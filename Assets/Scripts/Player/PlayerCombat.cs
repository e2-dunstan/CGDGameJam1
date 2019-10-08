using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Combat Settings")]
    [SerializeField] float attackCooldownDuration = 0.5f;
    [SerializeField] GameObject webProjectile;
    [SerializeField] Transform attackTransform;
    [SerializeField] float attackRange;
    [SerializeField] int punchDamage = 2;
    [SerializeField] float projectileSpeed;
    [SerializeField] int projectileDamage = 1;
    [SerializeField] float projectileLifetime = 0.25f;
    [SerializeField] int health = 10;

    private Player playerSingleton = null;
    private InputManager inputSingleton = null;
    private float attackCooldownTimer = 0;

    public GameObject bossEnemy;
    public bool isInBossScene = false;
    private void Start() 
    {
        playerSingleton = Player.Instance();
        inputSingleton = InputManager.Instance();
    }

    void Update()
    {
        UpdateAttackTransform();
        HandleInputs();
    }

    private void HandleInputs()
    {
        Debug.Log(inputSingleton.GetVerticalInput());
        //Check for inputs
        if (attackCooldownTimer <= 0)
        {
            if (inputSingleton.GetActionButton1Down())
            {
                if (inputSingleton.GetVerticalInput() != 0 || playerSingleton.CurrentPlayerState == Player.PlayerState.WEBBING)
                {
                    FireWebProjectile();
                }
                else
                {
                    PunchEnemy();
                }

                attackCooldownTimer = attackCooldownDuration;
            }
        }
        else
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void FireWebProjectile()
    {
        float directionMultiplier = 0.0f;

        //Right = 1 || Left = -1
        if (!isInBossScene)
        {
            directionMultiplier = playerSingleton.PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.RIGHT ? 1 : -1;

            WebProjectile projectile = Instantiate(webProjectile, attackTransform.position, Quaternion.identity).GetComponent<WebProjectile>();
            projectile.SpawnProjectile(new Vector2(projectileSpeed * 10 * directionMultiplier, 0), projectileLifetime, projectileDamage);

            attackCooldownTimer = attackCooldownDuration;
        }
        else
        {
            directionMultiplier = playerSingleton.PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.RIGHT ? 1 : -1;

            Vector2 relativePos = bossEnemy.transform.position - gameObject.transform.position;

            
                if (relativePos.x <= 0 && relativePos.y < 20 && directionMultiplier <= 0.5f)
                {
                    WebProjectile projectile = Instantiate(webProjectile, attackTransform.position, Quaternion.identity).GetComponent<WebProjectile>();
                    projectile.SpawnProjectile(new Vector2(relativePos.x, relativePos.y), 10, 1);
                }
                else if (relativePos.x >= 0 && relativePos.y < 20 && directionMultiplier >= 0.5f)
                {
                    WebProjectile projectile = Instantiate(webProjectile, attackTransform.position, Quaternion.identity).GetComponent<WebProjectile>();
                    projectile.SpawnProjectile(new Vector2(relativePos.x, relativePos.y), 10, 1);
                }
                else
                {
                    directionMultiplier = playerSingleton.PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.RIGHT ? 1 : -1;

                    WebProjectile projectile = Instantiate(webProjectile, attackTransform.position, Quaternion.identity).GetComponent<WebProjectile>();
                    projectile.SpawnProjectile(new Vector2(projectileSpeed * 10 * directionMultiplier, 0), projectileLifetime, projectileDamage);

                    attackCooldownTimer = attackCooldownDuration;
                }
    
        }
    }

    private void PunchEnemy()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackTransform.position, attackRange);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].CompareTag("Enemy"))
            {
                enemiesToDamage[i].GetComponent<Enemy>().InflictDamage(punchDamage);
            }
        }
    }

    void UpdateAttackTransform()
    {
        Vector2 attackPosition = attackTransform.localPosition;
        if (playerSingleton.PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.RIGHT)
        {
            attackPosition.x = Mathf.Abs(attackPosition.x);
        } 
        else if (playerSingleton.PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.LEFT) 
        {
            attackPosition.x = -Mathf.Abs(attackPosition.x);
        }
        attackTransform.localPosition = attackPosition;
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
    
    public void InflictDamage(int _damage)
    {
        Player.Instance().PlayerHealth.TakeDamage();
    }
}
