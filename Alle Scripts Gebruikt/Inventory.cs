using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class InventoryItem
{
    public string id;
    public string displayName;
    public ItemType type;
    public int amount;
    public int maxStack;
    public Sprite icon;
}

public class Inventory : MonoBehaviour
{
    [Min(1)] public int maxSlots = 12;

    // Simple list-based inventory
    public List<InventoryItem> items = new List<InventoryItem>();

    // Fired whenever inventory content changes
    public event Action OnInventoryChanged;

    /// <summary>
    /// Add a PickupItem (from the world) into the inventory. Returns true on success.
    /// </summary>
    public bool AddItem(PickupItem pickup)
    {
        if (pickup == null) return false;

        // stack if possible
        if (pickup.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it.id == pickup.id && it.amount < it.maxStack)
                {
                    int canTake = Mathf.Min(pickup.amount, it.maxStack - it.amount);
                    it.amount += canTake;
                    pickup.amount -= canTake;

                    if (pickup.amount <= 0)
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
            }
        }

        // add new stacks while we still have amount left
        while (pickup.amount > 0)
        {
            if (items.Count >= maxSlots)
            {
                Debug.Log("Inventory full.");
                OnInventoryChanged?.Invoke();
                return false; // couldn't fit all
            }

            int toMove = pickup.stackable ? Mathf.Min(pickup.amount, pickup.maxStack) : pickup.amount;

            var newItem = new InventoryItem
            {
                id = pickup.id,
                displayName = pickup.displayName,
                type = pickup.type,
                amount = toMove,
                maxStack = Mathf.Max(1, pickup.maxStack),
                icon = pickup.icon
            };

            items.Add(newItem);
            pickup.amount -= toMove;
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    /// <summary>
    /// Convenience overload for name-only adds (keeps compatibility with your earlier call).
    /// Creates a generic non-stackable item.
    /// </summary>
    public bool AddItem(string displayName)
    {
        if (items.Count >= maxSlots) return false;

        items.Add(new InventoryItem
        {
            id = displayName.ToLower().Replace(" ", "_"),
            displayName = displayName,
            type = displayName.ToLower().Contains("medkit") ? ItemType.Medkit :
                   displayName.ToLower().Contains("weapon") ? ItemType.Weapon : ItemType.Other,
            amount = 1,
            maxStack = 1,
            icon = null
        });

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(string idOrName)
    {
        string key = idOrName.ToLower();
        return items.Exists(i => i.id == key || i.displayName.ToLower() == key);
    }

    public bool RemoveItem(string idOrName, int amount = 1)
    {
        string key = idOrName.ToLower();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == key || items[i].displayName.ToLower() == key)
            {
                items[i].amount -= amount;
                if (items[i].amount <= 0) items.RemoveAt(i);
                OnInventoryChanged?.Invoke();
                return true;
            }
        }
        return false;
    }


}

