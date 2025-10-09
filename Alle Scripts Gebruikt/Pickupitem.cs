using UnityEngine;


public enum ItemType { Medkit, Weapon, Ammo,Hostage, Other }

public class PickupItem : MonoBehaviour
{
    [Header("Identity")]
    public string id = "medkit_small";           // unique-ish id used for stacking
    public string displayName = "Medkit";        // what shows in UI
    public ItemType type = ItemType.Medkit;

    [Header("Stacking")]
    public bool stackable = true;
    [Min(1)] public int amount = 1;              // how many (for ammo, etc.)
    [Min(1)] public int maxStack = 5;

    [Header("UI")]
    public Sprite icon;                          // optional: for UI

    // Optional helper to quickly mark an object as an "Item"
    private void Reset()
    {
        gameObject.tag = "Item";
    }
}

