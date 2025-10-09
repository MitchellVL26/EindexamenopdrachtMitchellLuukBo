using UnityEngine;

public class MedkitUse : MonoBehaviour
{
    public Inventory inventory;
    public Playerhealth1 health;

    [Header("Heal Settings")]
    public int healAmount = 25; // how much each medkit heals

    void Awake()
    {
        if (!inventory) inventory = GetComponent<Inventory>();
        if (!health) health = GetComponent<Playerhealth1>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!inventory || !health) return;

            // check if inventory has a medkit
            if (inventory.HasItem("medkit"))
            {
                // heal
                health.Heal(healAmount);

                // remove one medkit
                inventory.RemoveItem("medkit", 1);
                Debug.Log("[MedkitUse] Used one medkit.");
            }
            else
            {
                Debug.Log("[MedkitUse] No medkit in inventory!");
            }
        }
    }
}
