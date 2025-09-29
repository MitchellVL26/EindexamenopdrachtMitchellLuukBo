using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image healthBarFill;

    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxAmmo = 90;

    private int currentHealth;
    private int currentAmmo;
    private int currentScore;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        currentScore = 0;

        UpdateHUD();
    }

    // 🔹 Health
    public void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);
        UpdateHUD();
    }

    public void AddHealth(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    // 🔹 Ammo
    public void SetAmmo(int value)
    {
        currentAmmo = Mathf.Clamp(value, 0, maxAmmo);
        UpdateHUD();
    }

    public void UseAmmo(int amount)
    {
        SetAmmo(currentAmmo - amount);
    }

    // 🔹 Score
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (healthText != null)
            healthText.text = $"Health: {currentHealth}";

        if (ammoText != null)
            ammoText.text = $"Ammo: {currentAmmo}/{maxAmmo}";

        if (scoreText != null)
            scoreText.text = $"Score: {currentScore}";

        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
    }
}
