using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController playerController;
    public PhotonView PV;

    public InputHandler input;
    public Rigidbody rb;
    public EnduranceHandler enduranceHandler;
    Animator anim;
    public CapsuleCollider playerHitBox;
    public CapsuleCollider playerParryBox;
    public CapsuleCollider playerCol;
    public BoxCollider weaponCol;
    public Transform target;
    [SerializeField] private Transform oldTarget;
    //public AttackHandler attackHandler;
    
    
    [Header("Data")]
    public float runSpeed = 7f;
    public float jumpSpeed = 14f;
    public int jumpCost = 1;
    public float health;
    public float healthMax = 10f;
    public float endurance;
    public float enduranceMax = 20f;
    public float enduranceTicker = 3f;
    public float enduranceTickerAdd = 1f;

    [Header("Bools")]
    [SerializeField] public bool canRun = true;
    [SerializeField] private bool isRunning;
    [SerializeField] public bool canAttack = true;
    [SerializeField] public bool isAttacking;
    [SerializeField] public bool isParrying;
    [SerializeField] public bool canParry = true;
    [SerializeField] public bool isStunned = false;
    [SerializeField] public bool canJump = true;
    [SerializeField] public bool isJumping;
    [SerializeField] public bool isHit;
    [SerializeField] public bool isDead;
    [SerializeField] private bool lockedPos;

    [Header("Debug")]
    [SerializeField] private Vector3 rbPos;
    [SerializeField] private Transform rbGizmo;
    [SerializeField] private bool targetFound = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
    }
    
    void Start()
    {
        PV = GetComponent<PhotonView>();        
    
        if (PV.IsMine)
        {
            if (PlayerController.playerController == null)
            {
                PlayerController.playerController = this;
            }
            else
            {
                if (PlayerController.playerController != this)
                {
                    Destroy(PlayerController.playerController.gameObject);
                    PlayerController.playerController = this;
                }
            }
            PV.RPC("InitStats",RpcTarget.AllBuffered, 20, 20);
            GetComponents();
        }       


        //weaponCol.enabled = false;
        //if (PV.IsMine == true)
        //{
        //    GetComponent<InputHandler>().enabled = true;
        //    GetComponent<PlayerController>().enabled = true;
        //    GetComponent<PhotonTransformView>().enabled = true;
        //    this.transform.GetChild(0).GetComponent<Animator>().enabled = true;
        //    this.transform.GetChild(0).GetComponent<AttackHandler>().enabled = true;
        //    this.transform.GetChild(0).GetComponent<EnduranceHandler>().enabled = true;
        //    this.transform.GetChild(0).GetComponent<ParryHandler>().enabled = true;
        //    this.transform.GetChild(1).GetComponent<ParryBox>().enabled = true;
        //    this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetComponent<HitBox>().enabled = true;
        //    this.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<WeaponInfo>().enabled = true;
        //}
    }

    void Update()
    {
        if (PV.IsMine)
        {
            if (canAttack && target != null && !isDead)
            {
                transform.LookAt(target);
            }

            Jump();
            ApplyMovement();
            ApplyAttack();
            ApplyParry();
            if (health <= 0)
            {
                Death();
            }
            DrawGizmos();

            if(!targetFound)
            {
                if (this.transform.parent.gameObject.name == "Player1")
                {
                    target = GameObject.Find("Player2").transform;
                    targetFound = true;
                }
                else
                {
                    target = GameObject.Find("Player1").transform;
                    targetFound = true;
                }
            }
        }        
    }        
    
        
    private void DrawGizmos()
    {        
        // Rigidbody Position Gizmo
        if (rbGizmo != null)
        {
            rbPos = rb.position;
            rbGizmo.position = rbPos;
        }        
    }

    private void Jump()
    {        
        if (canJump && input.jumpBack && (int)endurance >= jumpCost)
        {
            enduranceHandler.RemoveEndurance(jumpCost);
            playerHitBox.enabled = false;
            weaponCol.enabled = false;
            // If Jumping in a specific direction
            if (input.vector != Vector2.zero)
            {
                Vector3 moveInput = new Vector3(input.vector.x, 0, input.vector.y);
                rb.AddForce(moveInput * jumpSpeed * 40);
            }
            // Jump away from target
            else
            {

            }
            isJumping = true;
            canJump = false;
            canParry = false;
            isAttacking = false;
            canAttack = false;
            isRunning = false;
            canRun = false;
            anim.Play("JumpBack");
            StartCoroutine(WaitForJumpEnd());
        }
    }

    private IEnumerator WaitForJumpEnd()
    {
        yield return new WaitForSeconds(0.5f);
        playerHitBox.enabled = true;
        isJumping = false;
        canJump = true;
        canAttack = true;
        canRun = true;
        canParry = true;
    }

    private void ApplyAttack()
    {
        if (canAttack && input.attackFast)
        {
            rb.velocity = Vector3.zero;
            PV.RPC("RPC_SetWeaponActive", RpcTarget.All, true);
            //weaponCol.enabled = true;
            canAttack = false;
            isAttacking = true;
            isRunning = false;
            canRun = false;
            Debug.Log("Attack Fast");
            anim.Play("AttackFast");
        }
    }
    [PunRPC]
    void RPC_SetWeaponActive(bool value)
    {
        weaponCol = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<BoxCollider>();

        if (value == true)
        {
            Debug.Log("Weapon activated");
            weaponCol.enabled = true;
        }
        else
        {
            Debug.Log("Weapon deactivated");
            weaponCol.enabled = false;
        }
    }

    private void ApplyParry()
    {
        if (canParry && input.parry)
        {
            rb.velocity = Vector3.zero;
            isParrying = true;
            canParry = false;
            weaponCol.enabled = false;
            canAttack = false;
            isAttacking = false;
            isRunning = false;
            canRun = false;
            //playerHitBox.enabled = false;
            anim.Play("Parry");
            StartCoroutine(WaitForParryEnd());
        }
    }
    private IEnumerator WaitForParryEnd()
    {
        yield return new WaitForSeconds(1f);
        //playerHitBox.enabled = true;
        isJumping = false;
        canJump = true;
        canAttack = true;
        canRun = true;
        isParrying = false;
        canParry = true;
    }
    private void ApplyMovement()
    {
        if (canRun && !isDead)
        {
            Vector3 moveInput = new Vector3(input.vector.x, 0, input.vector.y);
            rb.velocity = moveInput * runSpeed;
            if (input.vector != Vector2.zero)
            {
                isRunning = true;
            }
            else
            {
                rb.velocity = Vector3.zero;
                isRunning = false;
            }
        }
        else
        {
            //rb.velocity = Vector3.zero;
        }
    }

    public void SelfStun()
    {
        if (!isStunned)
        {
            LockPosition(true);
            weaponCol.enabled = false;
            isStunned = true;            
            isRunning = false;
            isJumping = false;
            isAttacking = false;
            canJump = false;
            canParry = false;            
            canAttack = false;            
            canRun = false;
            anim.Play("Stunned");
            StartCoroutine(WaitForStun());
        }
        
    }
    private IEnumerator WaitForStun()
    {
        yield return new WaitForSeconds(1.5f);
        LockPosition(false);
        isStunned = false;        
        isJumping = false;
        isAttacking = false;
        isRunning = false;
        canJump = true;
        canParry = true;        
        canAttack = true;        
        canRun = true;
    }
    public void LockPosition(bool value)
    {        
        if (value == true)
        {            
            if (!lockedPos)
            {
                if (target != null)
                {
                    oldTarget = target;
                }
                target = null;
                lockedPos = true;
                canRun = false;
                rb = GetComponent<Rigidbody>();

                rb.velocity = Vector3.zero;
            }            
        }
        if (value == false)
        {
            if (lockedPos)
            {
                target = oldTarget;
                canRun = true;
                lockedPos = false;
            }
        }
    }

    private void Death()
    {
        if(!isDead)
        {
            Debug.Log(gameObject.name + " is dead");
            //Destroy(this.gameObject, 3f);
            playerHitBox.enabled = false;
            LockPosition(true);
            target = null;
            isDead = true;
            canJump = false;
            canParry = false;
            canAttack = false;
            canRun = false;
            anim.Play("Death");
        }
        
    }
    [PunRPC]
    void InitStats(int h, int e)
    {
        Debug.Log("Health and Endurance Sets");
        health = h;
        endurance = e;
    }

    private void GetComponents()
    {
        input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
        anim = transform.GetComponent<Animator>();
        enduranceHandler = transform.GetComponent<EnduranceHandler>();
        playerParryBox = transform.GetChild(2).GetComponent<CapsuleCollider>();
        playerHitBox = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetComponent<CapsuleCollider>();
        weaponCol = transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<BoxCollider>();
        weaponCol.enabled = false;
        //rb = transform.GetChild(0).GetComponent<Rigidbody>();
        //anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        //enduranceHandler = transform.GetChild(0).GetChild(0).GetComponent<EnduranceHandler>();
        //playerParryBox = transform.GetChild(0).GetChild(1).GetComponent<CapsuleCollider>();
        //playerHitBox = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetComponent<CapsuleCollider>();
        //weaponCol = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<BoxCollider>();
    }

    
}
