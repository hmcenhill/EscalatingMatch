using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRemainingDisplay;

    public void Init()
    {
        GameManager.Instance.TimeUpdate += UpdateDisplay;
    }

    private void UpdateDisplay(float time)
    {
        timeRemainingDisplay.text = TimeSpan.FromSeconds(time).ToString("mm:ss");
    }
}
