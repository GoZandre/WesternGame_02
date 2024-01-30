using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoBehvior : MonoBehaviour
{
    [Header("Parameters")]

    public float lassoSpeed;


    private float _lassoLerp;

    private LineRenderer lineRenderer;

    private Transform _grabbedObject;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _grabbedObject = null;
        _lassoLerp = 0;
    }

    public void GrabObject(RaycastHit raycastHitObject)
    {
        SetLassoObjective(raycastHitObject.transform);
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
    }
}
