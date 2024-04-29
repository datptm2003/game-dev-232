using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BowWeapon : MonoBehaviour
{
    public Animator animator;
    public GameObject arrowDummy;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent.transform.parent.transform.GetChild(transform.parent.transform.parent.transform.childCount - 1).transform.gameObject.SetActive(true);
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

    public void DisplayArrow()
    {
        arrowDummy.SetActive(true);
    }
    public void GetHit()
    {
        // GameObject selectedTree = SelectionManager.Instance.selectedTree;

        // if (selectedTree != null)
        // {
        //     selectedTree.GetComponent<ChoppableTree>().GetHit();
        // }
        // bulletSpawn.SetActive(true);
        // FireWeapon();
        arrowDummy.SetActive(false);
        Weapon.Instance.FireWeapon();
    }
}
