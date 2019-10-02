using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] bool knockedBack = false;
    [SerializeField] bool stunned = false;
    [SerializeField] float upThrust = 100.0f;
    [SerializeField] float sideThrust = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GetComponent<Enemy>().enemyState == Enemy.EnemyState.STUNNED && !knockedBack)
        {
            knockedBack = true;
        }

        if(knockedBack && !stunned)
        {
            stunned = true;
            GetComponent<Rigidbody2D>().AddForce(transform.up * upThrust);
            GetComponent<Rigidbody2D>().AddForce(transform.right * sideThrust);
        }

        if(GetComponent<Enemy>().enemyState != Enemy.EnemyState.STUNNED)
        {
            stunned = false;
            knockedBack = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Enemy>().enemyState = Enemy.EnemyState.STUNNED;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Ground" && stunned)
        {
            GetComponent<Enemy>().enemyState = Enemy.EnemyState.IDLE;
        }
    }
}
