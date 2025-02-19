using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject towerPrefab; 
    private GameObject draggedTowerInstance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedTowerInstance = Instantiate(towerPrefab);
        ApplyUpgradesToNewTower(draggedTowerInstance);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedTowerInstance != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Optional grid snap
            worldPosition.x = Mathf.Round(worldPosition.x);
            worldPosition.y = Mathf.Round(worldPosition.y);

            draggedTowerInstance.transform.position = worldPosition;
            DisableTowerBehavior(draggedTowerInstance);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedTowerInstance != null)
        {
            if (IsValidPlacement(draggedTowerInstance.transform.position))
            {
                EnableTowerBehavior(draggedTowerInstance);

                draggedTowerInstance = null;
            }
            else
            {
                Destroy(draggedTowerInstance);
            }
        }
    }

    private bool IsValidPlacement(Vector3 position)
    {
        float minX = -10f, maxX = 10f;
        float minY = -5f, maxY = 5f;

        return position.x >= minX && position.x <= maxX &&
               position.y >= minY && position.y <= maxY;
    }

    private void ApplyUpgradesToNewTower(GameObject newTower)
    {
        ShopBehavior shop = FindObjectOfType<ShopBehavior>();

        if (shop == null) return;

        if (newTower.TryGetComponent<SeekerTowerBehavior>(out var seekerTower))
        {
            seekerTower.ApplyUpgrades(shop.GetSpeedLevel(), shop.GetRangeLevel());
        }
        else if (newTower.TryGetComponent<SniperTowerBehavior>(out var sniperTower))
        {
            sniperTower.ApplyUpgrades(shop.GetSpeedLevel(), shop.GetRangeLevel());
        }
        if (newTower.TryGetComponent<ScatterTowerBehavior>(out var scatterTower))
        {
            scatterTower.ApplyUpgrades(shop.GetSpeedLevel(), shop.GetRangeLevel());
        }
    }

    private void EnableTowerBehavior(GameObject tower)
    {
        if (tower.TryGetComponent<ScatterTowerBehavior>(out var scatterTower))
        {
            scatterTower.enabled = true;
        }
        else if (tower.TryGetComponent<SniperTowerBehavior>(out var sniperTower))
        {
            sniperTower.enabled = true;
        }
        else if (tower.TryGetComponent<SeekerTowerBehavior>(out var seekerTower))
        {
            seekerTower.enabled = true;
        }
    }
    
    private void DisableTowerBehavior(GameObject tower)
    {
        if (tower.TryGetComponent<ScatterTowerBehavior>(out var scatterTower))
        {
            scatterTower.enabled = false;
        }
        else if (tower.TryGetComponent<SniperTowerBehavior>(out var sniperTower))
        {
            sniperTower.enabled = false;
        }
        else if (tower.TryGetComponent<SeekerTowerBehavior>(out var seekerTower))
        {
            seekerTower.enabled = false;
        }
    }
}
