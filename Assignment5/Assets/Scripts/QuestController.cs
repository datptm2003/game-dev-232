using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestController : MonoBehaviour
{
    public GameObject questBoardUI;
    // public GameObject questTitle;
    // public GameObject questDescription;
    // public GameObject questReward;

    public List<Quest> questList = new List<Quest>();
    public List<Quest> standardQuestList = new List<Quest>();

    public List<GameObject> questSlotList = new List<GameObject>();

    public GameObject questDetailArea;
    public bool isOpenQuestDetailArea;
    public GameObject questOfferUI;

    public Button acceptBtn;
    public Button rejectBtn;

    public static QuestController Instance { get; set; }

    public bool isOpen;

    public Quest currentActiveQuest;

    public bool questSlotSelecting;

    public GameObject player;

    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        isOpen = false;
        questSlotSelecting = false;
        isOpenQuestDetailArea = false;
        // isFull = false;

        foreach (Transform child in questBoardUI.transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("QuestSlot"))
            {
                questSlotList.Add(child.gameObject);
            }
        }

        standardQuestList.Add(new Quest(0, "BIG BOSS", "Hello, welcome to the Big Boss mission.This is a very difficult and dramatic task.Prepare yourself to face it.You will head to the Kerning region and find the monster called Big Boss. Your mission is to defeat it.Please note that Big Boss is extremely fierce, so be careful.", "Stick x 1, Stone x 5", false));
        standardQuestList.Add(new Quest(1, "Quest 2", "Hahahahahahaaha", "Stick x 2, Stone x 10", false));

        acceptBtn = questOfferUI.transform.Find("AcceptBtn").GetComponent<Button>();
        acceptBtn.onClick.AddListener(delegate { AcceptQuest(currentActiveQuest); });

        rejectBtn = questOfferUI.transform.Find("RejectBtn").GetComponent<Button>();
        rejectBtn.onClick.AddListener(delegate { RejectQuest(currentActiveQuest); });

        // Debug.Log(questSlotList.Count);
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOpen)
        {
            Debug.Log("q is pressed");

            questBoardUI.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isOpen)
        {
            Debug.Log("q is pressed");

            questBoardUI.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }

        if (!questSlotSelecting)
        {
            foreach (Quest quest in questList)
            {
                // Debug.Log(quest.index);
                if (quest.state == 0)
                { // Accepted
                    questSlotList[quest.index].GetComponentInChildren<Image>().color = new Color32(76, 76, 76, 255);
                    questSlotList[quest.index].GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
                }
                else if (quest.state == 1)
                { // Completed
                    questSlotList[quest.index].GetComponentInChildren<Image>().color = new Color32(0, 125, 0, 255);
                    questSlotList[quest.index].GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
                }
                else if (quest.state == 2)
                { // Failed
                    questSlotList[quest.index].GetComponentInChildren<Image>().color = new Color32(125, 0, 0, 255);
                    questSlotList[quest.index].GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
                }
                else
                { // Unavailable
                    questSlotList[quest.index].GetComponentInChildren<Image>().color = new Color32(76, 76, 76, 0);
                    questSlotList[quest.index].GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 0);
                }

            }
        }

    }

    public void AcceptQuest(Quest quest)
    {
        questList.Add(new Quest(quest.id, quest.title, quest.description, quest.reward, true));
        questOfferUI.SetActive(false);
        NPC.Instance.isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

    public void RejectQuest(Quest quest)
    {
        questOfferUI.SetActive(false);
        NPC.Instance.isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }

    public void AddQuest(Quest quest)
    {
        questList.Add(quest);
        // Debug.Log(questList.Count);
        questSlotList[quest.index].GetComponentInChildren<Text>().text = quest.title;
    }

    public bool CheckExistQuest(int questId)
    {
        foreach (Quest quest in questList)
        {
            if (quest.id == questId) return true;
        }
        return false;
    }

    public Quest GetQuest(int questId)
    {
        foreach (Quest quest in questList)
        {
            if (quest.id == questId) return quest;
        }
        return null;
    }

    // void ShowQuestDetail() {
    //     questDescription
    // }
}