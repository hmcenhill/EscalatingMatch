using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PeekSlider peekSlider;

    private void Start()
    {
        peekSlider.Init();
    }

    public void BeginGame()
    {
        GameManager.Instance.StartGame();
    }
}
