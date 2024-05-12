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
    public GameObject SubToolHolder;

    public GameObject selectedItemModel;

    public GameObject player;

    private void Awake()
    {
  

        Instance = this;
        
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

        selectedItem.name = selectedItem.name.Replace("(Clone)", "");

        string selectedItemName = selectedItem.name.Replace("(Clone)", "");

        InventoryItem item = selectedItem.transform.GetComponent<InventoryItem>();
        string typeWeapon = "NoWeapon";
        if (item.isEquipable)
        {
            typeWeapon = item.type;
            // Display weapon model
            if (item.type == "Sword")
            {
                selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                    new Vector3(0.045f, 0.145f, 0f), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (item.type == "Axe")
            {
                print(selectedItemName);
                if (selectedItemName == "Axe")
                {
                    selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                        new Vector3(-0.624f, 0.118f, 0f), Quaternion.Euler(180f, 0f, 90f));
                }
                else
                {
                    selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                        new Vector3(-0.332f, 0.143f, 0f), Quaternion.Euler(0, -90f, -90f));
                }
            }
            else if (item.type == "Bow")
            {
                selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                    new Vector3(0f, 0.134f, 0f), Quaternion.Euler(0f, 0f, 90f));
            }
            else if (item.type == "Hammer")
            {
                selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"),
                    new Vector3(-0.291f, 0.075f, 0f), Quaternion.Euler(0f, 0f, 90f));
            }

            // Update stats
            UpdateStats(item.damage, item.strength, item.agility, item.luckily);

            // Display weapon equipped
            if (selectedItem.gameObject.CompareTag("WeaponEquipSlot"))
                EquipItem(selectedItem.gameObject, "WeaponEquipSlot");
        }

        PlayerState.Instance.typeWeapon = typeWeapon;
        if (typeWeapon == "Bow")
        {
            selectedItemModel.transform.SetParent(SubToolHolder.transform, false);
        }
        else
        {
            selectedItemModel.transform.SetParent(toolHolder.transform, false);
        }
    }

    void UpdateStats(int damage, int strength, int agility, int luckily)
    {
        PlayerState.Instance.setWeaponDamage(damage);
        PlayerState.Instance.setStrength(strength);
        PlayerState.Instance.setAgility(agility);
        PlayerState.Instance.setLuckily(luckily);
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

        if (availableSlot)
        {
            itemToEquip.transform.SetParent(availableSlot.transform, false);
        }
        else
        {
            Destroy(quickSlotsList[0].transform.GetChild(0).gameObject);
            itemToEquip.transform.SetParent(quickSlotsList[0].transform, false);
        }

        // Set transform of our object

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
        return null;
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
        print(availableSlot.transform.childCount);
        if (availableSlot.transform.childCount > 0)
        {
            Destroy(availableSlot.transform.GetChild(0).gameObject);
        }
        GameObject itemCopy = Instantiate(itemToEquip);
        itemCopy.transform.position = availableSlot.transform.position;
        itemCopy.transform.localScale = new Vector3(1.2f, 1.2f, 0.9f);
        itemCopy.transform.parent = availableSlot.transform;
        // itemToEquip.transform.SetParent(availableSlot.transform, false);
        // InventorySystem.Instance.ReCalculateList();
        // }
    }

    private GameObject FindNextEmptySlot(string tag)
    {
        if (tag == "WeaponEquipSlot")
        {
            return weaponSlot;
        }
        return null;
    }

    public void EquipRightClick(GameObject item)
    {
        if (CheckIfFull())
        {
            Destroy(quickSlotsList[0].transform.GetChild(0).gameObject);
            item.transform.SetParent(quickSlotsList[0].transform, false);

            SelectQuickSlot(1);
        }
        else
        {
            // EquipSystem.Instance.AddToQuickSlots(item);
            GameObject availableSlot = FindNextEmptySlot();

            item.transform.SetParent(availableSlot.transform, false);

            print(availableSlot.gameObject.name.Replace("QuickSlot", ""));
            int number = int.Parse(availableSlot.gameObject.name.Replace("QuickSlot", ""));
            print(number);
            SelectQuickSlot(number);
        }
    }
}
