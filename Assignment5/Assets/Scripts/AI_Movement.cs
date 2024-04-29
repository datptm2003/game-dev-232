using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Movement : MonoBehaviour
{
    public static AI_Movement Instance { get; set; }
    public Animator animator;

    public float moveSpeed = 2f;

    Vector3 stopPosition;

    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    int WalkDirection;

    public bool isWalking;

    public bool isAttacking;


    // Start is called before the first frame update
    void Start()
    {
        // animator = GetComponent<Animator>();

        // So that all the prelabs don't move/stop at the same time
        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

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

            walkCounter -= Time.deltaTime;

            switch (WalkDirection)
            {
                case 0:
                    gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 1:
                    gameObject.transform.localRotation = Quaternion.Euler(0f, 90, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 2:
                    gameObject.transform.localRotation = Quaternion.Euler(0f, -90, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
                case 3:
                    gameObject.transform.localRotation = Quaternion.Euler(0f, 180, 0f);
                    gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * moveSpeed * Time.deltaTime;
                    break;
            }

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
                waitCounter = waitTime;
            }
        }
        else if (!isAttacking && !isWalking)
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);

        isWalking = true;
        walkCounter = walkTime;
    }
}