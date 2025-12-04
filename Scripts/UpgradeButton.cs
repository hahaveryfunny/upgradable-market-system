using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] StatType statType;

    void Start()
    {
        var item = GetComponentInParent<ItemStats>();
        if (item != null)
        {
            statType = item.statType;
        }

    }

    public void UpgradeClicked()
    {
        GameEvents.OnUpgradeWeapon?.Invoke(statType);
    }
}
