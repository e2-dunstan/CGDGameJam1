using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    bool grounded = false;
    private float thrust = 100f;
    private float sideThrust = 100f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody2D>().AddForce(transform.up * thrust);
        }
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody2D>().AddForce(transform.up * thrust);
    }

}
