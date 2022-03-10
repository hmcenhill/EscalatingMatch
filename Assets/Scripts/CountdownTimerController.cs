using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private const float startFade = 0.2f;
    private const float startScale = 10f;
    private const float fadeTime = 0.3f;
    private RectTransform rect;

    public void Init()
    {
        rect = text.gameObject.GetComponent<RectTransform>();

        Hide();
    }

    public void CountdownDisplay(string s)
    {
        text.gameObject.SetActive(true);
        StartCoroutine(AnimateText(s));
    }

    public void GoDisplay(string s)
    {
        text.gameObject.SetActive(true);
        StartCoroutine(AnimateGo(s));
    }

    public void Hide()
    {
        text.gameObject.SetActive(false);
    }

    private IEnumerator AnimateText(string s)
    {
        text.text = s;
        text.alpha = startFade;
        rect.localScale = Vector3.one * startScale;

        StartCoroutine(ShrinkAndFocus(0.5f));

        yield return new WaitForSeconds(0.8f);
        Hide();
    }

    private IEnumerator ShrinkAndFocus(float timeToArrive)
    {
        var alphaStep = (1f - startFade) / timeToArrive;
        var sizeStep = (1f - startScale) / timeToArrive;
        var currentScale = startScale;

        while (1f - text.alpha > 0.01f)
        {
            text.alpha += alphaStep * Time.deltaTime;
            currentScale += sizeStep * Time.deltaTime;
            rect.localScale = Vector3.one * currentScale;
            yield return null;
        }
    }

    private IEnumerator AnimateGo(string s)
    {
        text.text = s;
        text.alpha = 1f;
        var currentScale = 1.5f;
        rect.localScale = Vector3.one * currentScale;

        yield return new WaitForSeconds(0.3f);

        var alphaStep = text.alpha / fadeTime;
        var sizeStep = startScale / fadeTime;

        while (text.alpha > 0.01f)
        {
            text.alpha -= alphaStep * Time.deltaTime;
            currentScale += sizeStep * Time.deltaTime;
            rect.localScale = Vector3.one * currentScale;

            yield return null;
        }
        Hide();
    }
}
