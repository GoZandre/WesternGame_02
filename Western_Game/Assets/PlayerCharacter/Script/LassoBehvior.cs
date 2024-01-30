using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoBehvior : MonoBehaviour
{
    [Header("Parameters")]

    public float lassoSpeed;


    private float _lassoLerp;

    private LineRenderer lineRenderer;
    private SpringJoint joint;

    private Transform _grabbedObject;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<SpringJoint>();
        _grabbedObject = null;
        _lassoLerp = 0;
    }

    public void GrabObject(RaycastHit raycastHitObject)
    {
        _lassoLerp = 0;
        SetLassoObjective(raycastHitObject.transform);

        Rigidbody rb;

        if(raycastHitObject.transform.TryGetComponent<Rigidbody>(out rb))
        {
            joint.connectedBody = rb;
            joint.connectedAnchor = raycastHitObject.transform.position - transform.position;
        }
        
    }

    public void UngrabObject()
    {
        _grabbedObject = null;
        joint.connectedBody = null;
    }

    public void SetLassoObjective(Transform newTransform)
    {
        _grabbedObject = newTransform;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        if (_grabbedObject != null)
        {
            if(_lassoLerp <= 1)
            {
                _lassoLerp += Time.deltaTime * lassoSpeed;

                Vector3 newPos = Vector3.Lerp(lineRenderer.GetPosition(0), _grabbedObject.position, _lassoLerp);

                lineRenderer.SetPosition(1, newPos);
            }
            else
            {
                lineRenderer.SetPosition(1, _grabbedObject.position);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
