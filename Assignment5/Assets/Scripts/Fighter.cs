using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    public static Fighter Instance { get; set; }
    Animator animator;

    public float cooldownTime = 0.8f;
    private float nextFireTime = 0f;
    public static int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;
    public string typeWeapon;

    private void Awake()
    {
        
        Instance = this;
        
    }

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
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("punch1"))
        {
            animator.SetBool("punch1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("punch2"))
        {
            animator.SetBool("punch2", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("kick1"))
        {
            animator.SetBool("kick1", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("bow_hit"))
        {
            animator.SetBool("bow_hit", false);
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }
        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // OnClick();
                if (typeWeapon == "NoWeapon")
                {
                    Punch();
                }
                else if (typeWeapon == "Bow")
                {
                    BowFire();
                }
                else
                {
                    OnClick();
                }
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
        // GameObject selectedNPC = SelectionManager.Instance.selectedNPC;

        if (selectedMonster != null)
        {
            GameObject player = this.transform.gameObject;
            StartCoroutine(ClickHit(selectedMonster, player));
        }

        // if (selectedNPC != null)
        // {
        //     Debug.Log("Here");
        //     GameObject player = this.transform.gameObject;
        //     interactNPC(selectedNPC, player);
        // }
    }

    IEnumerator ClickHit(GameObject selectedMonster, GameObject player)
    {
        yield return new WaitForSeconds(1f);

        selectedMonster.GetComponent<Monster>().TakeDamage(PlayerState.Instance.GetDamage(), player);
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
        yield return new WaitForSeconds(0.5f);

        selectedTree.GetComponent<Tree>().animator.SetBool("shake", true);

        yield return new WaitForSeconds(0.5f);

        selectedTree.GetComponent<Tree>().TakeDamage(PlayerState.Instance.weaponDamage);
        animator.SetBool("hit_tree", false);
        selectedTree.GetComponent<Tree>().animator.SetBool("shake", false);
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

        yield return new WaitForSeconds(1.5f);

        selectedStone.GetComponent<Stone>().TakeDamage(PlayerState.Instance.weaponDamage, transform.gameObject);
        animator.SetBool("hit_stone", false);
    }

    void BowFire()
    {
        StartCoroutine(FireHit());
    }

    IEnumerator FireHit()
    {
        resetAnimator();

        animator.SetBool("bow_hit", true);
        yield return new WaitForSeconds(0.35f);

        Weapon.Instance.FireWeapon();
        animator.SetBool("bow_hit", false);
    }

    void Punch()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1)
        {
            animator.SetBool("punch1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("punch1"))
        {
            animator.SetBool("punch1", false);
            animator.SetBool("punch2", true);
        }

        if (noOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("punch2"))
        {
            animator.SetBool("punch2", false);
            animator.SetBool("kick1", true);
        }

        GameObject selectedMonster = SelectionManager.Instance.selectedMonster;

        if (selectedMonster != null)
        {
            StartCoroutine(ClickHit(selectedMonster, this.transform.gameObject));
        }
    }

    void resetAnimator()
    {
        animator.SetBool("hit1", false);
        animator.SetBool("hit2", false);
        animator.SetBool("hit3", false);
        animator.SetBool("hit_tree", false);
        animator.SetBool("hit_stone", false);
        animator.SetBool("bow_hit", false);
    }


    public int HitDamage(string typeObject)
    {
        if (typeObject == "Stone")
        {
            if (typeWeapon == "Hammer")
            {
                return PlayerState.Instance.weaponDamage;
            }
            else if (typeWeapon == "Bow")
            {
                return 0;
            }
            else if (typeWeapon == "NoWeapon")
            {
                return PlayerState.Instance.damageRegular;
            }
            else
            {
                // return PlayerState.Instance.weaponDamage * 0.2;
            }
        }

        return 0;
    }
}
