using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{
    public ParticleSystem explosiveParticles;

    private bool isExploded;

    [SerializeField]
    private float ExplosionSpeed;

    private void Awake()
    {
        isExploded = false;
    }
    public void Explode()
    {
        if (!isExploded)
        {
            isExploded = true;

            GetComponent<SphereCollider>().enabled = true;

            ParticleSystem part = Instantiate(explosiveParticles);
            part.transform.position = transform.position;

           for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }


            StartCoroutine(DestroyTNT());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);

        DestructibleObject destructibleObject;
        EnemyBehavior enemy;
        Rigidbody rb;
        ExplosiveObject explosive;

        if (other.gameObject.TryGetComponent<DestructibleObject>(out destructibleObject))
        {
            destructibleObject.Destruction();
        }
        else if(other.gameObject.TryGetComponent<EnemyBehavior>(out enemy))
        {
            enemy.TakeDamage(1000f);
        }
        else if(other.gameObject.TryGetComponent<ExplosiveObject>(out explosive))
        {
            explosive.Explode();
        }
        else if(other.gameObject.TryGetComponent<Rigidbody>(out rb))
        {
            rb.AddForce((other.transform.position - transform.position).normalized * 1000f);
        }

    }

    private float lerp;

    public IEnumerator DestroyTNT()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        float baseRadius = sphereCollider.radius;

        sphereCollider.radius = 0;

        while (lerp < 1)
        {
            sphereCollider.radius = Mathf.Lerp(0, baseRadius, lerp);

            yield return null;
            lerp += Time.deltaTime * ExplosionSpeed;
        }
        Destroy(gameObject);
    }

}
