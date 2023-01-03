using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList = new List<ActionButtonUI>(); //awake te uyandirmak daha mi iyi


    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += CreateUnitActionButtons;
        UnitActionSystem.Instance.OnSelectedUnitChange += UpdateSelectedVisual;
        UnitActionSystem.Instance.OnSelectedActionChange += UpdateSelectedVisual;
        UnitActionSystem.Instance.OnBusyChanged += HideShowButtonsForBusyState;
        
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }
    private void CreateUnitActionButtons()
    {
        foreach (Transform oldButtonTransform in actionButtonContainerTransform)
        {
            Destroy(oldButtonTransform.gameObject);
        }
        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }
    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
    private void HideShowButtonsForBusyState(bool isBusy)
    {
        if (isBusy) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
}
