using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TetrisInventorySystem inventoryController;
    public ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(TetrisInventorySystem)) as TetrisInventorySystem;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;
        
    }
}

