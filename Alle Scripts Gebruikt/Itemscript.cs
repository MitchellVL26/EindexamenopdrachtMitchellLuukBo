using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [Header("Pickup")]
    public float maxDistance = 8f;
    public float pickupRadius = 0.25f;
    public LayerMask pickupMask = ~0;     // everything
    public bool useRaycastOnly = false;   // set true if spherecast seems flaky

    [Header("Cameras (assign both)")]
    public Camera fpsCam;
    public Camera tpsCam;

    [Header("Refs")]
    public Inventory inventory;

    [Header("Player Visuals")]
    [Tooltip("Optional: a medkit model on your player’s back, disabled at start.")]
    public GameObject medkitOnBack;

    Camera ActiveCam =>
        (fpsCam && fpsCam.isActiveAndEnabled) ? fpsCam :
        (tpsCam && tpsCam.isActiveAndEnabled) ? tpsCam : null;

    void Awake()
    {
        if (!inventory) inventory = GetComponentInParent<Inventory>();
        if (medkitOnBack) medkitOnBack.SetActive(false); // hidden at start
        RefreshVisual(); // ADDED: sync once on start in case inventory already has items
    }

    // ADDED: subscribe/unsubscribe to inventory changes
    void OnEnable()
    {
        if (inventory) inventory.OnInventoryChanged += RefreshVisual;
    }
    void OnDisable()
    {
        if (inventory) inventory.OnInventoryChanged -= RefreshVisual;
    }

    // ADDED: central place to show/hide medkit on back
    void RefreshVisual()
    {
        if (!medkitOnBack) return;
        bool hasMedkit = inventory && inventory.HasItem("medkit");
        medkitOnBack.SetActive(hasMedkit);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            var cam = ActiveCam;
            if (!cam) { Debug.LogWarning("[PickUp] No active camera!"); return; }
            if (!inventory) { Debug.LogWarning("[PickUp] No Inventory reference!"); return; }

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            bool hitSomething;

            RaycastHit hit;
            if (useRaycastOnly)
                hitSomething = Physics.Raycast(ray, out hit, maxDistance, pickupMask, QueryTriggerInteraction.Collide);
            else
                hitSomething = Physics.SphereCast(ray, pickupRadius, out hit, maxDistance, pickupMask, QueryTriggerInteraction.Collide);

            if (!hitSomething)
            {
                Debug.Log("[PickUp] No hit.");
                return;
            }

            Debug.Log($"[PickUp] Hit {hit.collider.name}");

            // climb parents to find PickupItem
            Transform t = hit.transform;
            PickupItem pickup = null;
            while (t != null)
            {
                pickup = t.GetComponent<PickupItem>();
                if (pickup != null) break;
                t = t.parent;
            }

            if (pickup != null)
            {
                bool added = inventory.AddItem(pickup);
                Debug.Log($"[PickUp] PickupItem: {pickup.displayName} -> added={added}");
                if (added)
                {
                    // keep immediate feedback, but RefreshVisual will also handle this
                    if (pickup.type == ItemType.Medkit && medkitOnBack)
                        medkitOnBack.SetActive(true);

                    Destroy(pickup.gameObject);
                    RefreshVisual(); // ADDED: ensure visuals reflect new inventory
                }
                return;
            }

            // fallback: tag "Item"
            t = hit.transform;
            while (t != null && !t.CompareTag("Item")) t = t.parent;

            if (t == null)
            {
                Debug.Log("[PickUp] No PickupItem and no 'Item' tag.");
                return;
            }

            inventory.AddItem(t.name);
            Debug.Log($"[PickUp] Fallback pickup: {t.name}");

            // name check for medkit
            if ((t.name.ToLower().Contains("medkit") || t.name.ToLower().Contains("firstaid")) && medkitOnBack)
                medkitOnBack.SetActive(true);

            Destroy(t.gameObject);
            RefreshVisual(); // ADDED
        }
    }
}
