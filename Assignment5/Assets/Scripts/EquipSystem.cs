using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // --- UI --- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();

    public GameObject helmetSlot;

    public GameObject accessoriesSlot;

    public GameObject armorSlot;

    public GameObject bootSlot;

    public GameObject weaponSlot;

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;

    public GameObject toolHolder;

    public GameObject selectedItemModel;

    public GameObject player;

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
        PopulateSlotList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectQuickSlot(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectQuickSlot(9);
        }
    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                // Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.GetComponent<Text>().color = Color.white;
                }

                Text toBeChanged = numbersHolder.transform.Find("Number" + number).GetComponent<Text>();
                toBeChanged.color = Color.yellow;
            }
            else
            {
                // We are trying to select the same slot
                selectedNumber = -1; // null

                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                // Changing the color
                foreach (Transform child in numbersHolder.transform)
                {
                    child.GetComponent<Text>().color = Color.white;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        print(selectedItemName);

        if (selectedItemName == "Axe")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(-0.624f, 0.118f, 0f), Quaternion.Euler(180f, 0f, 90f));
        }
        else if (selectedItemName == "Bow")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(0.42f, -0.22f, 0.66f), Quaternion.Euler(-6f, -18f, -23f));
        }
        else if (selectedItemName == "Hammer")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(-0.291f, 0.075f, 0f), Quaternion.Euler(0f, 0f, 90f));
        }
        else if (selectedItemName == "MoonSwordIce")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(0.045f, 0.145f, 0f), Quaternion.Euler(0f, 0f, 90f));
        }
        else if (selectedItemName == "MoonSwordFire")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(0.045f, 0.145f, 0f), Quaternion.Euler(0f, 0f, 90f));
        }
        else if (selectedItemName == "MoonSwordLight")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                new Vector3(0.045f, 0.145f, 0f), Quaternion.Euler(0f, 0f, 90f));
        }

        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else return false;
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();

        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);

        // // Getting clean name
        // string cleanName = itemToEquip.name.Replace("(Clone)", "");

        // // Adding item to list
        // itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
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

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null && selectedItemModel != null)
        {
            return selectedItemModel.GetComponent<WeaponStats>().weaponDamage;
        }
        else return 0;
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetComponent<WeaponStats>() != null)
            {
                return true;
            }
            else return false;
        }
        else return false;
    }

    public void EquipItem(GameObject itemToEquip, string tag)
    {
        GameObject availableSlot = FindNextEmptySlot(tag);
        if (availableSlot)
        {
            itemToEquip.transform.SetParent(availableSlot.transform, false);
            InventorySystem.Instance.ReCalculateList();
        }
    }

    private GameObject FindNextEmptySlot(string tag)
    {
        if (tag == "WeaponEquipSlot" && weaponSlot.transform.childCount == 0)
        {
            return weaponSlot;
        }
        return null;
    }
}
