using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    void Update()
    {
        if (!isActive) { return; }

        float spinAddAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360)
        {
            isActive = false;
            onActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isActive = true;
        totalSpinAmount = 0f;
        this.onActionComplete = onActionComplete;
    }
    public override string GetActionName() => "Spin";

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { unitGridPosition };
    }
}
