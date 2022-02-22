using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class TimerDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeRemainingDisplay;

    public void UpdateDisplay(int time)
    {
        var timeSb = new StringBuilder();

        if (time >= 60)
        {
            timeSb.Append($"{time / 60}:");
            time %= 60;
        }
        if (timeSb.Length > 0 && time < 10)
        {
            timeSb.Append("0");
        }
        timeSb.Append(time);
        timeRemainingDisplay.text = timeSb.ToString();
    }
}
