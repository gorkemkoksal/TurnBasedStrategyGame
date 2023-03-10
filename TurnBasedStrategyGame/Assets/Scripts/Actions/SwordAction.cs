using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public Action OnSwordActionStarted;
    public Action OnSwordActionCompleted;
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit,
    }
    private Unit targetUnit;
    private int maxSwordDistance = 1;
    private State state;
    private float stateTimer;
    private void Update()
    {
        if (!isActive) { return; }
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                var rotationSpeed = 10f;
                var aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);
                break;
            case State.SwingingSwordAfterHit:

                break;
        }
        if (stateTimer <= 0) { NextState(); }

    }
    private void NextState()
    {
        switch (state)
        {
            case State.SwingingSwordBeforeHit:
                state = State.SwingingSwordAfterHit;
                var afterHitStateTime = .5f;
                stateTimer = afterHitStateTime;
                targetUnit.Damage(100);
                ScreenShake.Instance.Shake(2f);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke();

                ActionEnd();
                break;

        }
    }
    public override string GetActionName() => "Sword";
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => new EnemyAIAction { GridPosition = gridPosition, ActionValue = 200 };
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();


        for (int x = -maxSwordDistance; x <= maxSwordDistance; x++)
        {
            for (int z = -maxSwordDistance; z <= maxSwordDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;
                if (!GridLevel.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = GridLevel.Instance.GetUnitOnGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = GridLevel.Instance.GetUnitOnGridPosition(gridPosition);
        state = State.SwingingSwordBeforeHit;
        var beforeHitStateTime = .7f;
        stateTimer = beforeHitStateTime;

        OnSwordActionStarted?.Invoke();
        ActionStart(onActionComplete);
    }
}
