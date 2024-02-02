using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetRb : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.centerOfMass = Vector3.zero;
    }

}
