using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{
    
    private Slider _slider;
    private RectTransform _rectTransform;

    public Transform enemyTransform;

    public MeshRenderer mr;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _rectTransform = GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (enemyTransform != null)
        {
            if (mr.isVisible)
            {
                Vector3 ScreenPos = Camera.main.WorldToScreenPoint(enemyTransform.position);
                _rectTransform.position = ScreenPos;
            }
            else
            {
                _rectTransform.position = new Vector3(-100,-100,-100);
            }
            

           
        }
    }


    public void UpdateHealth(float health)
    {
        _slider.value = health;
    }
}
