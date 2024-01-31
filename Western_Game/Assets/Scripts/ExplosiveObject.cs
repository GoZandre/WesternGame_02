using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObject : MonoBehaviour
{

    public void Explode()
    {
        GetComponent<SphereCollider>().enabled = true;
        StartCoroutine(DestroyTNT());
    }

    private void OnTriggerEnter(Collider other)
    {
        DestructibleObject destructibleObject;

        if (other.gameObject.TryGetComponent<DestructibleObject>(out destructibleObject))
        {
            destructibleObject.Destruction();
        }
    }

    public IEnumerator DestroyTNT()
    {
        yield return new WaitForSeconds(0.2f); Destroy(gameObject);
    }

}
