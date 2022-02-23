using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image bar;

    private ITimed whoWantsToKnow;
    private float startTime;
    private float timeRemaining;

    public void SetTimer(int time, ITimed who)
    {
        whoWantsToKnow = who;
        startTime = time;
        timeRemaining = startTime;

        UpdateDisplay(timeRemaining);
    }

    public void StartTimer() => StartCoroutine(RunTimer());
    public void CancelTimer() => StopAllCoroutines();

    private IEnumerator RunTimer()
    {
        yield return new WaitForFixedUpdate();
        timeRemaining -= Time.fixedDeltaTime;
        if (timeRemaining < 0)
        {
            TimesUp();
        }
        else
        {
            UpdateDisplay(timeRemaining);
            StartCoroutine(nameof(RunTimer));
        }
    }

    private void TimesUp()
    {
        whoWantsToKnow.TimesUp();
    }

    private void UpdateDisplay(float time)
    {
        UpdateBarDisplay(time);
        UpdateTextDisplay((int)time);
    }

    private void UpdateBarDisplay(float time)
    {
        if (startTime == 0) return;

        var rtr = timeRemaining / startTime;

        bar.GetComponent<RectTransform>().localScale = new Vector3(rtr, 1f, 1f);
        if (rtr > 0.5)
        {
            bar.color = Color.Lerp(Color.yellow, Color.green, (rtr - 0.5f) / 0.5f);
        }
        else
        {
            bar.color = Color.Lerp(Color.red, Color.yellow, rtr / 0.5f);
        }
    }

    private void UpdateTextDisplay(int time)
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
        text.text = timeSb.ToString();

    }
}
