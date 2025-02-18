using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject towerPrefab; // Assign this in Inspector
    private GameObject draggedTowerInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instantiate a new tower when dragging starts
        draggedTowerInstance = Instantiate(towerPrefab);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedTowerInstance != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // Distance from camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            
            // Optional grid snap
            worldPosition.x = Mathf.Round(worldPosition.x);
            worldPosition.y = Mathf.Round(worldPosition.y);
            
            draggedTowerInstance.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedTowerInstance != null)
        {
            if (IsValidPlacement(draggedTowerInstance.transform.position))
            {
                // Successfully placed
                draggedTowerInstance = null;
            }
            else
            {
                // Invalid placement → destroy the object
                Destroy(draggedTowerInstance);
            }
        }
    }

    private bool IsValidPlacement(Vector3 position)
    {
        // Example: Prevent placement outside (-10,10) and (-5,5) boundaries
        float minX = -10f, maxX = 10f;
        float minY = -5f, maxY = 5f;

        return position.x >= minX && position.x <= maxX &&
               position.y >= minY && position.y <= maxY;
    }
}