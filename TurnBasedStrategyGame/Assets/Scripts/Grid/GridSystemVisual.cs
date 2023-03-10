using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance;

    [SerializeField] private Transform gridSystemVisualSinglePrefab;
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //  UnitActionSystem.Instance.OnBusyChanged += ShowHideGrid;   //ben ekledim

        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            GridLevel.Instance.GetWidth(),
            GridLevel.Instance.GetHeight()
            ];

        for (int x = 0; x < GridLevel.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < GridLevel.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform =
                    Instantiate(gridSystemVisualSinglePrefab, GridLevel.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        GridLevel.Instance.OnAnyUnitMovedGridPosition += GridLevel_OnAnyUnitMovedGridPosition;
        UpdateGridVisual();
    }
    private void UnitActionSystem_OnSelectedActionChange()
    {
        UpdateGridVisual();
    }
    private void GridLevel_OnAnyUnitMovedGridPosition()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        for (int x = 0; x < GridLevel.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < GridLevel.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }
    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }
    public void UpdateGridVisual()
    {
        HideAllGridPositions();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        ShowGridPositionList(selectedAction.GetValidActionGridPositionList());
    }
    //private void ShowHideGrid(bool notShow) //ben ekledim
    //{
    //    if (notShow) { HideAllGridPositions(); }
    //    else UpdateGridVisual(); 
    //}
}
