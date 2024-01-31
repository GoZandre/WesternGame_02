using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float projectileSpeed;

    public float projectileLifetime = 3;

    private float life = 0;

    public float bulletDamage;

    public float bulletStrength;

    void FixedUpdate()
    {

        life += Time.fixedDeltaTime;

        if(life >= projectileLifetime)
        {
            Destroy(gameObject);
        }

        transform.position += transform.forward * projectileSpeed / 100;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        EnemyBehavior enemy;
        ExplosiveObject explosive;
        Rigidbody rb;

        if (other.gameObject.TryGetComponent<EnemyBehavior>(out enemy))
        {
            enemy.TakeDamage(bulletDamage);

            Destroy(gameObject);
        }
        else if(other.gameObject.TryGetComponent<ExplosiveObject>(out explosive))
        {
            explosive.Explode();
        }
        else if(other.gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce(projectileSpeed * bulletStrength * transform.forward);

            Destroy(gameObject);
        }
    }
}
