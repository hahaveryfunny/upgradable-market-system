using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTabButton : MonoBehaviour
{
    RectTransform rectTransform;
    float midPosOfTab;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        midPosOfTab = (rectTransform.anchorMin.x + rectTransform.anchorMax.x) / 2f;
    }

    public void EquipmentTabClicked()
    {
        GameEvents.OnEquipmentButtonClicked?.Invoke(midPosOfTab);
    }
}
