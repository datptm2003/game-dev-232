using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

// public class PlayerController : NetworkBehaviour

using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public float cooldownTime = 0.8f;
    private float nextFireTime = 0f;
    private int noOfClicks = 0;
    private float lastClickedTime = 0;
    private float maxComboDelay = 1;
    public string typeWeapon;


    public static void ResetStaticData() {
        OnAnyPlayerSpawned = null;
    }


    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpStrength = 8f; // Jump strength variable
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float gravityMultiplier = 3f; // Added gravity multiplier

    [Header("Ground Check Settings")]
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;
    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            enabled = false;
            return;
        }

    }
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();
        
        bool isJumpPressed = Input.GetButtonDown("Jump");
        MoveServerRPC(ySpeed, jumpStrength, moveAmount, moveInput, moveDir, isJumpPressed);

    }

    private void Attack() {
        if (!IsLocalPlayer)
            return;

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
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

    private void OnClick()
    {
        lastClickedTime = Time.time;
        noOfClicks++;

        if (noOfClicks == 1)
        {
            animator.SetBool("hit1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        // Send RPC to perform actions on other clients
        ClickHitServerRPC();
    }

    [ServerRpc]
    private void ClickHitServerRPC()
    {
        GameObject selectedMonster = SelectionManager.Instance.selectedMonster;

        if (selectedMonster != null)
        {
            StartCoroutine(ClickHitCoroutine(selectedMonster, gameObject));
        }
    }

    private IEnumerator ClickHitCoroutine(GameObject selectedMonster, GameObject player)
    {
        yield return new WaitForSeconds(1f);
        selectedMonster.GetComponent<Monster>().TakeDamage(PlayerState.Instance.GetDamage(), player);
    }

    private void ChopTree()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            StartCoroutine(ChopHit(selectedTree));
        }
    }

    private IEnumerator ChopHit(GameObject selectedTree)
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

    private void Exploit()
    {
        GameObject selectedStone = SelectionManager.Instance.selectedStone;

        if (selectedStone != null)
        {
            StartCoroutine(ExploitHit(selectedStone));
        }
    }

    private IEnumerator ExploitHit(GameObject selectedStone)
    {
        resetAnimator();
        animator.SetBool("hit_stone", true);

        yield return new WaitForSeconds(1.5f);

        selectedStone.GetComponent<Stone>().TakeDamage(PlayerState.Instance.weaponDamage, gameObject);
        animator.SetBool("hit_stone", false);
    }

    private void BowFire()
    {
        StartCoroutine(FireHit());
    }

    private IEnumerator FireHit()
    {
        resetAnimator();

        animator.SetBool("bow_hit", true);
        yield return new WaitForSeconds(0.35f);

        Weapon.Instance.FireWeapon();
        animator.SetBool("bow_hit", false);
    }

    private void Punch()
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
            ClickHitServerRPC(); // Trigger server RPC to perform action on other clients
        }
    }

    private void resetAnimator()
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
                // return (int)(PlayerState.Instance.weaponDamage * 0.2f); // Example calculation
            }
        }

        return 0;
    }

    [ServerRpc]
    private void MoveServerRPC(float ySpeed, float jumpStrength, float moveAmount, Vector3 moveInput, Vector3 moveDir, bool isJumpPressed)
    {
        // if (!IsOwner) return;

        if (isGrounded)
        {
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump")) // Jump input handling
            {
                ySpeed = jumpStrength; // Apply jump force
            }
        }
        else
        {
            ySpeed += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    private void onDrawGizmosSeletected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}