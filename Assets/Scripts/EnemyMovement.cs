using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    bool grounded = false;
    public float thrust = 100000f;
    public float sideThrust = 50000f;
    bool chasePlayer = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
        
            GetComponent<Rigidbody2D>().AddForce(transform.up * thrust);
            GetComponent<Rigidbody2D>().AddForce(transform.right * sideThrust);
        }

        if (chasePlayer)
        {
            Vector2 direction = player.transform.position - transform.position;
            if (direction.x < 0)
            {
                direction.Normalize();

            }
            else
            { 
                direction.Normalize();
                direction *= -1;
            }
            transform.position = new Vector3(transform.position.x + 10.0f * Time.fixedDeltaTime * direction.x, transform.position.y, transform.position.z);

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            chasePlayer = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            chasePlayer = false;
    }
}
