using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class DialogSphere : MonoBehaviour
{

    public string sentence;

    [SerializeField]
    private GameObject dialogObject;
    [SerializeField]
    private TextMeshPro textMeshPro;


    public UnityEvent OnPlayDialog = new UnityEvent();

    private void Start()
    {
        dialogObject.SetActive(false);
        playedDialog = false;
        textMeshPro.text = sentence;
    }

    bool playedDialog;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            dialogObject.SetActive(true);

            if (!playedDialog)
            {
                OnPlayDialog.Invoke();
                playedDialog = true;
            }
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
