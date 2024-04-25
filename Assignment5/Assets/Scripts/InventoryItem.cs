using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    // ------- Is this item trashable ------- //
    public bool isTrashable;

    // ------- Item Info UI ------- //
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;
    private Image itemInfoUI_itemImage;

    public string thisName, thisDescription, thisFunctionality;
    public Sprite sprite;

    // ------- Consumption ------- //
    private GameObject itemPendingConsumptions;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;

    // ------- Equipping ------- //
    public bool isEquipable;
    private GameObject itemPendingEquipping;
    public bool isInsideQuickSlot;

    public bool isInsideHelmetSlot;
    public bool isInsideAccessoriesSlot;
    public bool isInsideArmorSlot;
    public bool isInsideBootSlot;
    public bool isInsideWeaponSlot;

    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        itemInfoUI = InventorySystem.Instance.ItemInfoUI;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
        itemInfoUI_itemImage = itemInfoUI.transform.Find("ItemImage").GetComponent<Image>();
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
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
        itemInfoUI_itemImage.sprite = sprite;

        // 
        Vector2 pos = eventData.position;
        pos.x = 900f;
        if (pos.y < 325) pos.y = 325;
        if (pos.y > 610) pos.y = 610;
        itemInfoUI.transform.position = pos;
        Debug.Log(eventData.position);
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
                EquipSystem.Instance.AddToQuickSlots(gameObject);
                isInsideQuickSlot = true;
            }
            if (isEquipable && isInsideHelmetSlot == false && gameObject.tag == "helmet")
            {
                EquipSystem.Instance.AddToEquipSlots("helmet",gameObject);
                isInsideHelmetSlot = true;
            }
            if (isEquipable && isInsideAccessoriesSlot == false && gameObject.tag == "accessories")
            {
                EquipSystem.Instance.AddToEquipSlots("accessories",gameObject);
                isInsideAccessoriesSlot = true;
            }
            if (isEquipable && isInsideArmorSlot == false && gameObject.tag == "armor")
            {
                EquipSystem.Instance.AddToEquipSlots("armor",gameObject);
                isInsideArmorSlot = true;
            }
            if (isEquipable && isInsideBootSlot == false && gameObject.tag == "boot")
            {
                EquipSystem.Instance.AddToEquipSlots("boot",gameObject);
                isInsideBootSlot = true;
            }
            if (isEquipable && isInsideWeaponSlot == false && gameObject.tag == "weapon")
            {
                EquipSystem.Instance.AddToEquipSlots("weapon",gameObject);
                isInsideWeaponSlot = true;
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
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    public void consumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);

        caloriesEffectCalculation(caloriesEffect);

        hydrationEffectCalculation(hydrationEffect);
    }

    public static void healthEffectCalculation(float healthEffect)
    {
        // ------- Health ------- //

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

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

    public static void caloriesEffectCalculation(float caloriesEffect)
    {
        // ------- Calories ------- //

        float caloriesBeforeConsumption = PlayerState.Instance.currentCalories;
        float maxCalories = PlayerState.Instance.maxCalories;

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

    public static void hydrationEffectCalculation(float hydrationEffect)
    {
        // ------- Hydration ------- //

        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercentage;
        float maxHydration = PlayerState.Instance.maxHydrationPercentage;

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
