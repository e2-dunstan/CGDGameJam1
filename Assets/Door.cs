using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 positionToMoveTo;

    [SerializeField] private GameObject endPosition;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GameObject door;

    public float doorSpeed = 1.0f;

    private bool movementInitiated = false;

    private void Start()
    {
        positionToMoveTo = startPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInitiated == true)
        {
            door.transform.position = Vector2.MoveTowards(door.transform.position, positionToMoveTo, doorSpeed * Time.deltaTime);

            if(door.transform.position == positionToMoveTo)
            {
                movementInitiated = false;
            }
        }
    }

    public void CloseDoor()
    {
        Debug.Log("Closing Door");
        positionToMoveTo = startPosition.transform.position;
        movementInitiated = true;
    }

    public void OpenDoor()
    {
        Debug.Log("Open Door");
        positionToMoveTo = endPosition.transform.position;
        movementInitiated = true;
    }
}
