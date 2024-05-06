using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // ------- Is this item trashable ------- //
    [Header("Trashable")]
    public bool isTrashable;

    // ------- Item Info UI ------- //
    [Header("Item Info")]
    private GameObject itemInfoUI;
    private Text itemInfoUI_itemName;
    private Image itemInfoUI_itemImage;
    private Text itemInfoUI_itemDamage;
    private Text itemInfoUI_itemType;
    private Text itemInfoUI_itemStrength;
    private Text itemInfoUI_itemAgility;
    private Text itemInfoUI_itemLuckily;
    private Text itemInfoUI_itemAttack;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality, type;
    public int damage, attack, strength, agility, luckily;
    public Sprite sprite;

    // ------- Consumption ------- //
    [Header("Consumption")]
    private GameObject itemPendingConsumptions;
    public bool isConsumable;

    public int healthEffect;
    public int caloriesEffect;
    public int hydrationEffect;

    // ------- Equipping ------- //
    [Header("Equipping")]
    public bool isEquipable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemInfoUI_itemDamage = itemInfoUI.transform.Find("ItemDamage").GetComponent<Text>();
        itemInfoUI_itemType = itemInfoUI.transform.Find("ItemType").GetComponent<Text>();
        itemInfoUI_itemStrength = itemInfoUI.transform.Find("ItemStrength").GetComponent<Text>();
        itemInfoUI_itemAgility = itemInfoUI.transform.Find("ItemAgility").GetComponent<Text>();
        itemInfoUI_itemLuckily = itemInfoUI.transform.Find("ItemLuckily").GetComponent<Text>();
        itemInfoUI_itemAttack = itemInfoUI.transform.Find("ItemAttack").GetComponent<Text>();
        itemInfoUI_itemImage = itemInfoUI.transform.Find("ItemImage").GetComponent<Image>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }

    // Triggered when the mouse enters into the area of the item that has this script
    public void OnPointerEnter(PointerEventData eventData)
    {
        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemImage.sprite = sprite;

        itemInfoUI_itemDamage.text = "+ " + damage.ToString();
        itemInfoUI_itemType.text = type;
        itemInfoUI_itemStrength.text = "+ " + strength.ToString();
        itemInfoUI_itemAgility.text = "+ " + agility.ToString();
        itemInfoUI_itemLuckily.text = "+ " + luckily.ToString();
        itemInfoUI_itemAttack.text = "+ " + attack.ToString();

        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;

        // 
        Vector2 pos = eventData.position;

        if (pos.y < 200) // Quick slots
        {
            pos.x = 900;
            pos.y = 325;
        }
        else if (pos.x > 1050) // Inventory slots
        {
            pos.x = 900;
            if (pos.y < 325) pos.y = 325;
            if (pos.y > 610) pos.y = 610;
        }
        else // Equipment slots
        {
            // pos.x += 226;
            pos.x = 956;
            if (pos.y < 325) pos.y = 325;
            if (pos.y > 610) pos.y = 610;
        }

        itemInfoUI.transform.position = pos;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                // Setting this specific gameObject to be the item we want to destroy later
                itemPendingConsumptions = gameObject;
                consumingFunction(healthEffect, caloriesEffect, hydrationEffect);
            }

            if (isEquipable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
            {
                if (gameObject.CompareTag("WeaponEquipSlot"))
                {
                    Debug.Log("Equip weapon slot");
                    // EquipSystem.Instance.EquipItem(gameObject, "WeaponEquipSlot");
                    EquipSystem.Instance.EquipRightClick(gameObject);
                }
                // EquipSystem.Instance.AddToQuickSlots(gameObject);
                // isInsideQuickSlot = true;
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Right Mouse Button Click on
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumptions == gameObject)
            {
                // DestroyImmediate(gameObject);
                InventorySystem.Instance.DecreaseCountOfItem(thisName, 1);
                InventorySystem.Instance.ReCalculateList();
                // CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    public void consumingFunction(int healthEffect, int caloriesEffect, int hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);
    }

    public static void healthEffectCalculation(int healthEffect)
    {
        // ------- Health ------- //

        int healthBeforeConsumption = PlayerState.Instance.currentHealth;
        int maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    public static void caloriesEffectCalculation(int caloriesEffect)
    {
        // ------- Calories ------- //

        int caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        int maxCalories = PlayerState.Instance.maxCalories;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.setCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.setCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }

    public static void hydrationEffectCalculation(int hydrationEffect)
    {
        // ------- Hydration ------- //

        int hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercentage;
        int maxHydration = PlayerState.Instance.maxHydrationPercentage;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.setHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.setHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }


}
