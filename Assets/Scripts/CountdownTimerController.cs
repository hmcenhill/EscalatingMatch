using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Init()
    {
        Hide();
    }

    public void UpdateDisplay(string s)
    {
        text.gameObject.SetActive(true);
        StartCoroutine(AnimateText(s));
    }

    public void Hide()
    {
        text.gameObject.SetActive(false);
    }

    private IEnumerator AnimateText(string s)
    {
        text.text = s;
        yield return new WaitForSeconds(0.8f);
        Hide();
    }
}
