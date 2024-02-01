using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleObject : MonoBehaviour
{
    public UnityEvent OnDestroy = new UnityEvent();
    public void Destruction()
    {
        OnDestroy.Invoke();

        Debug.Log("Destroy myself");
        Destroy(gameObject);

        
    }
}
