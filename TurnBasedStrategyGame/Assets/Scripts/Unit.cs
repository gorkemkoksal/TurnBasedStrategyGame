using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [field: SerializeField] private bool isEnemy;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private const int MAX_ACTION_POINT = 2;
    private int actionPoints = MAX_ACTION_POINT;
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

        TurnSystem.Instance.OnTurnChanged += OnTurnEnd;
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
    public Vector3 GetWorldPosition()=> transform.position;
    public BaseAction[] GetBaseActionArray() => baseActionArray;
    public bool IsEnemy() => isEnemy;

    private bool CanSpendActionPointsToTakeAction(BaseAction baseAction) => actionPoints >= baseAction.GetActionPointCost();
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
    public void Damage()
    {
        print(transform + "damaged");
    }
}
