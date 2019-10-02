using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    private Camera mainCamera;
    private Transform playerTransform;

    [Header("New positions")]

    [Tooltip("Where the camera moves after the screen transition")]
    [SerializeField] private Transform newCameraPosition;
    private Vector3 newScreenView;

    [Tooltip("Where the player moves after the screen transition.")]
    [SerializeField] private Transform newPlayerPosition;
    private Vector2 entryPoint;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {

            playerTransform = _col.gameObject.transform;
            entryPoint = new Vector2(newPlayerPosition.position.x, playerTransform.position.y);

            newScreenView = new Vector3(newCameraPosition.position.x, newCameraPosition.transform.position.y, mainCamera.transform.position.z);
            _col.gameObject.transform.position = entryPoint;
            mainCamera.transform.position = newScreenView;
        }
    }
}
