using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;

    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    public bool isOpen;

    // public bool isFull;

    // Pickup Popup
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        // isFull = false;

        PopulateSlotList();

        Cursor.visible = false;
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
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }

    // public void AddToInventory(string itemName)
    // {
    //     whatSlotToEquip = FindNextEmptySlot();

    //     itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
    //     itemToAdd.transform.SetParent(whatSlotToEquip.transform);

    //     itemList.Add(itemName);

    //     Sprite sprite = itemToAdd.GetComponent<Image>().sprite;

    //     TriggerPickupPopUp(itemName, sprite);

    //     ReCalculateList();
    //     // CraftingSystem.Instance.RefreshNeededItems();
    // }

    public void AddToInventory(string itemName, int count)
    {
        whatSlotToEquip = FindItemSlot(itemName);
        Sprite sprite;

        if (isEquipment(itemName))
        {
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            sprite = itemToAdd.GetComponent<Image>().sprite;
        }
        else if (whatSlotToEquip.transform.childCount > 0)
        {
            Text numHolder = whatSlotToEquip.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

            numHolder.text = (int.Parse(numHolder.text) + count).ToString();

            sprite = whatSlotToEquip.transform.GetChild(0).GetComponent<Image>().sprite;
        }
        else
        {
            itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            itemToAdd.transform.SetParent(whatSlotToEquip.transform);

            GameObject numHolderGO = Instantiate(Resources.Load<GameObject>("numHolder"), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
            numHolderGO.transform.SetParent(whatSlotToEquip.transform);

            Text numHolder = whatSlotToEquip.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

            numHolder.text = count.ToString();
            sprite = itemToAdd.GetComponent<Image>().sprite;
        }

        TriggerPickupPopUp(itemName, sprite);

        ReCalculateList();
    }

    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(HidePickupAlert());
    }

    IEnumerator HidePickupAlert()
    {
        yield return new WaitForSeconds(3f);

        pickupAlert.SetActive(false);
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
        return null;
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
        if (counter == 56)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);

                    counter -= 1;
                }
            }
        }

        ReCalculateList();

        // CraftingSystem.Instance.RefreshNeededItems();
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

    bool isEquipment(string itemName)
    {
        if (itemName == "Axe" || itemName == "Bow" || itemName == "Hammer"
            || itemName == "MoonSwordFire" || itemName == "MoonSwordIce" || itemName == "MoonSwordLight"
            || itemName == "AxeDS" || itemName == "HammerDS" || itemName == "SwordDS")
            return true;
        return false;
    }

    private GameObject FindItemSlot(string itemName)
    {
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                if (isEquipment(itemName) == false && slot.transform.GetChild(0).name.Replace("(Clone)", "") == itemName)
                    return slot;
            }
            else
            {
                return slot;
            }
        }

        return null;
    }

    public void DecreaseCountOfItem(string nameToRemove, int amountToRemove)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name.Replace("(Clone)", "") == nameToRemove && amountToRemove != 0)
                {
                    // slotList[i].transform.GetChild(1)
                    Text numHolder = slotList[i].transform.GetChild(1).transform.GetChild(1).GetComponent<Text>();

                    if (int.Parse(numHolder.text) == amountToRemove)
                    {
                        DestroyImmediate(slotList[i].transform.GetChild(1).gameObject);
                        DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    }
                    else
                    {
                        numHolder.text = (int.Parse(numHolder.text) - amountToRemove).ToString();
                    }
                }
            }
        }
    }

    public void RemoveEquipment(string nameToRemove)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name.Replace("(Clone)", "") == nameToRemove)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }

    public int getCountOfItem(string itemName)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            // if (slotList[i] != null && slotList[i].transform.childCount > 1)
            if (slotList[i] != null && slotList[i].transform != null && slotList[i].transform.childCount > 1)
            {
                if (slotList[i].transform.GetChild(0).name.Replace("(Clone)", "") == itemName)
                {
                    Transform childTransform = slotList[i].transform.GetChild(1).transform.GetChild(1);
                    if (childTransform != null)
                    {
                        int count = int.Parse(childTransform.GetComponent<Text>().text);
                        // print(itemName + " " + count);
                        return count;

                    }
                }
            }
        }
        return 0;
    }
}
