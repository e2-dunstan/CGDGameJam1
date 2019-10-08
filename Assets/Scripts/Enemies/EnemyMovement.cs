using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private bool viewGizmos = true;

    public float moveSpeed = 1.0f;
    public Vector2 spawnPosition;

    [SerializeField] protected float wonderingBoundaryXLeft = 1;
    [SerializeField] protected float wonderingBoundaryXRight= 1;

    [SerializeField] protected float wonderingBoundaryYUp= 1;
    [SerializeField] protected float wonderingBoundaryYDown = 1;
    [SerializeField] protected bool canFly = false;

    [SerializeField] public bool hasRandomTargetLocation = false;

    [SerializeField] protected Vector2 randomLocation;

    [SerializeField] protected float randomXLocation;
    [SerializeField] protected float randomYLocation;

    public Vector3 targetDestination;
    public bool hasReachedDestination = false;

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

    public virtual void ForceMoveToSpecificPosition(Vector2 _targetPosition)
    {

    }

    public virtual bool IsWithinBounds(Transform entity)
    {
        if ((entity.position.x < (spawnPosition.x - wonderingBoundaryXLeft) || entity.position.x > (spawnPosition.x + wonderingBoundaryXRight)) ||
            (entity.position.y < (spawnPosition.y - wonderingBoundaryYDown) || entity.position.y > (spawnPosition.y + wonderingBoundaryYUp)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!viewGizmos) return;

        Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x - (wonderingBoundaryXLeft / 2), spawnPosition.y, 0),
                        new Vector3(wonderingBoundaryXLeft * -1, 6, 0));


        Gizmos.color = new Color(0.5f, 1.0f, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x + (((wonderingBoundaryXRight) + (wonderingBoundaryXLeft * -1)) / 2), spawnPosition.y + (wonderingBoundaryYUp / 2), 0),
                        new Vector3((wonderingBoundaryXRight + (wonderingBoundaryXLeft)), wonderingBoundaryYUp, 0));


        Gizmos.color = new Color(1, 0, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x + (((wonderingBoundaryXRight) + (wonderingBoundaryXLeft * -1)) / 2), spawnPosition.y - (wonderingBoundaryYDown / 2), 0),
                        new Vector3((wonderingBoundaryXRight + (wonderingBoundaryXLeft)), wonderingBoundaryYDown * -1, 0));


        Gizmos.color = new Color(0, 0, 0.5f, 0.3f);
        Gizmos.DrawCube(new Vector3(spawnPosition.x + (wonderingBoundaryXRight / 2), spawnPosition.y, 0),
                        new Vector3(wonderingBoundaryXRight, 6, 0));


    }
}
