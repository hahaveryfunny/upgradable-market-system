using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] Image weaponIcon;
    [SerializeField] WeaponData weaponData;
    [SerializeField] Image selectionOutline;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] Image lockedIcon;

    static ItemButton selectedItem;
    WeaponRuntime runtimeWeapon;
    void OnEnable()
    {
        if (selectionOutline != null)
        {
            selectionOutline.enabled = false;
        }
        GameEvents.OnItemUnlocked += RefreshVisuals;
    }

    void OnDisable()
    {
        GameEvents.OnItemUnlocked -= RefreshVisuals;
    }

    public void SelectFirstWhileBuilding()
    {
        if (selectedItem != null && selectedItem != this)
        {
            selectedItem.Deselect();
        }
        Select();
        selectedItem = this;
        GameEvents.OnMarketItemClicked?.Invoke(weaponData, runtimeWeapon);
    }

    public void RefreshVisuals(WeaponData givenData)
    {
        Debug.Log("0");
        if (givenData.weaponID != weaponData.weaponID) return;
        Debug.Log("1");
        var updatedRuntime = SaveManager.Instance.GetRuntime(givenData.weaponID);
        if (updatedRuntime == null) { Debug.Log("no updated runtime found "); return; }
        Debug.Log("2");
        if (updatedRuntime.unlocked)
        {
            Debug.Log("3");
            Debug.Log("weapon is unlocked = " + givenData.weaponID);
            if (selectedItem != null)
            {
                selectedItem.Deselect();
            }
            Select();
            selectedItem = this;
            Debug.Log("weapon icon = " + weaponData.icon);
            weaponIcon.sprite = weaponData.icon;
            IsIconLocked(false);
            priceText.text = "";
        }
        else
        {
            IsIconLocked(true);
            priceText.text = weaponData.purchasePrice.ToString();
        }
    }

    void IsIconLocked(bool locked)
    {
        lockedIcon.enabled = locked;
        weaponIcon.enabled = !locked;
    }


    public void SetUpButton(WeaponData givenData, WeaponRuntime givenRuntimeWeapon)
    {
        if (givenData == null) { Debug.Log("no data"); return; }
        weaponData = givenData;
        runtimeWeapon = givenRuntimeWeapon;
        if (runtimeWeapon == null) { Debug.LogWarning($"RuntimeWeapon missing for {givenData.weaponID}"); return; }
        if (weaponIcon == null) Debug.LogWarning("no wepaonIcon found");
        if (priceText == null) Debug.LogWarning("no priceText found");

        if (runtimeWeapon.unlocked)
        {
            weaponIcon.sprite = givenData.icon;
            IsIconLocked(false);
            priceText.text = "";
        }
        else
        {
            IsIconLocked(true);
            priceText.text = givenData.purchasePrice.ToString();
        }
        //background color falan da yapilabilir
    }
    public void ItemClicked()
    {
        if (weaponData == null) { Debug.LogWarning("no weapon found"); return; }
        if (runtimeWeapon == null) { Debug.LogWarning("no runtime data found"); return; }

        if (!runtimeWeapon.unlocked)
        {
            if (SaveManager.Instance.SaveData.money >= weaponData.purchasePrice)
            {
                runtimeWeapon.unlocked = true;
                SaveManager.Instance.SaveData.money -= weaponData.purchasePrice;
                GameEvents.OnSave?.Invoke();
                GameEvents.OnItemUnlocked?.Invoke(weaponData);
                GameEvents.OnPlaySFX?.Invoke(SFXType.ItemBuy);
                GameEvents.OnMoneyChanged?.Invoke(SaveManager.Instance.SaveData.money);
                GameEvents.OnMarketItemClicked?.Invoke(weaponData, runtimeWeapon);
            }
            else
            {
                GameEvents.OnMarketNotification?.Invoke("Insufficient Funds");
                GameEvents.OnPlaySFX?.Invoke(SFXType.Error);
            }
        }
        else
        {
            if (selectedItem != null && selectedItem != this)
            {
                selectedItem.Deselect();
            }
            Select();
            selectedItem = this;
            GameEvents.OnMarketItemClicked?.Invoke(weaponData, runtimeWeapon);
            GameEvents.OnPlaySFX?.Invoke(SFXType.UIClick);
        }
    }


    void Select()
    {
        selectionOutline.enabled = true;
    }

    void Deselect()
    {
        selectionOutline.enabled = false;
    }

    public static void ResetSelected()
    {
        selectedItem = null;
    }
}
