using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private bool isFlipped;

    [SerializeField] private CardName cardName;
    [SerializeField] private Image faceDisplay;

    private Sprite backImage;
    private Sprite faceImage;

    private void Awake()
    {
        faceDisplay = this.GetComponent<Image>();
        InitializeCard();
    }

    public void InitializeCard()
    {
        backImage = Resources.Load<Sprite>($"Graphics/CardFace/Blank");
        faceImage = Resources.Load<Sprite>($"Graphics/CardFace/{cardName}");

        faceDisplay.sprite = backImage;
    }

    public void Flip()
    {
        if (isFlipped)
        {
            faceDisplay.sprite = backImage;
            isFlipped = false;
        }
        else
        {
            faceDisplay.sprite = faceImage;
            isFlipped = true;
        }

    }
}
