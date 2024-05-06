using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarketItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Information")]
    public string itemName;
    public int buyPrice;
    public int sellPrice;
    public int stock;

    [Header("Info UI")]
    // public GameObject marketScreenUI;
    private Text marketItemInfoUI_stock;
    private Text marketItemInfoUI_itemName;

    Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        // marketScreenUI = Market.Instance.marketScreenUI;

        marketItemInfoUI_itemName = transform.Find("ItemName").GetComponent<Text>();
        marketItemInfoUI_stock = transform.Find("Stock").GetComponent<Text>();

        marketItemInfoUI_itemName.text = itemName;
        marketItemInfoUI_stock.text = stock.ToString();


        ColorUtility.TryParseHtmlString("#FEC876", out newColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 0; i < Market.Instance.slotList.Count; i++)
        {
            Text itemNameText = Market.Instance.slotList[i].transform.Find("ItemName").GetComponent<Text>();
            itemNameText.color = Color.white;
        }

        Market.Instance.marketItemSelected = this;
        marketItemInfoUI_itemName.color = newColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // craftItemInfoUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        marketItemInfoUI_itemName.text = itemName;
        marketItemInfoUI_stock.text = stock.ToString();
    }
}
