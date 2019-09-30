using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebProjectile : MonoBehaviour
{
    Vector2 velocity = Vector2.zero;
    int projectileDamage;
    float lifetime = 0;

    public void SpawnProjectile(Vector2 _velocity, float _lifetime, int _projectileDamage)
    {
        velocity = _velocity;
        lifetime = _lifetime;
        projectileDamage = _projectileDamage;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().InflictDamage(projectileDamage);
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
