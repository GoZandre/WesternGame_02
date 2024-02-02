using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;


    public Canvas mainCanvas;

    [Header("Reputation")]
    public Slider reputationSlider;
    public float reputationValue;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        
    }

    private void Start()
    {
        UpdateReputationSlider();
    }

    private void UpdateReputationSlider()
    {
        reputationSlider.value = reputationValue;
    }

    public void UpdateReputation(float reputationUpdate)
    {
        reputationValue += reputationUpdate;

        Mathf.Clamp(reputationValue, 0f, 1f);

        UpdateReputationSlider();
    }
}
