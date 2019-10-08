using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEnemy : Enemy
{
    public BanditMovement banditMovement;
    public float knockbackMagnitude = 500.0f;

    [SerializeField] private AudioClip punchAudioClip;
    [SerializeField] private float stunTime = 3f;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites;
    /* 0 = idle
     * 1 = stunned
     * 2 = punching
     * 3 = run frame 1
     * 4 = run frame 2
     */

    private float runTimeElapsed = 0;

    void Start()
    {
        attackTimer = 0f;
        idleTimer = 0f;
        stunTimer = 0f;

        health = health == 0 ? 4 : health;
        visionRange = visionRange == 0 ? 40 : visionRange;
        attackRange = attackRange == 0 ? 10 : attackRange;
        attackCooldown = attackCooldown == 0 ? 2 : attackCooldown;
        idleCooldown = idleCooldown == 0 ? 2 : idleCooldown;

        enemyState = EnemyState.WALKING;

        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        player = Player.Instance();
    }

    
    private void Update()
    {
        if(enemyState == EnemyState.DYING)
        {
            ScoreManager.Instance.AddScore(scoreForKilling);
            Destroy(this.gameObject);
            return;
        }

        UpdateTimers();
        CalculateDistanceFromPlayer();

        if(enemyState != EnemyState.STUNNED)
        { 
            ProcessMovementStates();

            //If Player can be seen and enemy can attack
            if (distanceFromPlayer <= attackRange && enemyState == EnemyState.PERSUING)
            {
                enemyState = EnemyState.ATTACKING;
            }
        }

        switch (enemyState)
        {
            case EnemyState.IDLE:
                banditMovement.StopMoving();
                spriteRenderer.sprite = GetGroundedSprite();
                break;
            case EnemyState.WALKING:
                banditMovement.MoveWithinDefinedWonderingBounds();
                spriteRenderer.sprite = GetGroundedSprite();
                break;
            case EnemyState.PERSUING:
                banditMovement.MoveTowardsDestination(new Vector2(player.transform.position.x, this.transform.position.y));
                spriteRenderer.sprite = GetGroundedSprite();
                break;
            case EnemyState.ATTACKING:
                banditMovement.StopMoving();
                if(!isOnAttackCooldown)
                {
                    AttackPlayer();
                }
                else if(attackTimer < 0.25f)
                {
                    spriteRenderer.sprite = sprites[2];
                }
                else
                {
                    spriteRenderer.sprite = GetGroundedSprite();
                }
                break;
            case EnemyState.STUNNED:
                spriteRenderer.sprite = sprites[1];
                break;
            default:
                break;

        }

        //Flips sprite based on player direction
        spriteRenderer.flipX = banditMovement.BanditMovementDirection == BanditMovement.MovementDirection.LEFT;
    }

    private void UpdateTimers()
    {
        if (isOnIdleCooldown)
        {
            if (idleTimer < idleCooldown)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleCooldown)
                {
                    idleTimer = 0f;
                    isOnIdleCooldown = false;
                    banditMovement.hasRandomTargetLocation = false;
                }
            }
        }

        if (isOnAttackCooldown)
        {
            if (attackTimer < attackCooldown)
            {
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    attackTimer = 0f;
                    isOnAttackCooldown = false;
                }
            }
        }

        if(enemyState == EnemyState.STUNNED)
        {
            if (stunTimer < stunTime)
            {
                stunTimer += Time.deltaTime;
                if (stunTimer >= stunTime)
                {
                    stunTime = 0f;
                    enemyState = EnemyState.IDLE;
                }
            }
        }
    }

    private void CheckReachedDestination()
    {
        if(banditMovement.hasReachedDestination == true)
        {
            isOnIdleCooldown = true;
            idleTimer = 0f;
        }
    }

    public override void StunEnemy()
    {
        stunTimer = 0;
        enemyState = EnemyState.STUNNED;
    }

    private void AttackPlayer()
    {
        attackTimer = 0f;
        isOnAttackCooldown = true;

        AudioManager.Instance.PlaySpecificClip(punchAudioClip, transform);

        player.PlayerCombat.InflictDamage(1);
    }

    private void ProcessMovementStates()
    {
        //If Player is can be seen
        if (IsPlayerWithinBounds() && IsFacingPlayer())
        { 
            enemyState = EnemyState.PERSUING;
            //isOnIdleCooldown = false;
        }
        //If Player cannot be scene
        else if(!isOnIdleCooldown)
        {
            enemyState = EnemyState.WALKING;
            CheckReachedDestination();
        }
        //If Player needs to idle
        else
        {
            enemyState = EnemyState.IDLE;
        }
    }

    private bool IsPlayerWithinBounds()
    {
        return banditMovement.IsWithinBounds(player.transform);
    }

    private bool IsFacingPlayer()
    {
        float playerX = player.transform.position.x;

        if(banditMovement.BanditMovementDirection == BanditMovement.MovementDirection.RIGHT)
        {
            if(this.transform.position.x < playerX)
            {
                return true;
            }
        }
        else if(banditMovement.BanditMovementDirection == BanditMovement.MovementDirection.LEFT)
        {
            if(this.transform.position.x > playerX)
            {
                return true;
            }
        }

        return false;
    }

    private Sprite GetGroundedSprite()
    {
        switch (enemyState)
        {
            case EnemyState.WALKING:
            case EnemyState.PERSUING:
                runTimeElapsed += Time.deltaTime;
                if (runTimeElapsed > 0.2f)
                {
                    if (runTimeElapsed > 0.4f) runTimeElapsed = 0;
                    return sprites[4];
                }
                else
                {
                    return sprites[3];
                }

            default:
                return sprites[0];
        }
    }

    private void OnCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (player.CurrentPlayerState == Player.PlayerState.WEBBING || player.CurrentPlayerState == Player.PlayerState.AIRBORNE)
            {
                player.WebManager.ToggleSwinging();
                Vector2 knockBackVelocity = player.transform.position - gameObject.transform.position;
                knockBackVelocity = knockBackVelocity.normalized;
                Debug.Log(knockBackVelocity.x);
                knockBackVelocity = knockBackVelocity * knockbackMagnitude;

                player.PlayerMovement.Rigidbody.velocity = knockBackVelocity;
                player.PlayerMovement.CarryOverVelocityFromSwinging(true);
            }
        }
    }
}
