using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    private const float animTime = 0.8f;

    private bool isFlipped;
    private BoardController board;

    private int interactionStoppers;
    public bool CanInteract { get => interactionStoppers == 0; }

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
        interactionStoppers = 0;
    }

    public void HaltInteraction() => interactionStoppers++;
    public void AllowInteraction()
    {
        interactionStoppers--;
        if (interactionStoppers < 0)
        {
            interactionStoppers = 0;
        }
    }

    public void Peak()
    {
        StartCoroutine(FlipCoroutine(faceImage));
    }

    public void Show()
    {
        if (!isFlipped && CanInteract && board.TryFlip())
        {
            StartCoroutine(FlipCoroutine(faceImage));
            StartCoroutine(JumpCoroutine(this.GetComponent<RectTransform>().sizeDelta.y / 3f, animTime));
            isFlipped = true;
            board.Flip(this);
        }
    }

    public void Hide()
    {
        StartCoroutine(FlipCoroutine(backImage));
        isFlipped = false;
    }

    public void Hop() => StartCoroutine(JumpCoroutine(this.GetComponent<RectTransform>().sizeDelta.y / 8f, animTime * 3f / 8f));

    private IEnumerator FlipCoroutine(Sprite image)
    {
        HaltInteraction();
        var t = this.GetComponent<RectTransform>();

        var flipSpeed = 8 * 90f / animTime;

        var timer = 0f;
        while (timer < animTime / 8f)
        {
            t.Rotate(Vector3.up, flipSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        faceDisplay.sprite = image;
        t.eulerAngles = new Vector3(0f, -90f, 0f);

        while (timer < animTime / 4f)
        {
            t.Rotate(Vector3.up, flipSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
        t.eulerAngles = Vector3.zero;
        AllowInteraction();
    }

    private IEnumerator JumpCoroutine(float jumpHeight, float jumpTime)
    {
        HaltInteraction();
        var returnPos = this.transform.position;
        var jumpToPos = returnPos + new Vector3(0f, jumpHeight, 0f);
        var jumpSpeed = jumpHeight / jumpTime;

        var timer = 0f;

        while (timer < jumpTime / 4)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector2.MoveTowards(this.transform.position, jumpToPos, 2 * jumpSpeed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(jumpTime / 4);
        while (timer < jumpTime)
        {
            timer += Time.deltaTime;
            this.transform.position = Vector2.MoveTowards(this.transform.position, returnPos, jumpSpeed * Time.deltaTime);
            yield return null;
        }

        this.transform.position = returnPos;
        AllowInteraction();
    }

    public void ReturnToDeck(Vector2 position) => StartCoroutine(ReturnToDeckCoroutine(position));

    private IEnumerator ReturnToDeckCoroutine(Vector2 position)
    {
        yield return new WaitForSeconds(animTime + 0.5f);
        HaltInteraction();
        Hop();
        yield return new WaitForSeconds(1f);
        FlyToPosition(position);
        Destroy(this.gameObject, 3f);
    }

    public void FlyToPosition(Vector2 position)
    {
        var speed = Vector2.Distance(this.transform.position, position);
        StartCoroutine(FlyToCoroutine(position, speed));
    }

    private IEnumerator FlyToCoroutine(Vector2 pos, float speed)
    {
        HaltInteraction();
        while (Vector2.Distance(pos, this.transform.position) > 0.01)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, pos, 2 * speed * Time.deltaTime);
            yield return null;
        }
        AllowInteraction();
    }
}
