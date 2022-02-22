using UnityEngine;

public class LoseMenu : MonoBehaviour
{
    public void Again()
    {
        GameManager.Instance.RepeatLevel();
    }

    public void MainMenu()
    {
        GameManager.Instance.MainMenu();
    }
}
