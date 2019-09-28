using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public Vector2 bothAxes;
    public bool action;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        bothAxes = new Vector2(horizontalInput, verticalInput);
        action = (Input.GetAxisRaw("Submit") > 0);
    }
}
