using UnityEngine;

public class WinMenu : MonoBehaviour
{
    public void Again()
    {
        GameManager.Instance.RepeatLevel();
    }

    public void Next()
    {
        GameManager.Instance.NextLevel();
    }
}
