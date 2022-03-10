using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void BeginGame()
    {
        GameManager.Instance.StartGame();
    }
}
