using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    Vector2 velocity = Vector2.zero;
    float lifetime = 0;

    public void SpawnProjectile(Vector2 _velocity, float _lifetime)
    {
        velocity = _velocity;
        lifetime = _lifetime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().TakeDamage(1);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
