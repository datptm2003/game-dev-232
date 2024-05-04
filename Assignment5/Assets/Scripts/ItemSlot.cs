using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject Item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // if there is not item already then set our item.
        if (!Item)
        {
            bool check = false;

            if (transform.CompareTag("Slot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                check = true;
            }
            else if (transform.CompareTag("QuickSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = true;
                check = true;
            }
            else if (transform.CompareTag("WeaponEquipSlot") && DragDrop.itemBeingDragged.CompareTag("WeaponEquipSlot"))
            {
                DragDrop.itemBeingDragged.GetComponent<InventoryItem>().isInsideQuickSlot = false;
                check = true;
            }

            GameObject numHolder = DragDrop.numHolderOfItemBeingDragged;

            if (check)
            {
                DragDrop.itemBeingDragged.transform.SetParent(transform);
                DragDrop.itemBeingDragged.transform.localPosition = new Vector2(0, 0);

                if (numHolder)
                {
                    numHolder.transform.SetParent(transform);
                    numHolder.transform.localPosition = new Vector2(0, 0);
                }
                InventorySystem.Instance.ReCalculateList();
            }
            else
            {
                print("here2");
                if (numHolder)
                {
                    print("here3");
                    numHolder.transform.SetParent(transform);
                    numHolder.transform.localPosition = new Vector2(0, 0);
                }
            }
        }
    }

    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
