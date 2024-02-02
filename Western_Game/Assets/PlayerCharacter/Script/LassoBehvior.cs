using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LassoBehvior : MonoBehaviour
{
    public UnityEvent OnGrabObject = new UnityEvent();
    public UnityEvent OnUngrabObject = new UnityEvent();

    [Header("Parameters")]

    public float lassoSpeed;


    private float _lassoLerp;

    private LineRenderer lineRenderer;
    private SpringJoint joint;
    public SpringJoint Joint => joint;

    public Transform grabbedObject;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        joint = GetComponent<SpringJoint>();

        OnGrabObject.RemoveAllListeners();
        OnUngrabObject.RemoveAllListeners();

        grabbedObject = null;
        _lassoLerp = 0;
    }
    private float previousMass;

    public void GrabObject(RaycastHit raycastHitObject)
    {
        Joint.autoConfigureConnectedAnchor = true;

        _lassoLerp = 0;
        SetLassoObjective(raycastHitObject.transform);

        Rigidbody rb;

        if(raycastHitObject.transform.TryGetComponent<Rigidbody>(out rb))
        {
            //
            joint.connectedBody = rb;

            previousMass = rb.mass;
            rb.mass = 0.5f;

            
        }

        OnGrabObject.Invoke();
    }

    public void UngrabObject()
    {
        OnUngrabObject.Invoke();
        OnUngrabObject.RemoveAllListeners();

        grabbedObject.GetComponent<Rigidbody>().mass = previousMass;

        grabbedObject = null;
        joint.connectedBody = null;
    }

    public void SetLassoObjective(Transform newTransform)
    {
        grabbedObject = newTransform;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);

        if (grabbedObject != null)
        {
            if(_lassoLerp <= 1)
            {
                _lassoLerp += Time.deltaTime * lassoSpeed;

                Vector3 newPos = Vector3.Lerp(lineRenderer.GetPosition(0), grabbedObject.position, _lassoLerp);

                lineRenderer.SetPosition(1, newPos);
            }
            else
            {
                lineRenderer.SetPosition(1, grabbedObject.position);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
