using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += OnChange;
        UpdateVisual();
    }
    private void OnChange() => UpdateVisual();
    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
            meshRenderer.enabled = false;
    }
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange -= OnChange;
    }
}
