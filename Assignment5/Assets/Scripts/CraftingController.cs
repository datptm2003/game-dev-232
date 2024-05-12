using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingController : MonoBehaviour
{
    [Header("CraftingItem")]
    public GameObject CraftItemInfoUI;
    public Button craftBTN;
    public bool isReadyToCraft = false;

    public static CraftingController Instance { get; set; }
    public GameObject craftingScreenUI;

    [Header("Crafting Item Selected")]
    public CraftingItem craftingItemSelected;
    public string craftingItemNameSelected;

    // Category buttons
    public Button toolsBTN;
    // public Button useItemsBTN;

    public List<string> inventoryItemList = new List<string>();

    public bool isOpen;

    [Header("Materials")]
    public Sprite Stone;
    public Sprite Iron;
    public Sprite Steel;
    public Sprite Stick;
    public Sprite Wood;
    public Sprite Leather;

    private void Awake()
    {
        Instance = this;
        
    }

    public GameObject GetCraftingItemInfoUI()
    {
        return CraftItemInfoUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        // Category buttons
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

        // useItemsBTN = craftingScreenUI.transform.Find("UseItemsButton").GetComponent<Button>();
        // useItemsBTN.onClick.AddListener(delegate { OpenUseItemsCategory(); });

        craftBTN = craftingScreenUI.transform.Find("CraftBTN").GetComponent<Button>();
        craftBTN.onClick.AddListener(delegate { CraftItemSelected(); });
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(true);
        // useItemsScreenUI.SetActive(false);
    }

    void OpenUseItemsCategory()
    {
        craftingScreenUI.SetActive(false);
        // useItemsScreenUI.SetActive(true);
    }

    void CraftItemSelected()
    {
        if (craftingItemNameSelected == "" || !isReadyToCraft) return;
        string itemName = craftingItemNameSelected.Replace(" ", "");

        // Add item into inventory
        InventorySystem.Instance.AddToInventory(itemName, 1);

        CraftingItem item = craftingScreenUI.transform.Find(itemName).GetComponent<CraftingItem>();

        // Remove resources from inventory
        if (item.numOfRequirements == 1)
        {
            InventorySystem.Instance.DecreaseCountOfItem(item.req1, int.Parse(item.req1Amount));
        }
        else if (item.numOfRequirements == 2)
        {
            InventorySystem.Instance.DecreaseCountOfItem(item.req1, int.Parse(item.req1Amount));
            InventorySystem.Instance.DecreaseCountOfItem(item.req2, int.Parse(item.req2Amount));
        }

        StartCoroutine(calculate(item.waitingTime));

        RefreshNeededItems(item);
    }

    public IEnumerator calculate(int waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);

        InventorySystem.Instance.ReCalculateList();
    }

    public int getCountOfItem(string itemName)
    {
        List<GameObject> slotList = InventorySystem.Instance.slotList;
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

    public void RefreshNeededItems(CraftingItem item)
    {
        int stone_count = getCountOfItem("Stone");
        int stick_count = getCountOfItem("Stick");

        // print("stone_count = " + stone_count);
        // print("stick_count = " + stick_count);

        // inventoryItemList = InventorySystem.Instance.itemList;

        // foreach (string itemName in inventoryItemList)
        // {
        //     switch (itemName)
        //     {
        //         case "Stone":
        //             stone_count += 1;
        //             break;
        //         case "Stick":
        //             stick_count += 1;
        //             break;
        //     }
        // }

        // if (stone_count >= int.Parse(item.req1Amount) && stick_count >= int.Parse(item.req2Amount))
        // {
        //     print("Openned");
        //     craftBTN.gameObject.SetActive(true);
        // }
        // else
        // {
        //     craftBTN.gameObject.SetActive(false);
        // }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            Debug.Log("c is pressed");

            craftingScreenUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            Debug.Log("c is pressed");

            craftingScreenUI.SetActive(false);
            // useItemsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }

        // if (isReadyToCraft)
        // {
        //     craftBTN.gameObject.SetActive(true);
        // }
        // else
        // {
        //     craftBTN.gameObject.SetActive(false);
        // }
        RefreshNeededItems();
    }

    public void RefreshNeededItems()
    {
        if (craftingItemSelected)
        {
            int stone_count = getCountOfItem(craftingItemSelected.req1);
            int stick_count = getCountOfItem(craftingItemSelected.req2);

            if (stone_count >= int.Parse(craftingItemSelected.req1Amount) && stick_count >= int.Parse(craftingItemSelected.req2Amount))
            {
                craftBTN.gameObject.SetActive(true);
                isReadyToCraft = true;
            }
            else
            {
                craftBTN.gameObject.SetActive(false);
                isReadyToCraft = false;
            }
        }
    }
}
