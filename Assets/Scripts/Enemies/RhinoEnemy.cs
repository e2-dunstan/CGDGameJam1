using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoEnemy : Enemy
{
    public EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.WALKING;
        enemyMovement.spawnPosition = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerIsVisible(enemyMovement.targetDestination);
        CalculateDistanceFromPlayer();

        //if (isStunned == false)
        //{
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
        //}

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
                enemyMovement.StopMovingForTime(1.0f);
                break;
            default:
                break;

        }
    }

    //protected override bool CanSeePlayer()
    //{
    //    return false;
    //}

}
