using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    // public bool isFull;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        // isFull = false;

        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            // SelectionManager.Instance.GetComponent<SelectionManager>().enable = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            // SelectionManager.Instance.GetComponent<SelectionManager>().enable = true;

            isOpen = false;
        }
    }

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }

        }
        if (counter == 21)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name; // Stone (Clone)
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");

                itemList.Add(result);
            }
        }
    }
}
