using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class MinimapController : MonoBehaviour
{
    [Header("UI refs")]
    public RectTransform minimapPanel;      // Het panel zelf (kan leeg blijven -> neemt dit component)
    public RectTransform playerIconPrefab;  // prefab (UI Image, RectTransform)
    public RectTransform enemyIconPrefab;   // prefab (UI Image, RectTransform)

    [Header("World mapping")]
    public Transform player;                // optioneel: sleep player of laat het zoeken via tag
    public string enemyTag = "Enemy";
    public Vector2 worldSize = new Vector2(50f, 50f); // wereldbreedte (x) en -diepte (z)
    public Vector3 worldCenter = Vector3.zero; // midden van het weergegeven gebied

    private RectTransform playerIcon;
    private List<RectTransform> enemyIcons = new List<RectTransform>();
    private List<Transform> enemies = new List<Transform>();

    void Awake()
    {
        if (minimapPanel == null) minimapPanel = GetComponent<RectTransform>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (playerIconPrefab != null && minimapPanel != null)
        {
            playerIcon = Instantiate(playerIconPrefab, minimapPanel);
            playerIcon.name = "PlayerIcon_Instance";
            // zorg dat anchors/pivot in midden staan
            playerIcon.anchorMin = playerIcon.anchorMax = new Vector2(0.5f, 0.5f);
            playerIcon.pivot = new Vector2(0.5f, 0.5f);
        }

        RefreshEnemies();
    }

    void Update()
    {
        if (player != null && playerIcon != null)
        {
            playerIcon.anchoredPosition = WorldToMinimap(player.position);
        }

        // update enemy icons (loop backwards voor veilige verwijdering)
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                if (i < enemyIcons.Count && enemyIcons[i] != null)
                    Destroy(enemyIcons[i].gameObject);

                if (i < enemyIcons.Count) enemyIcons.RemoveAt(i);
                enemies.RemoveAt(i);
                continue;
            }

            if (i < enemyIcons.Count && enemyIcons[i] != null)
                enemyIcons[i].anchoredPosition = WorldToMinimap(enemies[i].position);
        }

        if (Input.GetKeyDown(KeyCode.R)) RefreshEnemies();
    }

    public void RefreshEnemies()
    {
        // clear oude iconen
        foreach (var ic in enemyIcons) if (ic != null) Destroy(ic.gameObject);
        enemyIcons.Clear();
        enemies.Clear();

        GameObject[] found = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject g in found)
        {
            enemies.Add(g.transform);
            if (enemyIconPrefab != null && minimapPanel != null)
            {
                RectTransform rt = Instantiate(enemyIconPrefab, minimapPanel);
                rt.name = "EnemyIcon_" + g.name;
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                enemyIcons.Add(rt);
            }
        }
    }

    Vector2 WorldToMinimap(Vector3 worldPos)
    {
        float halfX = worldSize.x * 0.5f;
        float halfY = worldSize.y * 0.5f;

        float nx = (worldPos.x - worldCenter.x) / halfX; // -1 .. 1
        float ny = (worldPos.z - worldCenter.z) / halfY; // -1 .. 1

        nx = Mathf.Clamp(nx, -1f, 1f);
        ny = Mathf.Clamp(ny, -1f, 1f);

        float px = nx * (minimapPanel.rect.width * 0.5f);
        float py = ny * (minimapPanel.rect.height * 0.5f);

        return new Vector2(px, py);
    }
}
