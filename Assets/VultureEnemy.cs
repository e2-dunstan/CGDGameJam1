using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureEnemy : Enemy
{
    public EnemyMovement enemyMovement;
    public GameObject webProjectile;
    private GameObject player;
    [SerializeField] private GameObject Floor;

    public int hardBulletSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance().gameObject;
        webProjectile.GetComponent<WebProjectile>().isPlayerProjectile = false;
        enemyState = EnemyState.WALKING;
        enemyMovement.spawnPosition = gameObject.transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerIsVisible(enemyMovement.targetDestination);
        CalculateDistanceFromPlayer();

        if (enemyState != EnemyState.STUNNED)
        {
            //If Player cannot be seen
            if (distanceFromPlayer > visionRange && isOnIdleCooldown != true /*&& canSeePlayer == false*/)
            {
                enemyState = EnemyState.WALKING;
            }
            //If Player can be seen
            if (distanceFromPlayer <= visionRange && distanceFromPlayer > attackRange /*&& canSeePlayer*/)
            {
                enemyState = EnemyState.PERSUING;
            }
            //If Player can be seen and enemy can attack
            else if (distanceFromPlayer <= visionRange && distanceFromPlayer <= attackRange
                    && isOnAttackCooldown == false && canSeePlayer)
            {
                enemyState = EnemyState.ATTACKING;
            }
            else if (distanceFromPlayer <= visionRange && distanceFromPlayer <= attackRange
                    && isOnAttackCooldown == true /*&& canSeePlayer*/)
            {
                enemyState = EnemyState.PERSUING;
            }
        }

        switch (enemyState)
        {
            case EnemyState.IDLE:
                enemyMovement.StopMoving();
                break;
            case EnemyState.WALKING:
                enemyMovement.MoveWithinDefinedWonderingBounds();
                break;
            case EnemyState.PERSUING:
                enemyMovement.MoveTowardsDestination(gameObject.transform.position + new Vector3(-1, 0));
                break;
            case EnemyState.ATTACKING:
                enemyMovement.StopMoving();
                break;
            case EnemyState.STUNNED:
                enemyMovement.ForceMoveToSpecificPosition(new Vector2(gameObject.transform.position.x, Floor.transform.position.y));
                enemyMovement.MoveWithinDefinedWonderingBounds();

                if(enemyMovement.hasReachedDestination == true)
                {
                    enemyState = EnemyState.WALKING;
                }
                break;
            default:
                break;

        }
    }

    public void EasyShootAttack()
    {
        Vector2 relativePos = player.transform.position - gameObject.transform.position;

        WebProjectile projectile = Instantiate(webProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<WebProjectile>();
        projectile.SpawnProjectile(new Vector2(relativePos.x, relativePos.y), 10, 1);

    }

    public void HardShootAttack()
    {
        Vector2 relativePos = player.transform.position - gameObject.transform.position;

        WebProjectile projectile = Instantiate(webProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<WebProjectile>();
        projectile.SpawnProjectile(new Vector2(-5 * hardBulletSpeed, 5 * hardBulletSpeed), 10, 1);

        WebProjectile projectile2 = Instantiate(webProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<WebProjectile>();
        projectile2.SpawnProjectile(new Vector2(-5 * hardBulletSpeed, -5 * hardBulletSpeed), 10, 1);

        WebProjectile projectile3 = Instantiate(webProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<WebProjectile>();
        projectile3.SpawnProjectile(new Vector2(5 * hardBulletSpeed, -5 * hardBulletSpeed), 10, 1);

        WebProjectile projectile4 = Instantiate(webProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<WebProjectile>();
        projectile4.SpawnProjectile(new Vector2(5 * hardBulletSpeed, 5 * hardBulletSpeed), 10, 1);
    }

    public override void InflictDamage(int _damageAmount)
    {
        health = health - _damageAmount;
        //Change enemy colour

        if (health <= 0)
        {
            enemyState = EnemyState.DYING;
            //Play death animation
        }
        else
        {
            enemyState = EnemyState.STUNNED;
        }
    }
}
