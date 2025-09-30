using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemPicker : MonoBehaviour
{
    public float maxDistance = 8f;
    public float pickupRadius = 0.25f;
    public Camera cam;
    public Inventory inventory;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!inventory) inventory = GetComponentInParent<Inventory>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // right mouse button
        {
            if (!cam || !inventory) return;

            // center-screen cast (works with locked cursor)
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.SphereCast(ray, pickupRadius, out RaycastHit hit, maxDistance, ~0, QueryTriggerInteraction.Ignore))
            {
                // climb parents until we find an object tagged "Item"
                Transform t = hit.transform;
                while (t != null && !t.CompareTag("Item")) t = t.parent;
                if (t == null) return;

                // determine if it's the medkit (use name or a small PickupItem component with an id)
                string lowerName = t.name.ToLower();
                bool isMedkit = lowerName.Contains("firstaid") || lowerName.Contains("medkit");

                // add to inventory
                inventory.AddItem(t.name);

                // show the UI icon tagged InventoryItem
                if (isMedkit)
                {
                    GameObject ui = FindInactiveByTag("InventoryItem");
                    if (ui != null) ui.SetActive(true);
                    else Debug.LogWarning("No object with tag 'InventoryItem' found in scene (even inactive).");
                }

                // remove the world item
                Destroy(t.gameObject);
            }
        }
    }

    /// <summary>
    /// Finds the first GameObject in the active scene with the given tag,
    /// including inactive objects (which GameObject.FindWithTag cannot).
    /// </summary>
    private GameObject FindInactiveByTag(string tag)
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            var trs = roots[i].GetComponentsInChildren<Transform>(true); // include inactive
            foreach (var tr in trs)
            {
                if (tr.CompareTag(tag))
                    return tr.gameObject;
            }
        }
        return null;
    }
}
