using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLevel : MonoBehaviour
{
    public static GridLevel Instance { get; private set; }
    public event Action OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }
    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnitList();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }
    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke();
    }
    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetHeight() => gridSystem.GetHeight();
    public int GetWidth() => gridSystem.GetWidth();
    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }
    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetInteractable();
    }
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.SetInteractable(interactable);
    }
}
