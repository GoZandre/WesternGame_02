using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogSphere : MonoBehaviour
{
    public string sentence;

    [SerializeField]
    private GameObject dialogObject;
    [SerializeField]
    private TextMeshPro textMeshPro;

    private void Start()
    {
        dialogObject.SetActive(false);
        textMeshPro.text = sentence;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            dialogObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            dialogObject.SetActive(false);
        }
    }
}
