using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static AI_Movement;

[RequireComponent(typeof(BoxCollider))]
public class Monster : MonoBehaviour
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
        currentHealth = maxHealth;
        // animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    public void TakeDamage(int damage, GameObject player)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            IsDead();
        } else {
            // Debug.Log(AI_Movement.Instance);
            switch(monsterName) {
                case "Rabbit":
                    movement.MoveAway(player,12);
                    break;
                case "Bear":
                    movement.MoveToward(player,attackDamage,6);
                    break;
            }
            
        }
    }

    // public void Attack(GameObject player) {
    //     double curX = transform.position.x;
    //     double curZ = transform.position.z;
    //     double playerX = player.transform.position.x;
    //     double playerZ = player.transform.position.z;


    // }

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

        if (movement.type == "Bear") {
            if (QuestController.Instance.CheckExistQuest(0)) {
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
