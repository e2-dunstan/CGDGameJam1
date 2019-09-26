using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE = 0,
        WALKING = 1,
        PERSUING = 2,
        ATTACKING = 3,
        STUNNED  = 4,
        DYING = 5
    }

    [SerializeField] public EnemyState enemyState = EnemyState.IDLE;
    [SerializeField] public int health {get; set;}

    [SerializeField] protected bool canSeePlayer = false;
    [SerializeField] protected float distanceFromPlayer;
    [SerializeField] protected float visionRange = 1.0f;
     
    [SerializeField] protected bool isOnIdleCooldown = false;
    [SerializeField] protected bool isOnAttackCooldown = false;
    [SerializeField] protected float attackRange = 4.0f;
    [SerializeField] protected float attackCooldown = 4.0f;

    GameObject player;

    protected void InflictDamage(int _damageAmount)
    {
        health = health - _damageAmount;
    }

    ///<summary> 
    ///By default this will raycast out from the enemy for the specified visionRange. 
    ///Override this if more complicated player detection is required.
    ///</summary>
    protected virtual bool CheckIfPlayerIsVisible(Vector2 _targetDestination)
    {
        canSeePlayer = false;

        RaycastHit hit;

        Physics.Raycast(transform.position, _targetDestination, out hit, visionRange);

        Debug.DrawRay(transform.position, _targetDestination, Color.red);
            

        if (hit.transform != null && hit.transform.tag == "Player")
        {
            canSeePlayer = true;
        }
        else
        {
            canSeePlayer = false;
        }

        return canSeePlayer;
    }

    /// <summary>
    /// Will calculateDistanceFromPlayer every update to determin the enemies current state
    /// </summary>
    protected virtual void CalculateDistanceFromPlayer()
    {
        //Will remove this gameObject find, using until this is merged with alex's work, then we're implementing something else
        player = GameObject.FindGameObjectWithTag("Player");

        distanceFromPlayer = Vector2.Distance(gameObject.transform.position, player.transform.position);
        
    }
}
