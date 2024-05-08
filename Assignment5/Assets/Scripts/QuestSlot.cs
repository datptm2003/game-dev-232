using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public int index;
    private GameObject questSlotUI;
    // private Text questTitleUI;

    public Quest quest;

    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(index);
        questSlotUI = QuestController.Instance.questSlotList[index];
        // Debug.Log(QuestController.Instance.questSlotList.Count);
        // Debug.Log(questSlotUI);

        // questTitleUI = questSlotUI.GetComponentInChildren<Text>();

        if (index < QuestController.Instance.questList.Count)
        {
            quest = QuestController.Instance.questList[index];
        }
        else
        {
            quest = null;
        }


        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (isSelected)
        // {
        //     gameObject.GetComponent<DragDrop>().enabled = false;
        // }
        // else
        // {
        //     gameObject.GetComponent<DragDrop>().enabled = true;
        // }
        // Debug.Log(QuestController.Instance.questList.Count);
        if (index < QuestController.Instance.questList.Count)
        {
            quest = QuestController.Instance.questList[index];
        }
        else
        {
            quest = null;
        }
    }

    // Triggered when the mouse enters into the area of the item that has this script

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (quest == null) return;
        // Image bg = questSlotUI.GetComponentInChildren<Image>();
        // if (bg.color.a == 0) return;
        questSlotUI.GetComponentInChildren<Image>().color = new Color32(0, 125, 255, 255);
        QuestController.Instance.questSlotSelecting = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // if (quest == null) return;
        // Image bg = questSlotUI.GetComponentInChildren<Image>();
        // if (bg.color.a == 0) return;
        // questSlotUI.GetComponentInChildren<Image>().color = new Color(bg.color.r, bg.color.g, bg.color.b, 255);
        QuestController.Instance.questSlotSelecting = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Left Mouse Button Click on
        if (quest == null) return;
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject questDetail = QuestController.Instance.questDetailArea;
            questDetail.transform.Find("QuestTitle").GetComponent<Text>().text = quest.title;
            questDetail.transform.Find("Description").GetComponent<Text>().text = quest.description;
            questDetail.transform.Find("Reward").GetComponent<Text>().text = quest.reward;

            if (QuestController.Instance.isOpenQuestDetailArea == false)
            {
                QuestController.Instance.isOpenQuestDetailArea = true;
                questDetail.SetActive(true);
            }
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Left Mouse Button Click on
        if (quest == null) return;

    }
}