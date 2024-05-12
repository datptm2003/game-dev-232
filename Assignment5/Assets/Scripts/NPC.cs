using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class NPC : MonoBehaviour
{
    public static NPC Instance { get; set; }

    public bool playerInRange;
    public bool isOpen;
    public string npcName;

    public int questId;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    void interactNPC()
    {
        GameObject player = QuestController.Instance.player;

        GameObject questOfferUI = QuestController.Instance.questOfferUI;
        Quest currentActiveQuest = QuestController.Instance.standardQuestList[questId];
        QuestController.Instance.currentActiveQuest = currentActiveQuest;

        Quest questInList = QuestController.Instance.GetQuest(currentActiveQuest.id);

        if (questInList != null)
        {
            print(questInList.state);
        }
        if (!QuestController.Instance.CheckExistQuest(currentActiveQuest.id) || questInList.state == 0)
        {
            questOfferUI.transform.Find("Title").GetComponent<Text>().text = currentActiveQuest.title;
            questOfferUI.transform.Find("Description").GetComponent<Text>().text = currentActiveQuest.description;
            questOfferUI.transform.Find("Reward").GetComponent<Text>().text = currentActiveQuest.reward;
            print(questOfferUI);

            questOfferUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (questInList.state == 1 && questInList.isCompleted == false)
        {
            foreach (KeyValuePair<string, int> item in questInList.rewardDict)
            {
                for (int i = 0; i < item.Value; ++i)
                {
                    InventorySystem.Instance.AddToInventory(item.Key, 1);
                }
            }
            questInList.isCompleted = true;
        }
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isOpen)
            {
                Debug.Log("e is pressed");

                interactNPC();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                SelectionManager.Instance.DisableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

                isOpen = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && isOpen)
            {
                Debug.Log("e is pressed");

                // questBoardUI.SetActive(false);
                GameObject questOfferUI = QuestController.Instance.questOfferUI;
                questOfferUI.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                SelectionManager.Instance.EnableSelection();
                SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

                isOpen = false;
            }

        }
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
}
