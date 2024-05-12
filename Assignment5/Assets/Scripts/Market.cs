using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class Market : MonoBehaviour
{
    public static Market Instance { get; set; }
    public string marketName;
    public bool playerInRange;
    public GameObject marketScreenUI;
    public bool isOpen;

    [Header("Buttons")]
    public Button closeBTN;
    public Button buyBTN;
    public Button sellBTN;

    public List<GameObject> slotList = new List<GameObject>();

    public MarketItem marketItemSelected;

    private void Awake()
    {
        Instance = this;
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

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        closeBTN = marketScreenUI.transform.Find("CloseBTN").GetComponent<Button>();
        closeBTN.onClick.AddListener(delegate { closeScreen(); });

        buyBTN = marketScreenUI.transform.Find("BuyBTN").GetComponent<Button>();
        buyBTN.onClick.AddListener(delegate { buyItem(); });

        sellBTN = marketScreenUI.transform.Find("SellBTN").GetComponent<Button>();
        sellBTN.onClick.AddListener(delegate { sellItem(); });

        foreach (Transform child in marketScreenUI.transform)
        {
            if (child.CompareTag("MarketSlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void closeScreen()
    {
        isOpen = false;
        marketScreenUI.SetActive(false);
    }

    void buyItem()
    {
        if (!marketItemSelected) return;

        if (PlayerState.Instance.money >= marketItemSelected.buyPrice && marketItemSelected.stock > 0)
        {
            PlayerState.Instance.money -= marketItemSelected.buyPrice;
            marketItemSelected.stock -= 1;
            InventorySystem.Instance.AddToInventory(marketItemSelected.itemName, 1);
        }
    }

    void sellItem()
    {
        if (!marketItemSelected) return;

        if (InventorySystem.Instance.getCountOfItem(marketItemSelected.itemName) > 0)
        {
            InventorySystem.Instance.DecreaseCountOfItem(marketItemSelected.itemName, 1);
            PlayerState.Instance.money += marketItemSelected.sellPrice;
            marketItemSelected.stock += 1;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                isOpen = true;
                marketScreenUI.SetActive(true);

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            }
            else if (Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                isOpen = false;
                marketScreenUI.SetActive(false);

                if (!InventorySystem.Instance.isOpen && !CraftingController.Instance.isOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
        }
    }
}
