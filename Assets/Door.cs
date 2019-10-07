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
    // Update is called once per frame
    void Update()
    {
        door.transform.position = Vector2.MoveTowards(gameObject.transform.position, positionToMoveTo, 1.0f);
    }

    public void CloseDoor()
    {

        positionToMoveTo = startPosition.transform.position;
    }

    public void OpenDoor()
    {
        positionToMoveTo = endPosition.transform.position;
    }
}
