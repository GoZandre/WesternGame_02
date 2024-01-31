using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public void Destruction()
    {
        Debug.Log("Destroy myself");
        Destroy(gameObject);

        
    }
}
