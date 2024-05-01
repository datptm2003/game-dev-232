using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Tree : MonoBehaviour
{
    public string name;
    public bool playerInRange;
    public bool canBeChopped;

    public int maxHealth;
    public int currentHealth;

    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            IsDead();
        }
    }

    void IsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(gameObject);

        canBeChopped = false;

        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject brokenTree = Instantiate(Resources.Load<GameObject>("ChoppedTree"),
            new Vector3(treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler(0, 0, 0));


        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);
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
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = currentHealth;
            GlobalState.Instance.resourceMaxHealth = maxHealth;
        }
    }
}
