using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public enum MovementPositionType
{
    DEFAULT_RETURN_POSITION = 0,
    WONDERING_POSITION = 1,
    SHOOTING_POSITION = 2
}

[System.Serializable]
public struct MovementPosition
{ 
    public MovementPositionType movementPositionType;
    public GameObject movementObj;
}

public class VultureMovement : EnemyMovement
{
    public List<MovementPosition> movementPositions;

    [SerializeField] private bool canFly = true;

    [SerializeField] private bool hasRandomTargetLocation = false;

    [SerializeField] private Vector3 randomLocation;
    
    [SerializeField] private int randomPositionIndex = 0;
    [SerializeField] private int randomShootingPos = 0;

    public bool goingToShootingPos = false;
    public bool goingToDefaultPos = false;

    [SerializeField] private float maxWaitTimeBetweenRepositioning = 3000.0f;

    [SerializeField] private float minWaitTimeBetweenRepositioning = 500.0f;

    [SerializeField] private float timeBetweenMoving = 0.0f;
    private float timeToWaitBeforeMoving = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = gameObject.transform.position;

        timeToWaitBeforeMoving = Random.Range(minWaitTimeBetweenRepositioning, maxWaitTimeBetweenRepositioning);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void MoveWithinDefinedWonderingBounds()
    {

        if (goingToShootingPos == false && goingToDefaultPos == false)
        {
            if (hasRandomTargetLocation == false)
            {
                randomLocation = GetRandomMovementPositionOfType(MovementPositionType.WONDERING_POSITION).movementObj.transform.position;
                targetDestination = randomLocation;
                timeToWaitBeforeMoving = Random.Range(minWaitTimeBetweenRepositioning, maxWaitTimeBetweenRepositioning);
                hasRandomTargetLocation = true;
            }
        }
        else if(goingToShootingPos == true)
        {
            if (hasRandomTargetLocation == false)
            {
                randomLocation = GetRandomMovementPositionOfType(MovementPositionType.SHOOTING_POSITION).movementObj.transform.position;
                targetDestination = randomLocation;
                timeToWaitBeforeMoving = Random.Range(minWaitTimeBetweenRepositioning, maxWaitTimeBetweenRepositioning);
                goingToShootingPos = false;
                hasRandomTargetLocation = true;
            }
        }
        else if(goingToDefaultPos == true)
        {
            if (hasRandomTargetLocation == false)
            {
                randomLocation = GetRandomMovementPositionOfType(MovementPositionType.DEFAULT_RETURN_POSITION).movementObj.transform.position;
                targetDestination = randomLocation;
                timeToWaitBeforeMoving = Random.Range(minWaitTimeBetweenRepositioning, maxWaitTimeBetweenRepositioning);
                goingToDefaultPos = false;
                hasRandomTargetLocation = true;
            }
        }

        //If current position = end position
        if (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)
            != new Vector2(targetDestination.x, targetDestination.y) && hasRandomTargetLocation == true)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetDestination, moveSpeed);
        }
        else
        {
            hasReachedDestination = true;

            if (timeToWaitBeforeMoving <= timeBetweenMoving)
            {
                timeToWaitBeforeMoving = Random.Range(minWaitTimeBetweenRepositioning, maxWaitTimeBetweenRepositioning);
                timeBetweenMoving = 0.0f;
                hasRandomTargetLocation = false;
                hasReachedDestination = false;  
            }

            timeBetweenMoving += Time.deltaTime;
        }
    }

    private MovementPosition GetRandomMovementPositionOfType(MovementPositionType _movementPositionType)
    {
        
        List<MovementPosition> possiblePositions = movementPositions.Where(x => x.movementPositionType == _movementPositionType && x.movementObj.transform.position != targetDestination).ToList();

        int randomPossiblePositionIndex = Random.Range(0, possiblePositions.Count);

        MovementPosition randomMovementPositionFound = possiblePositions[randomPossiblePositionIndex];

        return randomMovementPositionFound;
    }

    public override void MoveTowardsDestination(Vector2 _targetDestination)
    {
        targetDestination = _targetDestination;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _targetDestination, moveSpeed);
    }

    public void ForceMoveToShootingPosition()
    {
        hasRandomTargetLocation = false;
        goingToDefaultPos = false;
        goingToShootingPos = true;
    }

    public void ForceMoveToDefaultPosition()
    {
        hasRandomTargetLocation = false;
        goingToDefaultPos = true;
        goingToShootingPos = false;
    }

    public override void ForceMoveToSpecificPosition(Vector2 _targetPosition)
    {
        hasRandomTargetLocation = true;
        goingToDefaultPos = false;
        goingToShootingPos = false;
        targetDestination = _targetPosition;
    }
}
