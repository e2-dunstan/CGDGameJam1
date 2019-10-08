using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureEnemy : Enemy
{
    public EnemyMovement enemyMovement;
    public GameObject enemyProjectile;
    private Player player;

    //Prevents vulture being hit twice
    [SerializeField] private bool canBeHit = true;

    [SerializeField] private GameObject Floor;

    public int hardBulletSpeed = 5;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] vultureAnimations;
    private float animTimeElapsed = 0;
    private float timeBetweenFrames = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance();
        enemyState = EnemyState.WALKING;
        enemyMovement.spawnPosition = gameObject.transform.position;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    { 
        switch (enemyState)
        {
            case EnemyState.IDLE:
                enemyMovement.StopMoving();
                break;
            case EnemyState.WALKING:
                enemyMovement.MoveWithinDefinedWonderingBounds();
                break;
            case EnemyState.PERSUING:
                break;
            case EnemyState.ATTACKING:
                //Handled by bossmanager for vulture
                break;
            case EnemyState.STUNNED:
                enemyMovement.ForceMoveToSpecificPosition(new Vector2(gameObject.transform.position.x, Floor.transform.position.y + 10));
                enemyMovement.MoveWithinDefinedWonderingBounds();
                break;
            case EnemyState.DYING:
                enemyMovement.ForceMoveToSpecificPosition(new Vector2(gameObject.transform.position.x, Floor.transform.position.y + 10));
                enemyMovement.MoveWithinDefinedWonderingBounds();
                break;
            default:
                break;
        }

        if (enemyState != EnemyState.DYING && enemyState != EnemyState.STUNNED)
        {
            animTimeElapsed += Time.deltaTime;
            if (animTimeElapsed > timeBetweenFrames / 2.0f)
            {
                spriteRenderer.sprite = vultureAnimations[0];
                if (animTimeElapsed > timeBetweenFrames)
                {
                    animTimeElapsed = 0;
                }
            }
            else
            {
                spriteRenderer.sprite = vultureAnimations[1];
            }
        }

    }

    public void EasyShootAttack()
    {
        Vector2 relativePos = player.transform.position - gameObject.transform.position;

        EnemyProjectile projectile = Instantiate(enemyProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        projectile.SpawnProjectile(new Vector2(relativePos.x, relativePos.y), 10, 1);

    }

    public void HardShootAttack()
    {
        EnemyProjectile projectile = Instantiate(enemyProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        projectile.SpawnProjectile(new Vector2(-5 * hardBulletSpeed, 5 * hardBulletSpeed), 10, 1);

        EnemyProjectile projectile2 = Instantiate(enemyProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        projectile2.SpawnProjectile(new Vector2(-5 * hardBulletSpeed, -5 * hardBulletSpeed), 10, 1);

        EnemyProjectile projectile3 = Instantiate(enemyProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        projectile3.SpawnProjectile(new Vector2(5 * hardBulletSpeed, -5 * hardBulletSpeed), 10, 1);

        EnemyProjectile projectile4 = Instantiate(enemyProjectile, gameObject.transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        projectile4.SpawnProjectile(new Vector2(5 * hardBulletSpeed, 5 * hardBulletSpeed), 10, 1);
    }

    public override void InflictDamage(int _damageAmount)
    {
        //Is grounded and can be damaged
        if (enemyMovement.hasReachedDestination == true && enemyState == EnemyState.STUNNED)
        {
            if (canBeHit == true)
            {
                //Change enemy colour
                health = health - _damageAmount;


                if (health <= 0)
                {
                    enemyState = EnemyState.DYING;
                    //Play death animation
                    spriteRenderer.sprite = vultureAnimations[2];
                }
                else
                {
                    //CHANGE BACK TO WALKING SPRITE
                    enemyState = EnemyState.WALKING;
                    InvincibleForTime(2.0f);
                }
            }
        }else if (enemyState != EnemyState.STUNNED)
        {
            //Is hit and begins falling
            //CHANGE TO FALLING SPRITE
            enemyState = EnemyState.STUNNED;
            spriteRenderer.sprite = vultureAnimations[2];
            InvincibleForTime(2);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (player.CurrentPlayerState == Player.PlayerState.WEBBING)
            {
                player.WebManager.ToggleSwinging();
                Vector2 knockBackVelocity = (enemyMovement.transform.position - player.transform.position) * 15;
                player.PlayerMovement.Rigidbody.velocity = knockBackVelocity;
                player.PlayerCombat.InflictDamage(1);
            }
        }
    }

    private void InvincibleForTime(float _time)
    {
        canBeHit = false;
        StartCoroutine("InvincibilityCoroutine", _time);
    }

    IEnumerator InvincibilityCoroutine(float _time)
    {
        yield return new WaitForSeconds(_time);
        canBeHit = true;
    }

}
