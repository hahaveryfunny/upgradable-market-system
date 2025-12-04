
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum StatType
{
    Accuracy, Capacity, Damage, Range, Reload
}
public class ItemStats : MonoBehaviour
{
    public StatType statType;
    [SerializeField] TextMeshProUGUI statNameText;
    [SerializeField] Image currentFill;
    [SerializeField] Image upgradeFill;
    [SerializeField] Image maxFill;
    [SerializeField] TextMeshProUGUI statValueText;
    [SerializeField] TextMeshProUGUI upgradePriceText;
    WeaponData currentWeaponData;
    [SerializeField] Button upgradeButton;
    [SerializeField] Color32 nonPurchasablePriceColor = new Color32(255, 110, 110, 255);
    Color32 upgradeTextColor = new Color32(255, 217, 117, 255);
    Color32 darkerUpgradeColor = new Color32(46, 184, 140, 255);


    // Start is called before the first frame update
    void Start()
    {
        if (statNameText == null) { Debug.LogWarning("no stat name field found"); return; }
        if (statValueText == null) { Debug.LogWarning("no stat value found"); return; }
        WeaponRuntime weaponRuntime = SaveManager.Instance.GetRuntime(currentWeaponData.weaponID);
        statNameText.text = $"{statType}: {weaponRuntime.GetCurrentValue(statType)}<color=#45CD86> +{currentWeaponData.increment[statType]}</color>";
        //statNameText.text = statType.ToString();

    }

    void OnEnable()
    {
        GameEvents.OnMarketItemClicked += SetUpStat;
        GameEvents.OnUpgradeWeapon += UpgradeWeaponStat;
        GameEvents.OnRefreshStats += SetUpStat;

    }
    void OnDisable()
    {
        GameEvents.OnMarketItemClicked -= SetUpStat;
        GameEvents.OnUpgradeWeapon -= UpgradeWeaponStat;
        GameEvents.OnRefreshStats -= SetUpStat;
    }




    private void SetUpStat(WeaponData weaponData, WeaponRuntime weaponRuntime)
    {
        currentWeaponData = weaponData;
        int price = weaponRuntime.GetPrice(weaponData, statType);
        upgradePriceText.text = price.ToString();
        if (weaponRuntime.MaxedOut(currentWeaponData, statType))
        {
            SetPurchasable(false);
        }
        weaponRuntime.Recalculate(currentWeaponData);
        float currentAmount = weaponRuntime.GetCurrentValue(statType);
        float maxAmount = weaponData.maxValue[statType];

        // Normalize values for UI (0–1)
        float currentFillAmount = Mathf.Clamp01(currentAmount / 100f);
        float upgradeFillAmount = Mathf.Clamp01((currentAmount + weaponData.increment[statType]) / 100f);
        float maxFillAmount = Mathf.Clamp01(maxAmount / 100f);

        // Set the UI fills
        currentFill.fillAmount = currentFillAmount;
        upgradeFill.fillAmount = Mathf.Min(upgradeFillAmount, maxFillAmount);
        maxFill.fillAmount = maxFillAmount;

        statValueText.text = currentAmount + " / " + maxAmount;
        Debug.Log("increment is = " + currentWeaponData.increment[statType]);
        statNameText.text = $"{statType}: {weaponRuntime.GetCurrentValue(statType)}<color=#45CD86> +{currentWeaponData.increment[statType]}</color>";

        SetPurchasable(SaveManager.Instance.SaveData.money >= price);

    }

    void UpgradeWeaponStat(StatType givenStatType)
    {
        var runtimeWeapon = SaveManager.Instance.GetRuntime(currentWeaponData.weaponID);
        if (runtimeWeapon == null) { Debug.LogWarning("no runtime found"); return; }
        if (statType != givenStatType)
        {
            SetUpStat(currentWeaponData, runtimeWeapon);
            return;
        }
        if (currentWeaponData == null) { Debug.LogWarning("current weapon data null"); return; }

        // if (runtimeWeapon.MaxedOut(currentWeaponData, statType))
        // {
        //     // upgradeButton.interactable = false;
        //     // GameEvents.OnMarketNotification?.Invoke("maxed out");
        //     // GameEvents.OnPlaySFX?.Invoke(SFXType.Error);
        //     return;
        // }


        SaveManager.Instance.SaveData.money -= runtimeWeapon.GetPrice(currentWeaponData, statType);
        GameEvents.OnSave?.Invoke();
        GameEvents.OnMoneyChanged?.Invoke(SaveManager.Instance.SaveData.money);
        GameEvents.OnPlaySFX?.Invoke(SFXType.ItemUpgrade);

        runtimeWeapon.IncreaseLevel(givenStatType);
        runtimeWeapon.Recalculate(currentWeaponData);
        GameEvents.OnRefreshStats?.Invoke(currentWeaponData, runtimeWeapon);
    }

    void SetPurchasable(bool purchasable)
    {
        if (purchasable)
        {
            upgradeButton.interactable = true;
            upgradePriceText.color = upgradeTextColor;
        }
        else
        {
            upgradeButton.interactable = false;
            upgradePriceText.color = nonPurchasablePriceColor;
        }
        // cb.normalColor = purchasable ? upgradeTextColor : nonPurchasablePriceColor;
        // upgradeButton.colors = cb;
    }




}
