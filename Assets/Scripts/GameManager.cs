using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField] private Transform playArea;
    private BoardController currentBoard;
    [SerializeField] private int level;


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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        ClearPlayArea();
        currentBoard = Instantiate(Resources.Load<GameObject>($"Prefabs/Boards/Level{level}"), playArea).GetComponent<BoardController>();
        currentBoard.Init();
    }

    private void ClearPlayArea()
    {
        foreach(Transform child in playArea)
        {
            Destroy(child.gameObject);
        }
    }


}