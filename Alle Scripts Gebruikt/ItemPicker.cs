using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    [Header("Icons (optional)")]
    public GameObject medkitIcon;
    public GameObject weaponIcon;

    void OnEnable()
    {
        if (!inventory) inventory = Object.FindFirstObjectByType<Inventory>();
        if (inventory != null) inventory.OnInventoryChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        if (inventory != null) inventory.OnInventoryChanged -= Refresh;
    }

    void Refresh()
    {
        if (!inventory) return;

        if (medkitIcon)
        {
            bool hasMedkit = inventory.items.Exists(i => i.type == ItemType.Medkit);
            medkitIcon.SetActive(hasMedkit);
        }

        if (weaponIcon)
        {
            bool hasWeapon = inventory.items.Exists(i => i.type == ItemType.Weapon);
            weaponIcon.SetActive(hasWeapon);
        }
    }
}

