using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwing : MonoBehaviour
{
    public Transform player;
    public Transform webOrigin;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;

    private Vector3[] lineVerts = new Vector3[2];

    private bool isSwinging = false;


    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        springJoint = GetComponentInChildren<SpringJoint2D>();

        lineVerts[0] = webOrigin.position;
    }

    private void FixedUpdate()
    {
        lineVerts[1] = player.position;

        lineRenderer.SetPositions(lineVerts);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isSwinging = !isSwinging;
            Debug.Log("Swinging " + isSwinging);
        }

        springJoint.enabled = isSwinging;
        lineRenderer.enabled = isSwinging;
    }
}
