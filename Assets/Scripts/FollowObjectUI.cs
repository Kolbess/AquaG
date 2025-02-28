using System;
using UnityEngine;
using UnityEngine.UI;

public class FollowObjectUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
}