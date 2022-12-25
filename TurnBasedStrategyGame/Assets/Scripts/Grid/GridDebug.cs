using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshProText;
    private GridObject gridObject;
    public void SetGridObject(GridObject gridObject) => this.gridObject = gridObject;
    private void Update()
    {
        textMeshProText.text = gridObject.ToString();
    }
}
