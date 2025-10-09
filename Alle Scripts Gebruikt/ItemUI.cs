using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPicker : MonoBehaviour
{
    [Header("Pickup")]
    public float maxDistance = 8f;
    public float pickupRadius = 0.25f;
    public LayerMask pickupMask = ~0;

    [Header("Cameras")]
    public Camera fpsCam;
    public Camera tpsCam;

    [Header("Refs")]
    public Inventory inventory;

    [Header("UI (Optional)")]
    public GameObject medkitUIIcon;

    void Awake()
    {
        if (!inventory) inventory = GetComponentInParent<Inventory>();
        if (!medkitUIIcon) medkitUIIcon = FindInactiveByTag("InventoryItem");
    }

    // Helper to pick whichever camera is active
    Camera ActiveCam =>
        (fpsCam && fpsCam.isActiveAndEnabled) ? fpsCam :
        (tpsCam && tpsCam.isActiveAndEnabled) ? tpsCam : null;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse, swap to E if you prefer
        {
            var cam = ActiveCam;
            if (!cam || !inventory) return;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.SphereCast(ray, pickupRadius, out RaycastHit hit, maxDistance, pickupMask, QueryTriggerInteraction.Ignore))
            {
                Transform t = hit.transform;
                PickupItem pickup = null;

                // climb parents until we find a PickupItem
                while (t != null)
                {
                    pickup = t.GetComponent<PickupItem>();
                    if (pickup != null || t.CompareTag("Item")) break;
                    t = t.parent;
                }
                if (t == null) return;

                // no PickupItem? fallback to just name
                if (pickup == null)
                {
                    bool isMedkitByName = t.name.ToLower().Contains("medkit") || t.name.ToLower().Contains("firstaid");
                    inventory.AddItem(t.name);
                    if (isMedkitByName && medkitUIIcon) medkitUIIcon.SetActive(true);
                    Destroy(t.gameObject);
                    return;
                }

                // use structured pickup info
                bool added = inventory.AddItem(pickup);
                if (added)
                {
                    if (pickup.type == ItemType.Medkit && medkitUIIcon) medkitUIIcon.SetActive(true);
                    Destroy(pickup.gameObject);
                }
                else
                {
                    Debug.Log("Inventory full.");
                }
            }
        }
    }

    private GameObject FindInactiveByTag(string tag)
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var root in roots)
        {
            var trs = root.GetComponentsInChildren<Transform>(true);
            foreach (var tr in trs)
                if (tr.CompareTag(tag)) return tr.gameObject;
        }
        return null;
    }
}
