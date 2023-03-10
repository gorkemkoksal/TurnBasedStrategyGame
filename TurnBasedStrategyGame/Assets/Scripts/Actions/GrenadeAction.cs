using System;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    [SerializeField] private Transform grenadeProjectilePrefab;
    private int maxThrowDistance = 7;
    private void Update()
    {
        if (!isActive) { return; }
        ActionEnd();
    }
    public override string GetActionName() => "Grenade";
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => new EnemyAIAction { GridPosition = gridPosition, ActionValue = 0 };
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;

                var testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        var grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition,OnGrenadeBehaviourComplete);

        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviourComplete()
    {
        ActionEnd();
    }
}
