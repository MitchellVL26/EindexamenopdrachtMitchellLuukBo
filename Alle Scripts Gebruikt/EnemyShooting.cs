using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;   // enemy bullet prefab (with Rigidbody)
    public Transform firePoint;       // muzzle transform
    public float fireRate = 1f;       // shots per second
    public float bulletSpeed = 20f;
    public float maxRange = 100f;     // only shoot if within range

    [Header("Targeting")]
    public Transform player;          // assign in Inspector (optional; auto-find if null)
    public bool aimIn3D = true;       // true = pitch/yaw, false = yaw only

    Coroutine shootLoop;

    void OnEnable()
    {
        shootLoop = StartCoroutine(ShootForever());
    }

    void OnDisable()
    {
        if (shootLoop != null) StopCoroutine(shootLoop);
    }

    IEnumerator ShootForever()
    {
        var wait = new WaitForSeconds(1f / Mathf.Max(0.01f, fireRate));

        while (true)
        {
            if (player == null)
            {
                var p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }

            if (player != null)
            {
                AimAtPlayer();

                if ((player.position - transform.position).sqrMagnitude <= maxRange * maxRange)
                {
                    FireOnce();
                }
            }

            yield return wait; // fixed cadence
        }
    }

    void AimAtPlayer()
    {
        if (player == null) return;

        Vector3 origin = firePoint != null ? firePoint.position : transform.position;
        Vector3 dir = (player.position - origin).normalized;

        if (!aimIn3D) dir = new Vector3(dir.x, 0f, dir.z).normalized;

        if (firePoint != null)
            firePoint.rotation = Quaternion.LookRotation(dir);
        else
            transform.rotation = Quaternion.LookRotation(aimIn3D ? dir : new Vector3(dir.x, 0f, dir.z));
    }

    void FireOnce()
    {
        if (bulletPrefab == null) return;

        Transform spawn = firePoint != null ? firePoint : transform;
        GameObject bullet = Instantiate(bulletPrefab, spawn.position, spawn.rotation);

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = false; // keep enemy bullets flying straight too
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.linearVelocity = spawn.forward * bulletSpeed;
        }

        if (bullet.TryGetComponent<Bullet>(out var b))
        {
            b.shooter = gameObject;   // this enemy is the shooter
            b.targetTag = "Player";   // enemy bullets hurt the player
        }

        Destroy(bullet, 5f);
    }

}
