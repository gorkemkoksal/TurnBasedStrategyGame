using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [field: SerializeField] private bool isEnemy;
    private const int MAX_ACTION_POINT = 2;

    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    private GridPosition gridPosition;
    private HealthSystem healthSystem;
 
    private BaseAction[] baseActionArray;
    private int actionPoints = MAX_ACTION_POINT;
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChanged += OnTurnEnd;

        healthSystem.OnDead += HealthSystem_OnDead;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }
    void Update()
    {
        GridPosition newGridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            GridLevel.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }
    public T GetAction<T>() where T : BaseAction
    {
        foreach(BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
       
    }
    public GridPosition GetGridPosition() => gridPosition;
    public Vector3 GetWorldPosition() => transform.position;
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public bool IsEnemy() => isEnemy;

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => actionPoints >= baseAction.GetActionPointCost();
    private void SpendActionPoints(int amount) => actionPoints -= amount;
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!CanSpendActionPointsToTakeAction(baseAction)) return false;

        SpendActionPoints(baseAction.GetActionPointCost());
        return true;
    }
    private void OnTurnEnd()
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            actionPoints = MAX_ACTION_POINT;
        }
    }
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    private void HealthSystem_OnDead()
    {
        GridLevel.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        Destroy(gameObject);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized() => healthSystem.GetHealthNormalized();
}
