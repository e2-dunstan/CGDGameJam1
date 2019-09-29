using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwing : MonoBehaviour
{
    [SerializeField] private Transform webOrigin;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;

    private Vector3[] lineVerts = new Vector3[2];

    private bool isSwinging = false;


    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        springJoint = GetComponentInChildren<SpringJoint2D>();

        lineVerts[0] = webOrigin.position;

        isSwinging = false;
        UpdateSwinging();
    }

    private void FixedUpdate()
    {
        lineVerts[1] = Player.Instance().transform.position;
        lineRenderer.SetPositions(lineVerts);
    }

    public void ToggleSwinging()
    {
        isSwinging = !isSwinging;
        UpdateSwinging();
    }

    private void UpdateSwinging()
    {
        springJoint.enabled = isSwinging;
        lineRenderer.enabled = isSwinging;

        Player.Instance().CurrentPlayerState = isSwinging ? Player.PlayerState.WEBBING : Player.PlayerState.AIRBORNE;
        Player.Instance().PlayerMovement.Rigidbody.gravityScale = isSwinging ? 100 : 1;

        Debug.Log("Swinging " + isSwinging);
    }
}
