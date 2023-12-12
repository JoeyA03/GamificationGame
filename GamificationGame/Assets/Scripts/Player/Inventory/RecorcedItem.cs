using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RecorcedItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData evenData)
    {
        Debug.Log("Holding");
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }


    public void OnDrag(PointerEventData evenData)
    {
        Debug.Log("Dragging");
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData evenData)
    {
        Debug.Log("Let go");
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

}
