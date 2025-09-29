using UnityEngine;
using UnityEngine.SceneManagement;

public class Playerhealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 7;   // number of hits player can take
    private int currentHealth;

    private Animator anim;
    private Collider col;

    [Header("HUD Reference")]
    public PlayerHUD playerHUD;   // drag your HUD panel with PlayerHUD script here

    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        col = GetComponent<Collider>();

        if (playerHUD != null)
            playerHUD.SetHealth(currentHealth); // init HUD at start
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
        Destroy(gameObject, 3f);

        // Option 2: Load game over scene immediately
        // SceneManager.LoadSceneAsync("GameOverScene");
    }
}
