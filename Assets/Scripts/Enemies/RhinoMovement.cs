using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime;
public class RhinoMovement : EnemyMovement
{
    [SerializeField] private float wonderingBoundaryXLeft = 1;
    [SerializeField] private float wonderingBoundaryXRight= 1;

    [SerializeField] private float wonderingBoundaryYUp= 1;
    [SerializeField] private float wonderingBoundaryYDown = 1;
    [SerializeField] private bool canFly = false;

    [SerializeField] private bool hasRandomTargetLocation = false;

    [SerializeField] private Vector2 randomLocation;

    [SerializeField] private float randomXLocation;
    [SerializeField] private float randomYLocation;


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
        if (IsWithinBounds() == true)
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

    private bool IsWithinBounds()
    {
        if (gameObject.transform.position.x < (spawnPosition.x - wonderingBoundaryXLeft) || gameObject.transform.position.x > (spawnPosition.x + wonderingBoundaryXRight) ||
            gameObject.transform.position.y < (spawnPosition.y - wonderingBoundaryYDown) || gameObject.transform.position.y > (spawnPosition.y + wonderingBoundaryYUp))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x + (wonderingBoundaryXRight/2), spawnPosition.y, 0), new Vector3(wonderingBoundaryXRight, (wonderingBoundaryYUp + wonderingBoundaryYDown), 0));


        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x - (wonderingBoundaryXLeft / 2), spawnPosition.y, 0), new Vector3(wonderingBoundaryXLeft * -1, (wonderingBoundaryYUp + wonderingBoundaryYDown), 0));


        Gizmos.color = new Color(0, 1, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x, spawnPosition.y + (wonderingBoundaryYUp / 2), 0), new Vector3((wonderingBoundaryXRight + wonderingBoundaryXLeft), wonderingBoundaryYUp, 0));


        Gizmos.color = new Color(1, 0, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x, spawnPosition.y - (wonderingBoundaryYDown / 2), 0), new Vector3((wonderingBoundaryXRight + wonderingBoundaryXLeft), wonderingBoundaryYDown * -1, 0));

    }
}
