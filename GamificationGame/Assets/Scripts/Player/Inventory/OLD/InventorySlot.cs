using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerUpHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            RecorcedItem recordedItemd = dropped.GetComponent<RecorcedItem>();
            recordedItemd.parentAfterDrag = transform;
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        Debug.Log(name + " Game Object Clicked!");
        string[] splitArray = name.Split(",");

        int x = int.Parse(splitArray[0]);
        int y = int.Parse(splitArray[1]);

        //Debug.Log($"{x}, {y}");



        //transform.parent.GetComponent<InventorySystem>().grid.SetValue(x, y);
        transform.parent.GetComponent<InventorySystem>().SetValue(x, y);
        //transform.parent.GetComponent<InventorySystem>().SetValue(1, 1);
        //transform.parent.GetComponent<InventorySystem>().SetValue(int.Parse(splitArray[0]), int.Parse(splitArray[1]));
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        Debug.Log(name + "No longer being clicked");
        string[] splitArray = name.Split(",");

        int x = int.Parse(splitArray[0]);
        int y = int.Parse(splitArray[1]);

        transform.parent.GetComponent<InventorySystem>().SetValueReset(x, y);
    }

}
