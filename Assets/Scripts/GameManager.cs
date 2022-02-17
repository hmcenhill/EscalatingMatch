using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] private UIController ui;

    public event Action<float> TimeUpdate;


    private float timeRemaining;

    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {

        ui.Init();
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            TimeUpdate(timeRemaining);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {

    }
}