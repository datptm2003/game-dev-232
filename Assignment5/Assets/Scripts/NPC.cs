using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NPC : MonoBehaviour
{
    Animator animator;
    public bool playerInRange;

    public string name;

    public int questId;
    
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
    //     {
    //         animator.SetBool("hit1", false);
    //     }
    //     if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
    //     {
    //         animator.SetBool("hit2", false);
    //     }
    //     if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
    //     {
    //         animator.SetBool("hit3", false);
    //     }

    //     if (Time.time - lastClickedTime > maxComboDelay)
    //     {
    //         noOfClicks = 0;
    //     }
    //     if (Time.time > nextFireTime)
    //     {
    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             OnClick();
    //         }
    //         else if (Input.GetKeyDown(KeyCode.F))
    //         {
    //             ChopTree();
    //             Exploit();
    //         }
    //     }
    // }

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
