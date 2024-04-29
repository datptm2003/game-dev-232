using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // public static CraftingItem Instance { get; set; }
    public List<string> inventoryItemList = new List<string>();

    [Header("Item Info UI")]
    private GameObject craftItemInfoUI;
    private Text craftItemInfoUI_ItemName;
    private Text craftItemInfoUI_ItemDamage;
    private Text craftItemInfoUI_ItemStrength;
    private Text craftItemInfoUI_ItemAgility;
    private Text craftItemInfoUI_ItemLuckily;
    private Text craftItemInfoUI_ItemLuckily2;
    private Text craftItemInfoUI_ItemRequirement1;
    private Text craftItemInfoUI_ItemRequirement2;
    private Image craftItemInfoUI_ItemImage;
    // private Button craftBTN;

    [Header("Item Information")]
    public string itemName;
    public string itemDamage;
    public string itemStrength;
    public string itemAgility;
    public string itemLuckily;

    public int numOfRequirements;
    public int waitingTime;

    public string req1;
    public string req2;

    public string req1Amount;
    public string req2Amount;

    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        craftItemInfoUI = CraftingController.Instance.CraftItemInfoUI;
        craftItemInfoUI_ItemDamage = craftItemInfoUI.transform.Find("ItemDamage").GetComponent<Text>();
        craftItemInfoUI_ItemName = craftItemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        craftItemInfoUI_ItemStrength = craftItemInfoUI.transform.Find("ItemStrength").GetComponent<Text>();
        craftItemInfoUI_ItemAgility = craftItemInfoUI.transform.Find("ItemAgility").GetComponent<Text>();
        craftItemInfoUI_ItemLuckily = craftItemInfoUI.transform.Find("ItemLuckily").GetComponent<Text>();

        craftItemInfoUI_ItemRequirement1 = craftItemInfoUI.transform.Find("ItemRequirement1").GetComponent<Text>();
        craftItemInfoUI_ItemRequirement2 = craftItemInfoUI.transform.Find("ItemRequirement2").GetComponent<Text>();
        craftItemInfoUI_ItemImage = craftItemInfoUI.transform.Find("ItemImage").GetComponent<Image>();
        // craftBTN = craftItemInfoUI.transform.Find("CraftBTN").GetComponent<Button>();
        // craftBTN.onClick.AddListener(delegate { CraftAnyItem(); });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CraftingController.Instance.craftingItemNameSelected = itemName;

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

        craftItemInfoUI.SetActive(true);
        craftItemInfoUI_ItemName.text = itemName;
        craftItemInfoUI_ItemDamage.text = itemDamage;
        craftItemInfoUI_ItemStrength.text = itemStrength;
        craftItemInfoUI_ItemAgility.text = itemAgility;
        craftItemInfoUI_ItemLuckily.text = itemLuckily;

        craftItemInfoUI_ItemRequirement1.text = req1Amount;
        craftItemInfoUI_ItemRequirement2.text = req2Amount;
        craftItemInfoUI_ItemImage.sprite = sprite;

        if (stone_count >= int.Parse(req1Amount) && stick_count >= int.Parse(req2Amount))
        {
            print("Open");
            // craftBTN.gameObject.SetActive(true);
            CraftingController.Instance.isReadyToCraft = true;
        }
        else
        {
            // craftBTN.gameObject.SetActive(false);
            CraftingController.Instance.isReadyToCraft = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // craftItemInfoUI.SetActive(false);
    }

    void CraftAnyItem()
    {
        // Add item into inventory
        InventorySystem.Instance.AddToInventory(itemName.Replace(" ", ""));

        // Remove resources from inventory
        if (numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(req1, int.Parse(req1Amount));
        }
        else if (numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(req1, int.Parse(req1Amount));
            InventorySystem.Instance.RemoveItem(req2, int.Parse(req2Amount));
        }

        StartCoroutine(calculate());

        // refresh list
        RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(waitingTime);

        InventorySystem.Instance.ReCalculateList();
    }

    public void RefreshNeededItems()
    {
        // int stone_count = 0;
        // int stick_count = 0;

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

        // print(stone_count + " " + req1Amount + " " + stick_count + " " + req2Amount);

        // if (stone_count >= int.Parse(req1Amount) && stick_count >= int.Parse(req2Amount))
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
        RefreshNeededItems();
    }
}
