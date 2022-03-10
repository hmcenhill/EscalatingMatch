using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Image dimmer;

    private float dimmedAlpha = 0.3f;
    private float dimSpeed = 0.25f;

    public void Dim()
    {
        StopCoroutine(nameof(UndimCoroutine));
        StartCoroutine(nameof(DimCoroutine));
    }

    public void Undim()
    {
        StopCoroutine(nameof(DimCoroutine));
        StartCoroutine(nameof(UndimCoroutine));
    }

    private IEnumerator DimCoroutine()
    {
        var c = Color.black;
        var a = dimmer.color.a;
        var dimStep = (dimmedAlpha - a) / dimSpeed;

        while (a < dimmedAlpha)
        {
            a += dimStep * Time.deltaTime;
            c.a = Mathf.Min(a, dimmedAlpha);
            dimmer.color = c;
            yield return null;
        }
    }

    private IEnumerator UndimCoroutine()
    {
        var c = Color.black;
        var a = dimmer.color.a;
        var dimStep = a / dimSpeed;

        while (a > 0f)
        {
            a -= dimStep * Time.deltaTime;
            c.a = Mathf.Max(a, 0f);
            dimmer.color = c;
            yield return null;
        }
    }
}
