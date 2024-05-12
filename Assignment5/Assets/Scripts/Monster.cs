using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
// using static AI_Movement;

[RequireComponent(typeof(BoxCollider))]
public class Monster : NetworkBehaviour
{
    public string monsterName;
    public bool playerInRange;
    public bool playerInRangeToAttack;
    public bool canBeKilled;

    public int maxHealth;
    public int currentHealth;

    public Animator animator;

    // Previous position
    public Vector3 previousPosition;
    public int attackDamage;

    public AI_Movement movement;

    // Start is called before the first frame update
    void Start()
    {
        // animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    public override void OnNetworkSpawn() {
        if (!IsServer) {
            enabled = false;
            return;
        }
        currentHealth = maxHealth;
    }


    public void TakeDamage(int damage, GameObject player)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            IsDead();
        }
        else
        {
            // Debug.Log(AI_Movement.Instance);
            switch (monsterName)
            {
                case "Rabbit":
                    movement.MoveAway(player, 12);
                    break;
                case "Bear":
                    movement.MoveToward(player, attackDamage, 6);
                    break;
            }

        }
    }

    void IsDead()
    {
        // Destroy(gameObject);

        canBeKilled = false;

        SelectionManager.Instance.selectedMonster = null;
        SelectionManager.Instance.monsterHealthBar.gameObject.SetActive(false);

        animator.SetTrigger("die");
        animator.SetBool("WalkForward", false);
        animator.SetBool("Idle", false);
        animator.SetBool("Death", true);

        // AI_Movement.Instance.walkCounter = 0;
        // AI_Movement.Instance.waitCounter = 8;

        if (movement.type == "Bear")
        {
            if (QuestController.Instance.CheckExistQuest(0))
            {
                Quest quest = QuestController.Instance.GetQuest(0);
                quest.state = 1;
            }
        }

        StartCoroutine(DestroyMonster(name));
    }

    IEnumerator DestroyMonster(string name)
    {
        yield return new WaitForSeconds(3f);

        Vector3 pos = transform.position;
        print(pos);

        string monsterModelName = name + "_Model";
        print(monsterModelName);

        // GameObject 
        Destroy(gameObject);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>(monsterModelName),
            pos, Quaternion.Euler(0, 0, 0));

        if (monsterModelName == "Bear_Model")
        {
            GameObject leather = Instantiate(Resources.Load<GameObject>("Leather_Model"),
                new Vector3(pos.x + 3, pos.y, pos.z), Quaternion.Euler(0, 0, 90));
        }
        else if (monsterModelName == "Rabbit_Model")
        {
            int x = Random.Range(1, 10);
            if (x < 3)
            {
                GameObject leather = Instantiate(Resources.Load<GameObject>("Leather_Model"),
                new Vector3(pos.x, pos.y, pos.z), Quaternion.Euler(0, 0, 90));
            }
        }


        SelectionManager.Instance.monsterHealthBar.gameObject.SetActive(false);
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

    // public void GetHit() {
    //     animator.SetTrigger("shake");

    //     currentHealth -= 1;


    // }

    // Update is called once per frame
    void Update()
    {
        if (canBeKilled)
        {
            GlobalState.Instance.resourceHealth = currentHealth;
            GlobalState.Instance.resourceMaxHealth = maxHealth;
        }
    }
}
