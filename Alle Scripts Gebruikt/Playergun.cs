using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 40f;

    [Header("Activation")]
    public Camera linkedCamera;        // camera that "owns" this gun
    public bool requireCameraEnabled = true;

    [Header("Shooter (owner)")]
    public GameObject shooterRoot;     // assign your Player root here (or set in Awake)

    void Awake()
    {
        if (shooterRoot == null)
        {
            // Try to find the player root on this gun�s hierarchy
            var player = GetComponentInParent<PlayerTag>(); // tiny marker component (see below)
            shooterRoot = player != null ? player.gameObject : transform.root.gameObject;
        }
    }

    void Update()
    {
        // Only fire if this gun is the active one (its camera is enabled)
        if (requireCameraEnabled && linkedCamera != null && !linkedCamera.enabled) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
    }

    void Fire()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = false; // <- stops the bullet from falling
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }

        if (bullet.TryGetComponent<Bullet>(out var b))
        {
            b.shooter = shooterRoot;   // set this to your player root (not just the gun)
            b.targetTag = "Enemy";     // player bullets hurt enemies
        }

        Destroy(bullet, 5f);
    }

}
