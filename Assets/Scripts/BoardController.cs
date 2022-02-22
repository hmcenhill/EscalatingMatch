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
    private IList<CardController> cards;

    private CardController heldCard;
    private int timeRemaining;

    private bool canFlip = false;
    private const float flipCooldown = 0.5f;
    private bool peakMode;


    public void Init(bool enablePeak)
    {
        peakMode = enablePeak;
        cardPositions = new List<Vector2>();
        var cards = cardContainer.GetComponentsInChildren<CardController>();
        cardSize = cards[0].GetComponent<RectTransform>().sizeDelta;

        pairsRemaining = cards.Length / 2;
        timeRemaining = pairsRemaining * 2 + 10;
        timer.UpdateDisplay(timeRemaining);
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
        cards = new List<CardController>();
        var dealFrom = GameManager.Instance.DeckPosition.position;
        var options = Enum.GetValues(typeof(CardName)).Cast<CardName>().ToList();

        for (var i = 0; i < pairsRemaining; i++)
        {
            var pick = new System.Random().Next(options.Count);

            var first = Instantiate(cardPrefab, dealFrom, Quaternion.identity, cardContainer).GetComponent<CardController>();
            var second = Instantiate(cardPrefab, dealFrom, Quaternion.identity, cardContainer).GetComponent<CardController>();

            first.Init(this, options[pick], cardSize);
            second.Init(this, options[pick], cardSize);

            deck.Add(first);
            deck.Add(second);
            cards.Add(first);
            cards.Add(second);

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
            deck[pick].transform.SetAsLastSibling();
            deck[pick].FlyToPosition(cardPositions[i]);
            deck.RemoveAt(pick);
        }
        StartCoroutine(Begin());
    }

    private IEnumerator Begin()
    {
        var countdownTimer = 3;

        while (countdownTimer > 0)
        {
            GameManager.Instance.CountDown.UpdateDisplay(countdownTimer.ToString());
            if (peakMode && countdownTimer == 2)
            {
                foreach (var card in cards)
                {
                    card.Peak();
                }
            }
            if (peakMode && countdownTimer == 1)
            {
                foreach (var card in cards)
                {
                    card.Hide();
                }
            }
            countdownTimer--;
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.CountDown.UpdateDisplay("GO");
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
            timer.UpdateDisplay(timeRemaining);
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

    private void NoMatch(CardController first, CardController second) => StartCoroutine(NoMatchFoundCoroutine(first, second));

    private IEnumerator NoMatchFoundCoroutine(CardController first, CardController second)
    {
        second.HaltInteraction();
        first.HaltInteraction();
        yield return new WaitForSeconds(1f);
        first.Hide();
        second.Hide();
        yield return new WaitForSeconds(0.5f);
        first.AllowInteraction();
        second.AllowInteraction();
    }

    private void MatchFound(CardController first, CardController second) => StartCoroutine(MatchFoundCoroutine(first, second));

    private IEnumerator MatchFoundCoroutine(CardController first, CardController second)
    {
        first.ReturnToDeck(GameManager.Instance.CompletePosition.position);
        second.ReturnToDeck(GameManager.Instance.CompletePosition.position);

        pairsRemaining--;

        yield return new WaitForSeconds(0.5f);

        if (pairsRemaining == 0)
        {
            GameOverWin();
        }
    }

    private void GameOverWin() => GameManager.Instance.WinBoard();

    private void GameOverLose() => GameManager.Instance.LoseBoard();
}
