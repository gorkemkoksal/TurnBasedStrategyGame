using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{
    public event Action<Unit> OnShoot;
    [SerializeField] private LayerMask obstaclesLayerMask;
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    void Update()
    {
        if (!isActive) { return; }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                var rotationSpeed = 10f;
                var aimDirection = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotationSpeed);
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }
        if (stateTimer <= 0) { NextState(); }
    }
    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                var shootingStateTime = .1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                var coolOffStateTime = .5f;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionEnd();
                break;
        }
    }
    private void Shoot()
    {
        OnShoot?.Invoke(targetUnit);
        ScreenShake.Instance.Shake();

        targetUnit.Damage(40);                              
    }
    public override string GetActionName() => "Shoot";
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;

                var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance) continue;

                if (!GridLevel.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                Unit targetUnit = GridLevel.Instance.GetUnitOnGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy()) continue;

                var unitWorldPosition = GridLevel.Instance.GetWorldPosition(unitGridPosition);
                var shootDir=(targetUnit.GetWorldPosition()-unitWorldPosition).normalized;
                var unitShoulderHeight = 1.7f;
                if(Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()), obstaclesLayerMask))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = GridLevel.Instance.GetUnitOnGridPosition(gridPosition);

        canShootBullet = true;
        state = State.Aiming;
        var aimingStateTime = 1f;
        stateTimer = aimingStateTime;

        ActionStart(onActionComplete);
    }
    public Unit GetTargetUnit() => targetUnit;
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit=GridLevel.Instance.GetUnitOnGridPosition(gridPosition);

        return new EnemyAIAction
        {
            GridPosition = gridPosition,
            ActionValue = 100 + Mathf.RoundToInt((1-targetUnit.GetHealthNormalized()) * 100),
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }
}
