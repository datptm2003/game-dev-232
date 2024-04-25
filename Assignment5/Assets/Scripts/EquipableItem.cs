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
            CraftingSystem.Instance.isOpen == false &&
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
            Debug.Log(EquipSystem.Instance.GetWeaponDamage());
            selectedMonster.GetComponent<Monster>().TakeDamage(EquipSystem.Instance.GetWeaponDamage());
        }
    }
}