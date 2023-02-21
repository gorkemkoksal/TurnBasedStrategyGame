using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebug : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshProText;
    private object gridObject;
    public virtual void SetGridObject(object gridObject) => this.gridObject = gridObject;
    protected virtual void Update()
    {
        textMeshProText.text = gridObject.ToString();
    }
}
