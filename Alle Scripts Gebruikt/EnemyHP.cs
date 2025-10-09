using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 50;
    public int Current { get; private set; }

    [Header("Death")]
    [SerializeField] private bool destroyOnDeath = true;
    [SerializeField] private float destroyDelay = 2f;

    [Header("Optional")]
    [SerializeField] private Renderer[] flashRenderers; // assign if you want a quick hit flash
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashTime = 0.07f;

    public System.Action<int, int> OnHealthChanged; // (current, max)
    public System.Action OnDied;

    void Awake()
    {
        Current = Mathf.Max(1, maxHealth);
        OnHealthChanged?.Invoke(Current, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (Current <= 0 || amount <= 0) return;

        Current = Mathf.Max(0, Current - amount);
        OnHealthChanged?.Invoke(Current, maxHealth);

        // optional quick feedback
        if (flashRenderers != null && flashRenderers.Length > 0)
            StartCoroutine(HitFlash());

        Debug.Log($"{name} took {amount} damage → {Current}/{maxHealth}");

        if (Current == 0)
            Die();
    }

    void Die()
    {
        if (Current != 0) return;

        // Disable common components safely if they exist
        var col = GetComponent<Collider>();
        if (col) col.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        var ai = GetComponent<MonoBehaviour>(); // placeholder if you have an AI script; disable below if you know its name
        // Example: var nav = GetComponent<UnityEngine.AI.NavMeshAgent>(); if (nav) nav.enabled = false;

        OnDied?.Invoke();
        Debug.Log($"{name} died.");

        if (destroyOnDeath)
            Destroy(gameObject, destroyDelay);
        else
            gameObject.SetActive(false);
    }

    System.Collections.IEnumerator HitFlash()
    {
        var originals = new Color[flashRenderers.Length];
        for (int i = 0; i < flashRenderers.Length; i++)
        {
            if (flashRenderers[i] && flashRenderers[i].material.HasProperty("_Color"))
            {
                originals[i] = flashRenderers[i].material.color;
                flashRenderers[i].material.color = flashColor;
            }
        }
        yield return new WaitForSeconds(flashTime);
        for (int i = 0; i < flashRenderers.Length; i++)
        {
            if (flashRenderers[i] && flashRenderers[i].material.HasProperty("_Color"))
                flashRenderers[i].material.color = originals[i];
        }
    }
}
