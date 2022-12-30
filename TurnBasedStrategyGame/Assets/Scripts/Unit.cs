using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private int actionPoints = 2;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
    private void Start()
    {
        gridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        GridLevel.Instance.AddUnitAtGridPosition(gridPosition, this);
    }
    void Update()
    {
        GridPosition newGridPosition = GridLevel.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridLevel.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }
    public MoveAction GetMoveAction() => moveAction;
    public SpinAction GetSpinAction() => spinAction;
    public GridPosition GetGridPosition() => gridPosition;
    public BaseAction[] GetBaseActionArray() => baseActionArray;

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => actionPoints >= baseAction.GetActionPointCost();
    private void SpendActionPoints(int amount) => actionPoints -= amount;
    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!CanSpendActionPointsToTakeAction(baseAction)) return false;

        SpendActionPoints(baseAction.GetActionPointCost());
        return true;
    }
}
