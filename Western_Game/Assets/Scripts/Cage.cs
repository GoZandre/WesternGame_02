using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    public float openSpeed;

    private float lerp;

    public Quaternion objectiveRot;

    private void Start()
    {
        lerp = 0;
    }

    public void OpenCage()
    {
        StartCoroutine(CageRotation());
    }

    private IEnumerator CageRotation()
    {
        Quaternion startRot = transform.rotation;

        while (lerp < 1)
        {
            Debug.Log(lerp);

            yield return null;
            lerp += Time.deltaTime * openSpeed;

            Quaternion newRot = Quaternion.Slerp(startRot, objectiveRot, lerp);

            transform.rotation = newRot;
        }
    }
}
