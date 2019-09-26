using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private Transform newCameraPosition;
    [SerializeField] private Transform newPlayerPosition;
    private Transform playerTransform;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.transform;
            collision.gameObject.transform.position = new Vector2(newPlayerPosition.position.x, playerTransform.position.y);
            mainCamera.transform.position = new Vector3(newCameraPosition.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }
}
