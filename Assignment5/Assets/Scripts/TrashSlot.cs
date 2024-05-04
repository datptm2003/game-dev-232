using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject trashAlertUI;

    private Text textToModify;

    public Sprite trash_closed;
    public Sprite trash_opened;

    private Image imageComponent;

    Button YesBTN, NoBTN;

    GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    GameObject itemToBeDeleted;
    GameObject numHolderOfItemBeingDragged;

    public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        imageComponent = transform.Find("background").GetComponent<Image>();

        textToModify = trashAlertUI.transform.Find("Text").GetComponent<Text>();

        YesBTN = trashAlertUI.transform.Find("yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });

        NoBTN = trashAlertUI.transform.Find("no").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
    }

    public void OnDrop(PointerEventData eventData)
    {
        // itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;

            StartCoroutine(notifyBeforeDeletion());
        }
    }

    IEnumerator notifyBeforeDeletion()
    {
        trashAlertUI.SetActive(true);
        textToModify.text = "Throw away this " + itemName + "?";
        yield return new WaitForSeconds(1f);
    }

    private void CancelDeletion()
    {
        imageComponent.sprite = trash_closed;
        trashAlertUI.SetActive(false);
    }

    private void DeleteItem()
    {
        imageComponent.sprite = trash_closed;
        // print(itemToBeDeleted.name);

        List<GameObject> slotList = InventorySystem.Instance.slotList;
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i] != null && slotList[i].transform != null && slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == "numHolder(Clone)")
                {
                    print("Here1");
                    numHolderOfItemBeingDragged = slotList[i].transform.GetChild(0).gameObject;
                    print(i);
                    break;
                }
            }
        }

        if (numHolderOfItemBeingDragged)
        {
            Vector3 pos = EquipSystem.Instance.player.transform.position;

            if (itemName == "Leather" || itemName == "Stick" || itemName == "Wood")
            {
                GameObject leather = Instantiate(Resources.Load<GameObject>(itemName + "_Model"),
                    pos, Quaternion.Euler(0, 0, 90));
            }
            else
            {
                GameObject stick = Instantiate(Resources.Load<GameObject>(itemName + "_Model"),
                    pos, Quaternion.Euler(0, 0, 0));
            }

            DestroyImmediate(numHolderOfItemBeingDragged.gameObject);
            DestroyImmediate(itemToBeDeleted.gameObject);
        }

        InventorySystem.Instance.ReCalculateList();
        // CraftingSystem.Instance.RefreshNeededItems();
        trashAlertUI.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_opened;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageComponent.sprite = trash_closed;
        }
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }
}
