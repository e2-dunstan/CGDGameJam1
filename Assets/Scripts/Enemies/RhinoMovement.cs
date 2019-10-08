using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;
public class RhinoMovement : EnemyMovement
{
    // Start is called before the first frame update
    void Start()
    {
        randomYLocation = gameObject.transform.position.y;
        spawnPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
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

            hasRandomTargetLocation = true;
        }

        //Check if unit is outside bounds
        if (IsWithinBounds(this.transform) == true)
        {
            hasRandomTargetLocation = false;
        }

        if(gameObject.transform.position != new Vector3(randomXLocation, randomYLocation, 0) && hasRandomTargetLocation == true)
        {
            targetDestination = new Vector2(randomXLocation, randomYLocation);
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(randomXLocation, randomYLocation), moveSpeed);
        }
        else
        {
            hasRandomTargetLocation = false;
        }

    }

    public override void MoveTowardsDestination(Vector2 _targetDestination)
    {
        targetDestination = _targetDestination;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _targetDestination, moveSpeed);
    }
}
