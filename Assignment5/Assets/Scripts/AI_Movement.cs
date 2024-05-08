using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static System.Math;

public class AI_Movement : MonoBehaviour
{
    // public static AI_Movement Instance { get; set; }
    public Animator animator;

    public float moveSpeed = 2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;
    int chaseTime;

    int WalkDirection;

    public bool isWalking;

    public bool isAttacking;

    public int attackDamage;

    public GameObject curPlayer;

    public string type;


    // Start is called before the first frame update
    void Start()
    {
        // animator = GetComponent<Animator>();

        // So that all the prelabs don't move/stop at the same time
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);
        chaseTime = 0;

        waitCounter = waitTime;
        walkCounter = walkTime;

        isAttacking = false;

        ChooseDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking && isWalking)
        {
            animator.SetTrigger("WalkForward");
            animator.SetBool("WalkForward", true);
            animator.SetBool("Idle", false);
            // animator.ResetTrigger("Attack1");

            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    gameObject.transform.rotation = Quaternion.Euler(0f, 90, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    gameObject.transform.rotation = Quaternion.Euler(0f, -90, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    gameObject.transform.rotation = Quaternion.Euler(0f, 180, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 4:
                    gameObject.transform.rotation = Quaternion.Euler(0f, 45, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 5:
                    gameObject.transform.rotation = Quaternion.Euler(0f, -45, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 6:
                    gameObject.transform.rotation = Quaternion.Euler(0f, 135, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 7:
                    gameObject.transform.rotation = Quaternion.Euler(0f, -135, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }

            // gameObject.transform.rotation = Quaternion.Euler(0f, WalkDirection*180, 0f);
            // gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;

            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;
                // stop movement
                transform.position = stopPosition;
                // animator.SetBool("isRunning", false);
                animator.SetTrigger("Idle");
                animator.SetBool("Idle", true);
                animator.SetBool("WalkForward", false);

                // reset the waitCounter
                if (chaseTime > 0)
                {
                    waitCounter = waitTime / 20;
                    moveSpeed = 3f;
                }
                else
                {
                    waitCounter = waitTime;
                    moveSpeed = 2f;
                }

            }
        }
        else if (!isAttacking && !isWalking)
        {
            waitCounter -= Time.deltaTime;
            // Debug.Log(chaseTime);
            if (waitCounter <= 0 && chaseTime > 0)
            {
                // Debug.Log(chaseTime);
                switch (type)
                {
                    case "Bear":
                        MoveToward(curPlayer, attackDamage, chaseTime - 1);
                        // Attack();
                        break;
                    case "Rabbit":
                        MoveAway(curPlayer, chaseTime - 1);
                        break;
                }


            }
            else if (waitCounter <= 0 && chaseTime == 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 7);

        isWalking = true;
        walkCounter = walkTime;
    }

    public void MoveToward(GameObject player, int attackDmg, int objChaseTime)
    {
        if (player == null) return;
        curPlayer = player;
        attackDamage = attackDmg;

        double curX = transform.position.x;
        double curZ = transform.position.z;
        double playerX = player.transform.position.x;
        double playerZ = player.transform.position.z;
        double[,] disUnit = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 }, { Sqrt(2) / 2, Sqrt(2) / 2 }, { -Sqrt(2) / 2, Sqrt(2) / 2 }, { Sqrt(2) / 2, -Sqrt(2) / 2 }, { -Sqrt(2) / 2, -Sqrt(2) / 2 } }; // Need to be modified


        double minDis = Sqrt((playerX - (curX + disUnit[0, 0] * moveSpeed)) * (playerX - (curX + disUnit[0, 0] * moveSpeed)) + (playerZ - (curZ + disUnit[0, 1] * moveSpeed)) * (playerZ - (curZ + disUnit[0, 1] * moveSpeed)));

        for (int i = 0; i < 8; i++)
        {
            double curDis = Sqrt((playerX - (curX + disUnit[i, 0] * moveSpeed)) * (playerX - (curX + disUnit[i, 0] * moveSpeed)) + (playerZ - (curZ + disUnit[i, 1] * moveSpeed)) * (playerZ - (curZ + disUnit[i, 1] * moveSpeed)));
            if (curDis <= minDis)
            {
                WalkDirection = i;
                minDis = curDis;
            }
        }

        isWalking = true;
        walkCounter = walkTime / 3;
        chaseTime = objChaseTime;

        Attack(player, attackDmg);
    }

    public void MoveAway(GameObject player, int objChaseTime)
    {
        if (player == null) return;
        curPlayer = player;

        double curX = transform.position.x;
        double curZ = transform.position.z;
        double playerX = player.transform.position.x;
        double playerZ = player.transform.position.z;
        double[,] disUnit = { { 0, 1 }, { 1, 0 }, { -1, 0 }, { 0, -1 }, { Sqrt(2) / 2, Sqrt(2) / 2 }, { -Sqrt(2) / 2, Sqrt(2) / 2 }, { Sqrt(2) / 2, -Sqrt(2) / 2 }, { -Sqrt(2) / 2, -Sqrt(2) / 2 } }; // Need to be modified


        double maxDis = Sqrt((playerX - (curX + disUnit[0, 0] * moveSpeed)) * (playerX - (curX + disUnit[0, 0] * moveSpeed)) + (playerZ - (curZ + disUnit[0, 1] * moveSpeed)) * (playerZ - (curZ + disUnit[0, 1] * moveSpeed)));

        for (int i = 0; i < 8; i++)
        {
            double curDis = Sqrt((playerX - (curX + disUnit[i, 0] * moveSpeed)) * (playerX - (curX + disUnit[i, 0] * moveSpeed)) + (playerZ - (curZ + disUnit[i, 1] * moveSpeed)) * (playerZ - (curZ + disUnit[i, 1] * moveSpeed)));
            if (curDis >= maxDis)
            {
                WalkDirection = i;
                maxDis = curDis;
            }
        }

        isWalking = true;
        walkCounter = walkTime / 3;
        chaseTime = objChaseTime;


    }

    public void Attack(GameObject player, int monsterDmg)
    {
        if (player == null) return;
        // Debug.Log(PlayerState.Instance.currentHealth);
        double curX = transform.position.x;
        double curZ = transform.position.z;
        double playerX = player.transform.position.x;
        double playerZ = player.transform.position.z;

        if (Sqrt((playerX - curX) * (playerX - curX) + (playerZ - curZ) * (playerZ - curZ)) < 5)
        {
            animator.SetTrigger("Attack1");
            // animator.SetBool("Attack1", true);
            animator.SetBool("Idle", false);
            animator.SetBool("WalkForward", false);
            PlayerState.Instance.currentHealth -= monsterDmg;
        }
    }

    // private void Awake()
    // {
    //     if (Instance != null && Instance != this)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         Instance = this;
    //     }
    // }
}