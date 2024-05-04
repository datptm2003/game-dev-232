using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;
    public int minCount;
    public int maxCount;
    public int count;

    public string GetItemName()
    {
        return ItemName;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange)
        if (Input.GetKeyDown(KeyCode.Z) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (!InventorySystem.Instance.CheckIfFull())
            {
                count = count == 0 ? Random.Range(minCount, maxCount) : count;
                InventorySystem.Instance.AddToInventory(ItemName, count);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
