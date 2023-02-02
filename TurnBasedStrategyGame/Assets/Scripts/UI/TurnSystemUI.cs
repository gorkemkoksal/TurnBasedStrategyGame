using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject enemyTurnVisualGameObject;
    private void Start()
    {
        endTurnButton.onClick.AddListener(() => { TurnSystem.Instance.NextTurn(); });
        TurnSystem.Instance.OnTurnChanged += UpdateEnemyTurnVisual;
        TurnSystem.Instance.OnTurnChanged += UpdateEndTurnButtonVisibility;


        UpdateEnemyTurnVisual();

    }
    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }
    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}
