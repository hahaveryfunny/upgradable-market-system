using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightedItem : MonoBehaviour
{
    [SerializeField] Image highlightedImage;

    void ChangeImage(WeaponData weaponData,WeaponRuntime runtimeWeapon)
    {
        if (highlightedImage == null) { Debug.LogWarning("no image found"); return; }
        highlightedImage.sprite = weaponData.icon;
    }


    void OnEnable()
    {
        GameEvents.OnMarketItemClicked += ChangeImage;
    }

    void OnDisable()
    {
        GameEvents.OnMarketItemClicked += ChangeImage;
    }
}
