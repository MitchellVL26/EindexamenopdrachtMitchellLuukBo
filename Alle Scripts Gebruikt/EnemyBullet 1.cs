using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;

    [Header("Ownership / Filtering")]
    public GameObject shooter;          // set by spawner (player root or enemy root)
    public string targetTag = "Player";       // optional; used only for *extra* filtering / logging

    private void OnTriggerEnter(Collider other)
    {
        // 1) Ignore our own shooter (including any child colliders of the shooter)
        if (shooter != null)
        {
            var otherRoot = other.transform.root.gameObject;
            if (otherRoot == shooter) return;
        }

        // 2) Try to find health on what we hit (or its parents)
        var enemyHP = other.GetComponentInParent<EnemyHP>();
        var playerHP = other.GetComponentInParent<Playerhealth1>();

        bool applied = false;

        if (enemyHP != null)
        {
            enemyHP.TakeDamage(damage);
            Debug.Log($"[Bullet] Damaged ENEMY '{enemyHP.name}' for {damage}. Collider: {other.name}");
            applied = true;
        }
        else if (playerHP != null)
        {
            playerHP.TakeDamage(damage);
            Debug.Log($"[Bullet] Damaged PLAYER '{playerHP.name}' for {damage}. Collider: {other.name}");
            applied = true;
        }
        else
        {
            // No health found — environment or something else
            Debug.Log($"[Bullet] Hit '{other.name}' (tag '{other.tag}') but no EnemyHP/Playerhealth found.");
        }

        // 3) Despawn on any solid hit (don’t despawn if we literally hit another trigger volume)
        if (!other.isTrigger || applied)
            Destroy(gameObject);
    }
}
