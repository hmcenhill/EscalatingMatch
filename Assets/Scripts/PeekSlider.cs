using UnityEngine;
using UnityEngine.UI;

public class PeekSlider : MonoBehaviour
{
    private bool peekOn;
    private Image sliderImage;

    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    private void Start()
    {
        sliderImage = this.GetComponent<Image>();
        peekOn = false;
        SetImage();
    }

    public void OnClick()
    {
        peekOn = !peekOn;
        GameManager.Instance.SetMode(peekOn);
        SetImage();
    }

    private void SetImage() => sliderImage.sprite = peekOn ? onSprite : offSprite;
}
