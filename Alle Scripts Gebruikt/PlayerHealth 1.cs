using UnityEngine;
using UnityEngine.SceneManagement;

public class Playerhealth1 : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 7;   // number of hits player can take
    private int currentHealth;

    private Animator anim;
    private Collider col;

    [Header("HUD Reference")]
    public PlayerHUD playerHUD;   // drag your HUD panel with PlayerHUD script here

    [Header("Inventory Reference")]
    public Inventory inventory;   // drag your Inventory script here

    [Header("Medkit Settings")]
    public int healAmount = 3;    // how much health one medkit restores
    public KeyCode useMedkitKey = KeyCode.F;

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth); // init HUD at start
    }

    void Update()
    {
        // Press F to use a medkit
        if (Input.GetKeyDown(useMedkitKey))
        {
            TryUseMedkit();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player hit! Health left: " + currentHealth);

        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth); // update HUD

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        int before = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        Debug.Log($"Player healed {currentHealth - before}. Health: {currentHealth}/{maxHealth}");

        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth); // update HUD
    }

    void TryUseMedkit()
    {
        if (!inventory)
        {
            Debug.LogWarning("No Inventory assigned to Playerhealth1!");
            return;
        }

        if (currentHealth >= maxHealth)
        {
            Debug.Log("Already at full health. Can't use medkit.");
            return;
        }

        if (inventory.HasItem("medkit"))
        {
            Heal(healAmount);
            inventory.RemoveItem("medkit", 1);
            Debug.Log("Used a medkit.");
        }
        else
        {
            Debug.Log("No medkit in inventory!");
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");

        if (anim != null)
        {
            anim.Play("Death"); // state must match your Animator
        }

        if (col != null)
            col.enabled = false; // stop further hits

        // Disable HUD or flash "Game Over"
        if (playerHUD != null)
            playerHUD.SetHealth(0);

        // Option 1: Destroy player after 3 seconds
        Destroy(gameObject, 0.5f);
        SceneManager.LoadScene("End Screen");
        // Option 2: Load game over scene immediately
        // SceneManager.LoadSceneAsync("GameOverScene");
    }

}
