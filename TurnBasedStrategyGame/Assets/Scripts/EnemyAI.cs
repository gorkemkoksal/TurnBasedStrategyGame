using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private float Timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TimerReset;
    }
    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn()) { return; }
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            TurnSystem.Instance.NextTurn();
        }
    }
    private void TimerReset()
    {
        Timer = 2f;
    }
}
