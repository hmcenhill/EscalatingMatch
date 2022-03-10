using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private Image sunBurstEffect;
    [SerializeField] private TextMeshProUGUI timeElapsed;
    [SerializeField] private TextMeshProUGUI movesTaken;

    private void Start()
    {
        StartCoroutine(nameof(GrowSunburst));
    }

    public void SetValues(string time, string moves)
    {
        timeElapsed.text = time;
        movesTaken.text = moves;
    }

    private IEnumerator GrowSunburst()
    {
        var sunScale = sunBurstEffect.gameObject.GetComponent<RectTransform>();
        var targetSize = 1.5f;
        var jumpHeight = 30f;
        var timeToArrive = 0.3f;
        var stepSpeed = targetSize / timeToArrive;
        var jumpSpeed = jumpHeight / timeToArrive;
        var currentSize = 0.05f;

        sunScale.localScale = Vector2.one * currentSize;
        while (currentSize < targetSize)
        {
            currentSize += stepSpeed * Time.deltaTime;
            sunScale.localScale = Vector2.one * currentSize;
            sunScale.transform.localPosition += new Vector3(0f, jumpSpeed * Time.deltaTime, 0f);
            yield return null;
        }

        var settleTime = 0f;
        while (settleTime < timeToArrive)
        {
            settleTime += Time.deltaTime;
            sunScale.transform.localPosition -= new Vector3(0f, jumpSpeed * Time.deltaTime, 0f);
            yield return null;
        }
    }


    public void Again()
    {
        GameManager.Instance.RepeatLevel();
    }

    public void Next()
    {
        GameManager.Instance.NextLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

}
