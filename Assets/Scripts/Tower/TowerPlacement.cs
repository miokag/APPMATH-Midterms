using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacement : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private GameObject sniperTowerPrefab;
    [SerializeField] private GameObject scatterTowerPrefab;
    [SerializeField] private GameObject seekerTowerPrefab;

    private GameObject currentTowerPrefab; // The prefab being placed
    private GameObject currentTowerInstance; // The instance being dragged

    private void Update()
    {
        if (currentTowerInstance != null)
        {
            // Follow the mouse position
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Snap to grid (optional)
            worldPosition.x = Mathf.Round(worldPosition.x);
            worldPosition.y = Mathf.Round(worldPosition.y);

            currentTowerInstance.transform.position = worldPosition;

            // Place the tower on left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }

            // Cancel placement on right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                CancelPlacement();
            }
        }
    }

    // Called when a tower button is clicked in the shop
    public void SelectTower(string towerType)
    {
        if (currentTowerInstance != null)
        {
            Destroy(currentTowerInstance); // Cancel previous placement
        }

        switch (towerType)
        {
            case "SniperTower":
                currentTowerPrefab = sniperTowerPrefab;
                break;
            case "ScatterTower":
                currentTowerPrefab = scatterTowerPrefab;
                break;
            case "SeekerTower":
                currentTowerPrefab = seekerTowerPrefab;
                break;
            default:
                Debug.LogWarning("Unknown tower type: " + towerType);
                return;
        }

        // Instantiate the tower for placement
        currentTowerInstance = Instantiate(currentTowerPrefab);
    }

    // Place the tower at the current position
    private void PlaceTower()
    {
        if (currentTowerInstance != null)
        {
            // Check if the position is valid (e.g., not overlapping with other towers)
            if (IsPositionValid(currentTowerInstance.transform.position))
            {
                // Tower is placed, clear the current instance
                currentTowerInstance = null;
                currentTowerPrefab = null;
            }
            else
            {
                Debug.Log("Invalid position!");
            }
        }
    }

    // Cancel the current placement
    private void CancelPlacement()
    {
        if (currentTowerInstance != null)
        {
            Destroy(currentTowerInstance);
            currentTowerInstance = null;
            currentTowerPrefab = null;
        }
    }

    // Check if the position is valid (e.g., not overlapping with other towers)
    private bool IsPositionValid(Vector3 position)
    {
        // Example: Check if the position is within the play area
        float minX = -10f, maxX = 10f;
        float minY = -5f, maxY = 5f;

        return position.x >= minX && position.x <= maxX &&
               position.y >= minY && position.y <= maxY;
    }
}