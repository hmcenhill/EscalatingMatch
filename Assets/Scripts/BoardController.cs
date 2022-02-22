using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private TimerDisplayController timer;
    [SerializeField] private CardController cardPrefab;

    private IList<Vector2> cardPositions;
    private int pairsRemaining;
    private Vector2 cardSize;

    private CardController heldCard;
    private int timeRemaining;

    private bool canFlip = false;
    private const float flipCooldown = 1f;


    public void Init()
    {
        cardPositions = new List<Vector2>();
        var cards = cardContainer.GetComponentsInChildren<CardController>();
        cardSize = cards[0].GetComponent<RectTransform>().sizeDelta;

        pairsRemaining = cards.Length / 2;
        timeRemaining = pairsRemaining * 1 + 70;
        foreach (var card in cards)
        {
            cardPositions.Add(new Vector2(card.transform.position.x, card.transform.position.y));
            Destroy(card.gameObject);
        }

        ChooseCards();
    }

    public void ChooseCards()
    {
        var deck = new List<CardController>();
        var options = Enum.GetValues(typeof(CardName)).Cast<CardName>().ToList();

        for (var i = 0; i < pairsRemaining; i++)
        {
            var pick = new System.Random().Next(options.Count);

            var first = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity, cardContainer).GetComponent<CardController>();
            var second = Instantiate(cardPrefab, Vector2.zero, Quaternion.identity, cardContainer).GetComponent<CardController>();

            first.Init(this, options[pick], cardSize);
            second.Init(this, options[pick], cardSize);

            deck.Add(first);
            deck.Add(second);

            options.RemoveAt(pick);
        }

        StartCoroutine(Deal(deck));
    }

    private IEnumerator Deal(IList<CardController> deck)
    {
        for (var i = 0; i < pairsRemaining * 2; i++)
        {
            yield return new WaitForSeconds(0.1f);

            var pick = new System.Random().Next(deck.Count);
            deck[pick].FlyToPosition(cardPositions[i]);
            deck.RemoveAt(pick);
        }
        Begin();
    }

    public void Begin()
    {
        RunCountdownTimer();
        canFlip = true;
    }

    private void RunCountdownTimer()
    {
        timeRemaining--;
        if (timeRemaining < 0)
        {
            GameOverLose();
        }
        else
        {
            timer.UpdateDisplay((int)timeRemaining);
            StartCoroutine(nameof(CountdownCoroutine));
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(1f);
        RunCountdownTimer();
    }


    public bool TryFlip()
    {
        if (canFlip)
        {
            canFlip = false;
            StartFlipCooldown();
            return true;
        }
        return false;
    }

    private void StartFlipCooldown() => StartCoroutine(nameof(FlipCooldownCoroutine));

    private IEnumerator FlipCooldownCoroutine()
    {
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
    }

    public void Flip(CardController card)
    {
        if (heldCard == null)
        {
            heldCard = card;
        }
        else
        {
            if (card.Card == heldCard.Card)
            {
                MatchFound(heldCard, card);
            }
            else
            {
                NoMatch(heldCard, card);
            }
            heldCard = null;
        }
    }

    private void NoMatch(CardController first, CardController second)
    {
        first.Hide();
        second.Hide();
    }

    private void MatchFound(CardController first, CardController second)
    {
        Destroy(first.gameObject);
        Destroy(second.gameObject);

        pairsRemaining--;

        if (pairsRemaining == 0)
        {
            GameOverWin();
        }
    }

    private void GameOverWin()
    {
        Debug.Log("Winner!");
    }

    private void GameOverLose()
    {
        Debug.Log("OuttaTime!");
    }
}
