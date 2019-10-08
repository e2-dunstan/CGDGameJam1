using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditMovement : EnemyMovement
{
    public enum MovementDirection
    {
        LEFT,
        RIGHT
    }

    private MovementDirection movementDirection;
    public MovementDirection BanditMovementDirection { get => movementDirection; set => movementDirection = value; }

    private Vector2 previousPosition;

    void Start()
    {
        randomYLocation = gameObject.transform.position.y;
        spawnPosition = gameObject.transform.position;

        previousPosition = this.transform.position;
    }

    private void FixedUpdate()
    {
        UpdateMovementDirection();
    }

    private void UpdateMovementDirection()
    {
        Vector2 enemyVelocity = ((Vector2)this.transform.position - previousPosition);

        if (enemyVelocity.x == 0)
        {
            //Leave movementDirection to its previous setting
        }
        else if (enemyVelocity.x > 0)
        {
            movementDirection = MovementDirection.RIGHT;
        }
        else if (enemyVelocity.x < 0)
        {
            movementDirection = MovementDirection.LEFT;
        }

        previousPosition = this.transform.position;
    }

    public override void MoveWithinDefinedWonderingBounds()
    {
        if (hasRandomTargetLocation == false)
        { 
            randomXLocation = Random.Range(spawnPosition.x - wonderingBoundaryXLeft, spawnPosition.x + wonderingBoundaryXRight);
            
            if (canFly == true)
            {
                randomYLocation = Random.Range(spawnPosition.y - wonderingBoundaryYDown, spawnPosition.y + wonderingBoundaryYUp);
            }

            hasReachedDestination = false;
            hasRandomTargetLocation = true;
        }

        //Check if unit is outside bounds
        if (!IsWithinBounds(this.transform))
        {
            hasRandomTargetLocation = false;
        }

        //Has reached destination
        if(gameObject.transform.position != new Vector3(randomXLocation, randomYLocation, 0) && hasRandomTargetLocation == true)
        {
            targetDestination = new Vector2(randomXLocation, randomYLocation);
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(randomXLocation, randomYLocation), moveSpeed);
        }
        else
        {
            hasReachedDestination = true;
        }
    }

    public override void MoveTowardsDestination(Vector2 _targetDestination)
    {
        targetDestination = _targetDestination;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _targetDestination, moveSpeed);
    }
}
