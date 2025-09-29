using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 100;
    public int currentHealth;

    public int maxAmmo = 30;
    public int currentAmmo;

    [Header("References")]
    public PlayerHUD playerHUD;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;

        if (playerHUD != null)
        {
            playerHUD.SetHealth(currentHealth);
            playerHUD.SetAmmo(currentAmmo);
        }
    }

    // 🔹 Example: player takes damage
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth);
    }

    // 🔹 Example: player heals
    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth);
    }

    // 🔹 Example: shooting
    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            if (playerHUD != null)
                playerHUD.SetAmmo(currentAmmo);

            Debug.Log("Bang!");
        }
        else
        {
            Debug.Log("Out of ammo!");
        }
    }

    // 🔹 Example: ammo pickup
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        if (playerHUD != null)
            playerHUD.SetAmmo(currentAmmo);
    }
}
