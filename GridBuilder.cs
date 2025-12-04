using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] RectTransform size;

    void OnEnable()
    {
        GameEvents.OnMarketTabClicked += UpdateGrid;
    }
    void OnDisable()
    {
        GameEvents.OnMarketTabClicked -= UpdateGrid;
    }

    void UpdateGrid(WeaponData[] newWeapons)
    {
        ClearGrid();

        ItemButton firstButton = null;

        for (var i = 0; i < newWeapons.Length; i++)
        {
            var buttonGO = Instantiate(buttonPrefab, transform);
            var button = buttonGO.GetComponent<ItemButton>();

            var runtimeWeapon = SaveManager.Instance.GetRuntime(newWeapons[i].weaponID);
            button.SetUpButton(newWeapons[i], runtimeWeapon);

            if (firstButton == null && runtimeWeapon.unlocked)
            {
                firstButton = button;
                button.SelectFirstWhileBuilding();
            }
        }
    }


    void ClearGrid()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        // Reset the selected item when the grid changes
        ItemButton.ResetSelected();
    }
}
