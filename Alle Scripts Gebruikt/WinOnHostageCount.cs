using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOnHostageCount : MonoBehaviour
{
    [Header("Refs")]
    public Inventory inventory;

    [Header("Win Settings")]
    [Tooltip("Win when hostage count is >= this number.")]
    public int requiredHostages = 5;

    [Tooltip("Load a scene on win. Leave empty to just log or show UI.")]
    public string sceneToLoadOnWin = ""; // e.g. "WinScene"
    public float loadDelay = 1.5f;

    [Header("Optional UI")]
    public GameObject winPanel; // assign a UI panel to show on win (inactive by default)

    bool hasWon = false;

    void Awake()
    {
        if (!inventory) inventory = FindObjectOfType<Inventory>();
    }

    void OnEnable()
    {
        if (inventory) inventory.OnInventoryChanged += CheckWin;
        // also check at start in case you already start with some hostages
        CheckWin();
    }

    void OnDisable()
    {
        if (inventory) inventory.OnInventoryChanged -= CheckWin;
    }

    void CheckWin()
    {
        if (hasWon || inventory == null) return;

        int hostageCount = 0;
        foreach (var it in inventory.items)
        {
            if (it.type == ItemType.Hostage)
                hostageCount += it.amount;
        }

        // Win if you have 5 or more (covers both "5 or 6")
        if (hostageCount >= requiredHostages)
        {
            hasWon = true;
            Debug.Log($"YOU WIN! Hostages collected: {hostageCount}");

            if (winPanel) winPanel.SetActive(true);

            if (!string.IsNullOrEmpty(sceneToLoadOnWin))
                Invoke(nameof(LoadWinScene), loadDelay);
            // else: you can add any other win logic here (freeze player, play SFX, etc.)
        }
    }

    void LoadWinScene()
    {
        SceneManager.LoadScene(sceneToLoadOnWin);
    }
}
