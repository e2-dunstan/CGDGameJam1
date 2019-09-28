using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 1.0f;

    [SerializeField] public Vector2 spawnPosition;
     public Vector2 targetDestination;

    public virtual void MoveWithinDefinedWonderingBounds()
    {

    }

    public virtual void MoveTowardsDestination(Vector2 _targetDestination)
    {

        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _targetDestination, moveSpeed);
    }

    public virtual void StopMoving()
    {

    }

    public virtual void StopMovingForTime(float _timePeriod)
    {

    }

}
