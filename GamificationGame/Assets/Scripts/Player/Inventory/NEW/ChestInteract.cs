using UnityEngine.EventSystems;
using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public TetrisInventorySystem inventoryController;
    public ChestGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(TetrisInventorySystem)) as TetrisInventorySystem;
        //itemGrid = GetComponent<ItemGrid>();
    }
     
    public void OnPointerEnter(PointerEventData eventData)
    {
        //inventoryController.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.SelectedItemGrid = null;

    }
}
