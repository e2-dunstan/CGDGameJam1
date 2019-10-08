using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Vector2 velocity = Vector2.zero;
    protected int projectileDamage;
    protected float lifetime = 0;

    public virtual void SpawnProjectile(Vector2 _velocity, float _lifetime, int _projectileDamage)
    {
        velocity = _velocity;
        lifetime = _lifetime;
        projectileDamage = _projectileDamage;
    }

    protected virtual void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
