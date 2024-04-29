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
    public GameObject useItemsScreenUI;

    public string craftingItemNameSelected;

    // Category buttons
    public Button toolsBTN;
    public Button useItemsBTN;

    public List<string> inventoryItemList = new List<string>();

    public bool isOpen;

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

        useItemsBTN = craftingScreenUI.transform.Find("UseItemsButton").GetComponent<Button>();
        useItemsBTN.onClick.AddListener(delegate { OpenUseItemsCategory(); });

        craftBTN = craftingScreenUI.transform.Find("CraftBTN").GetComponent<Button>();
        craftBTN.onClick.AddListener(delegate { CraftItemSelected(); });
    }

    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(true);
        useItemsScreenUI.SetActive(false);
    }

    void OpenUseItemsCategory()
    {
        craftingScreenUI.SetActive(false);
        useItemsScreenUI.SetActive(true);
    }

    void CraftItemSelected()
    {
        if (craftingItemNameSelected == "" || !isReadyToCraft) return;
        string itemName = craftingItemNameSelected.Replace(" ", "");
        print(itemName);
        // Add item into inventory
        InventorySystem.Instance.AddToInventory(craftingItemNameSelected.Replace(" ", ""));

        CraftingItem item = craftingScreenUI.transform.Find(itemName).GetComponent<CraftingItem>();

        // Remove resources from inventory
        if (item.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(item.req1, int.Parse(item.req1Amount));
        }
        else if (item.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(item.req1, int.Parse(item.req1Amount));
            InventorySystem.Instance.RemoveItem(item.req2, int.Parse(item.req2Amount));
        }

        StartCoroutine(calculate(item.waitingTime));

        RefreshNeededItems(item);
    }

    public IEnumerator calculate(int waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);

        InventorySystem.Instance.ReCalculateList();
    }

    public void RefreshNeededItems(CraftingItem item)
    {
        int stone_count = 0;
        int stick_count = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        if (stone_count >= int.Parse(item.req1Amount) && stick_count >= int.Parse(item.req2Amount))
        {
            print("Openned");
            craftBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBTN.gameObject.SetActive(false);
        }
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
            useItemsScreenUI.SetActive(false);

            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }

        if (isReadyToCraft)
        {
            craftBTN.gameObject.SetActive(true);
        }
        else
        {
            craftBTN.gameObject.SetActive(false);
        }
    }
}
