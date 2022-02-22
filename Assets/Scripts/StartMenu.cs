using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void BeginNormal()
    {
        GameManager.Instance.SetMode(false);
        BeginGame();
    }

    public void BeginPeek()
    {
        GameManager.Instance.SetMode(true);
        BeginGame();
    }

    private void BeginGame()
    {
        GameManager.Instance.StartGame();
    }
}
