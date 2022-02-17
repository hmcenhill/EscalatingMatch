using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TimerDisplayController timerDisplayController;


    public void Init()
    {
        timerDisplayController.Init();
    }
}
