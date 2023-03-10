using System;
using System.Collections.Generic;

public class InteractAction : BaseAction
{
    private int maxInteractInstance = 1;
    private void Update()
    {
        if (!isActive) { return; }
    }
    public override string GetActionName() => "Interact";

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) => new EnemyAIAction { GridPosition = gridPosition, ActionValue = 0 };

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        var unitGridPosition = unit.GetGridPosition();


        for (int x = -maxInteractInstance; x <= maxInteractInstance; x++)
        {
            for (int z = -maxInteractInstance; z <= maxInteractInstance; z++)
            {
                GridPosition offSetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if (!GridLevel.Instance.IsValidGridPosition(testGridPosition)) continue;
                var interactable = GridLevel.Instance.GetInteractableAtGridPosition(testGridPosition);
                if (interactable == null) continue;
                if (GridLevel.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;

                validGridPositionList.Add(testGridPosition);
            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        var interactable = GridLevel.Instance.GetInteractableAtGridPosition(gridPosition);
        interactable.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }
    private void OnInteractComplete()
    {
        ActionEnd();
    }
}
