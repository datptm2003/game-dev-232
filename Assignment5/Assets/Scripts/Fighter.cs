using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    Animator animator;

    public float cooldownTime = 0.8f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                ChopTree();
                Exploit();
            }
        }
    }

    void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1)
        {
            animator.SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            animator.SetBool("hit2", true);
        }

        if (noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false);
            animator.SetBool("hit3", true);
        }

        GameObject selectedMonster = SelectionManager.Instance.selectedMonster;

        if (selectedMonster != null)
        {
            GameObject player = this.transform.gameObject;
            // Debug.Log(playerPosition);
            // Debug.Log("AAAAABCCCC");
            StartCoroutine(ClickHit(selectedMonster, player));
        }
    }

    IEnumerator ClickHit(GameObject selectedMonster, GameObject player)
    {
        yield return new WaitForSeconds(1f);

        selectedMonster.GetComponent<Monster>().TakeDamage(PlayerState.Instance.GetDamage(),player);
    }

    void ChopTree()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            StartCoroutine(ChopHit(selectedTree));
        }
    }

    IEnumerator ChopHit(GameObject selectedTree)
    {
        resetAnimator();
        animator.SetBool("hit_tree", true);

        yield return new WaitForSeconds(1.5f);

        selectedTree.GetComponent<ChoppableTree>().GetHit();
        animator.SetBool("hit_tree", false);
    }

    void Exploit()
    {
        GameObject selectedStone = SelectionManager.Instance.selectedStone;

        if (selectedStone != null)
        {
            StartCoroutine(ExploitHit(selectedStone));
        }
    }

    IEnumerator ExploitHit(GameObject selectedStone)
    {
        resetAnimator();
        animator.SetBool("hit_stone", true);

        yield return new WaitForSeconds(1f);

        selectedStone.GetComponent<Stone>().TakeDamage(PlayerState.Instance.weaponDamage,transform.gameObject);
        animator.SetBool("hit_stone", false);
    }

    void resetAnimator()
    {
        animator.SetBool("hit1", false);
        animator.SetBool("hit2", false);
        animator.SetBool("hit3", false);
        animator.SetBool("hit_tree", false);
        animator.SetBool("hit_stone", false);
    }


    public void GetHit()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetHit();
        }


    }
}
