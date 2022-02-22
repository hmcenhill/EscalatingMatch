using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] private Transform playArea;
    [SerializeField] private Transform deckPosition;
    [SerializeField] private Transform completePosition;
    [SerializeField] CountdownTimerController cdc;

    private BoardController currentBoard;
    private int currentLevel = 1;
    private const int maxLevel = 4;
    private bool peekMode = false;

    public CountdownTimerController CountDown { get => cdc; }
    public Transform DeckPosition { get => deckPosition; }
    public Transform CompletePosition { get => completePosition; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetMode(bool peekMode)
    {
        this.peekMode = peekMode;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        cdc.Init();
        MainMenu();
    }

    public void MainMenu()
    {
        ClearPlayArea();
        Instantiate(Resources.Load<GameObject>($"Prefabs/StartMenu"), playArea);
    }

    public void LoadLevel(int level)
    {
        ClearPlayArea();

        currentBoard = Instantiate(Resources.Load<GameObject>($"Prefabs/Boards/Level{level}"), playArea).GetComponent<BoardController>();
        currentBoard.Init(peekMode);
    }

    public void StartGame()
    {
        currentLevel = 1;
        LoadLevel(currentLevel);
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel > maxLevel)
        {
            currentLevel = maxLevel;
        }
        LoadLevel(currentLevel);
    }

    public void RepeatLevel()
    {
        LoadLevel(currentLevel);
    }

    private void ClearPlayArea()
    {
        foreach (Transform child in playArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void WinBoard()
    {
        ClearPlayArea();
        Instantiate(Resources.Load<GameObject>($"Prefabs/Win"), playArea);
    }

    public void LoseBoard()
    {
        ClearPlayArea();
        Instantiate(Resources.Load<GameObject>($"Prefabs/Lose"), playArea);
    }
}