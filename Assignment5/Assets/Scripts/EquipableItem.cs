using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && // Left mouse button
            InventorySystem.Instance.isOpen == false &&
            // CraftingSystem.Instance.isOpen == false &&
            CraftingController.Instance.isOpen == false &&
            SelectionManager.Instance.handIsVisible == false
        )
        {


            animator.SetTrigger("hit");
        }
    }

    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }

        GameObject selectedMonster = SelectionManager.Instance.selectedMonster;

        if (selectedMonster != null)
        {
            selectedMonster.GetComponent<Monster>().TakeDamage(PlayerState.Instance.GetDamage(), this.transform.gameObject);
        }

        GameObject selectedStone = SelectionManager.Instance.selectedStone;

        if (selectedStone != null)
        {
            selectedStone.GetComponent<Stone>().TakeDamage(PlayerState.Instance.weaponDamage, this.transform.gameObject);
        }
    }
}
