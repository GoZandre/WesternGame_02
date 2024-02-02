using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Cage : MonoBehaviour
{
    public float openSpeed;

    private float lerp;

    public Quaternion objectiveRot;

    public UnityEvent OnOpenCage = new UnityEvent();

    private bool isOpen;

    private void Start()
    {
        lerp = 0;
        isOpen = false;
    }

    public void OpenCage()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(CageRotation());

        }
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

        OnOpenCage.Invoke();
    }
}
