using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event Action OnTurnChanged;
    private bool isPlayerTurn = true;
    private void Awake()
    {
        Instance = this;
    }
    public void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        OnTurnChanged?.Invoke();
    }
    public bool IsPlayerTurn() => isPlayerTurn;
}
