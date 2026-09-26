using System;
using UnityEngine;

public class WeightSetManager : MonoBehaviour
{
    [SerializeField] private Canvas weightSetCanvas;
    
    // Internal Variables
    private WeightSetField weightSetField;
    
    private void Awake()
    {
        weightSetField = FindFirstObjectByType<WeightSetField>();
    }

    public void EnableCanvas(bool enable)
    {
        weightSetCanvas.enabled = enable;
        SetWeight();
    }
    
    public void SetWeight()
    {
        int weight = weightSetField.GetSelectedWeight();
        GameManager.selectedWeight = weight;
    }
}
