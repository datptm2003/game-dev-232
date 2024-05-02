using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Stone : MonoBehaviour
{
    public string name;
    public bool playerInRange;
    public bool canBeKilled;

    public int maxHealth;
    public int currentHealth;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage, GameObject player)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            IsDead();
        }
    }

    void IsDead()
    {
        // Destroy(gameObject);

        canBeKilled = false;

        // SelectionManager.Instance.selectedMonster = null;
        // SelectionManager.Instance.monsterHealthBar.gameObject.SetActive(false);

        // animator.SetTrigger("die");
        // animator.SetBool("WalkForward", false);
        // animator.SetBool("Idle", false);
        // animator.SetBool("Death", true);

        StartCoroutine(DestroyMonster(name));
    }

    IEnumerator DestroyMonster(string name)
    {
        yield return new WaitForSeconds(1f);

        // Vector3 pos = transform.position;
        // print(pos);

        string monsterModelName = name + "_Model";
        print(monsterModelName);

        // GameObject 
        Destroy(gameObject);
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        // GameObject brokenTree = Instantiate(Resources.Load<GameObject>(monsterModelName),
        //     pos, Quaternion.Euler(0, 0, 0));
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
