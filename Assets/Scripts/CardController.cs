using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private const float flipSpeed = 5f;

    private bool isFlipped;
    private BoardController board;

    private CardName cardName;
    public CardName Card { get => cardName; }
    private Image faceDisplay;

    private Sprite backImage;
    private Sprite faceImage;

    public void Init(BoardController board, CardName cardName, Vector2 size)
    {
        this.board = board;
        this.cardName = cardName;

        faceDisplay = this.GetComponent<Image>();
        this.GetComponent<RectTransform>().sizeDelta = size;

        backImage = Resources.Load<Sprite>($"Graphics/CardFace/Blank");
        faceImage = Resources.Load<Sprite>($"Graphics/CardFace/{cardName}");

        faceDisplay.sprite = backImage;
        isFlipped = false;
    }

    public void Show()
    {
        if (!isFlipped && board.TryFlip())
        {
            FlipTo(faceImage);
            isFlipped = true;
            board.Flip(this);
        }
    }

    public void Hide()
    {
        FlipTo(backImage);
        isFlipped = false;
    }

    private void FlipTo(Sprite image) => StartCoroutine(FlipCoroutine(image));

    private IEnumerator FlipCoroutine(Sprite image)
    {
        var t = this.GetComponent<RectTransform>();

        var returnSize = t.sizeDelta;
        var flatSize = new Vector2(0f, returnSize.y);
        var flatSpeed = returnSize.x / (flipSpeed / 2f);

        //var returnPos = t.position;
        var returnPos = this.transform.position;
        var jumpToPos = returnPos + new Vector3(0f, returnSize.y / 2, 0f);
        var jumpSpeed = returnSize.y / (flipSpeed / 2f);

        var timer = 0f;
        while (timer < flipSpeed / 2f)
        {
            t.sizeDelta = Vector2.MoveTowards(t.sizeDelta, flatSize, flatSpeed * Time.deltaTime);
            this.transform.position = Vector2.MoveTowards(this.transform.position, jumpToPos, jumpSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        faceDisplay.sprite = image;

        while (timer < flipSpeed)
        {
            t.sizeDelta = Vector2.MoveTowards(t.sizeDelta, returnSize, flatSpeed * Time.deltaTime);
            this.transform.position = Vector2.MoveTowards(this.transform.position, returnPos, jumpSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }

        t.sizeDelta = returnSize;
        this.transform.position = returnPos;
    }

    public void FlyToPosition(Vector2 position)
    {
        this.transform.position = position;

        var speed = Vector2.Distance(this.transform.position, position) / 5f;
        StartCoroutine(FlyToCoroutine(position, speed));
    }

    private IEnumerator FlyToCoroutine(Vector2 pos, float speed)
    {
        while (Vector2.Distance(pos, this.transform.position) > 0.01)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pos, speed);
            yield return null;
        }
    }
}
