using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }
    public bool onTarget;

    public GameObject selectedObject;

    [Header("Interaction UI")]
    public GameObject interaction_Info_UI;
    public GameObject key_Interaction_Info_UI;

    Text interaction_text;
    Text keyInteractionText;
    Image keyInteractionImage;

    public Sprite z;
    public Sprite f;
    public Sprite mouse;

    [Header("Icon")]

    public Image centerDotImage;
    public Image handIcon;

    public bool handIsVisible;

    public GameObject selectedTree;
    public GameObject chopHolder;

    public GameObject selectedMonster;
    public GameObject selectedStone;
    public GameObject monsterHealthBar;

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.transform.GetChild(1).transform.GetComponent<Text>();
        keyInteractionText = key_Interaction_Info_UI.transform.GetChild(1).transform.GetComponent<Text>();
        keyInteractionImage = key_Interaction_Info_UI.transform.GetChild(2).transform.GetComponent<Image>();
    }

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

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            Monster monster = selectionTransform.GetComponent<Monster>();
            if (monster && monster.playerInRange)
            {
                monster.canBeKilled = true;
                selectedMonster = monster.gameObject;
                monsterHealthBar.gameObject.SetActive(true);
                monsterHealthBar.gameObject.transform.GetChild(0).GetComponent<Text>().text = monster.monsterName;

                keyInteractionImage.sprite = mouse;
                keyInteractionText.text = "Kill";
                key_Interaction_Info_UI.SetActive(true);
                // interaction_text.text = monster.monsterName;
                // interaction_Info_UI.SetActive(true);

                // if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                // {
                //     StartCoroutine(DealDamageTo(monster, 0.3f, EquipSystem.Instance.GetWeaponDamagge()));
                // }
            }
            else
            {
                if (selectedMonster != null)
                {
                    selectedMonster.gameObject.GetComponent<Monster>().canBeKilled = false;
                    selectedMonster = null;
                    monsterHealthBar.gameObject.SetActive(false);
                }
                key_Interaction_Info_UI.SetActive(false);
                // interaction_text.text = "";
                // interaction_Info_UI.SetActive(false);
            }


            Stone stone = selectionTransform.GetComponent<Stone>();
            if (stone && stone.playerInRange)
            {
                stone.canBeKilled = true;
                selectedStone = stone.gameObject;
                chopHolder.gameObject.SetActive(true);
                chopHolder.gameObject.transform.GetChild(0).GetComponent<Text>().text = stone.name;

                keyInteractionImage.sprite = f;
                keyInteractionText.text = "Exploit";
                key_Interaction_Info_UI.SetActive(true);
                // interaction_text.text = monster.monsterName;
                // interaction_Info_UI.SetActive(true);

                // if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                // {
                //     StartCoroutine(DealDamageTo(monster, 0.3f, EquipSystem.Instance.GetWeaponDamagge()));
                // }
            }
            else
            {
                if (selectedStone != null)
                {
                    selectedStone.gameObject.GetComponent<Stone>().canBeKilled = false;
                    selectedStone = null;
                    chopHolder.gameObject.SetActive(false);
                }
                key_Interaction_Info_UI.SetActive(false);
                // interaction_text.text = "";
                // interaction_Info_UI.SetActive(false);
            }


            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

            if (choppableTree && choppableTree.playerInRange)
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                chopHolder.gameObject.SetActive(true);

                keyInteractionImage.sprite = f;
                keyInteractionText.text = "Chop";
                key_Interaction_Info_UI.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    chopHolder.gameObject.SetActive(false);
                }
                key_Interaction_Info_UI.SetActive(false);
            }


            // if (interactable)
            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickable"))
                {
                    centerDotImage.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);

                    handIsVisible = true;

                    keyInteractionImage.sprite = z;
                    keyInteractionText.text = "Pickup";
                    key_Interaction_Info_UI.SetActive(true);
                }
                else
                {
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);

                    handIsVisible = false;
                    key_Interaction_Info_UI.SetActive(false);
                }
            }
            else
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                key_Interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

                handIsVisible = false;
            }
        }
        else
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            key_Interaction_Info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            handIsVisible = false;
        }
    }

    IEnumerator DealDamageTo(Monster monster, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        monster.TakeDamage(damage);
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);

        selectedObject = null;
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        key_Interaction_Info_UI.SetActive(false);
    }
}