using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private PlayerHandler PH;
    private PlayerHitBox PHB;

    private Rigidbody myRB;
    private Animator myAnim;
    private Transform cam;

    public float movementSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    [Header("Input")]
    public bool hasMovementInput;
    public float currentVelocity;
    public Vector2 input;
    public bool jump;


    public float Angle;
    private Quaternion targetRotation;

    [Header("Debug")]
    [SerializeField] private bool drawDebugLine = true;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
        PHB = GetComponent<PlayerHitBox>();
        myRB = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (PV.IsMine)
        {
            GetInput();

            DrawDebugLine();
        }
    }

    public void Move()
    {        
        if (input != Vector2.zero)
        {
            PV.RPC("RPC_ApplyMovement", RpcTarget.All);
        }
        else
        {
            PV.RPC("RPC_StopMovement", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_ApplyJump()
    {
        PHB.hitBox.enabled = false;
        PV.RPC("RPC_RemoveStat", RpcTarget.AllBuffered, "Stamina", 1);

        // If Jumping in a specific direction
        if (input != Vector2.zero)
        {
            // Jump in input direction
            myRB.AddForce((Vector3.ClampMagnitude(new Vector3(input.x, 0, input.y), 1f) * 100) * jumpSpeed * 10);
            
            // Jump in current look direction
            //myRB.AddRelativeForce((Vector3.forward * 100) * jumpSpeed * 10);

            Angle = Mathf.Atan2(input.x, input.y);
            Angle = Mathf.Rad2Deg * Angle;
            targetRotation = Quaternion.Euler(0, Angle, 0);
            myRB.rotation = targetRotation;
        }
        // Jump back
        else
        {
            myRB.AddRelativeForce((Vector3.back * 100) * jumpSpeed * 10);
            PH.myAnim.Play("2Hand-Sword-Roll-Backward");
        }
        PH.isJumping = true;
        PH.isAttacking = false;
        PH.isRunning = false;

        PH.canJump = false;
        PH.canAttack = false;
        PH.canRun = false;
        PH.canParry = false;
        PH.myAnim.SetBool("IsAttacking", false);
        PH.myAnim.SetBool("IsJumping", true);
        StartCoroutine(WaitForJump(0.25f));
    }

    private IEnumerator WaitForJump(float time)
    {
        yield return new WaitForSeconds(time);
        PH.isJumping = false;
        PH.isAttacking = false;
        PH.isRunning = false;

        PH.canJump = true;
        PH.canAttack = true;
        PH.canRun = true;
        PH.canParry = true;
        PH.myAnim.SetBool("IsJumping", false);
    }
    [PunRPC]
    void RPC_ApplyMovement()
    {
        hasMovementInput = true;
        //Rigidbody movement
        Vector3 lookDir = new Vector3(input.x, 0, input.y) * 1000f - transform.position;
        Vector3 moveInput = new Vector3(input.x, 0, input.y);
        moveInput = Vector3.ClampMagnitude(moveInput, 1.0f) * 50f *Time.fixedDeltaTime * movementSpeed;
        myRB.velocity = moveInput;
        currentVelocity = myRB.velocity.magnitude;
        //Rigidbody Rotation
        Angle = Mathf.Atan2(input.x, input.y);
        Angle = Mathf.Rad2Deg * Angle;
        targetRotation = Quaternion.Euler(0, Angle, 0);
        myRB.MoveRotation(Quaternion.Slerp(transform.localRotation,
            Quaternion.Slerp(transform.localRotation, targetRotation, Time.fixedDeltaTime * rotationSpeed),
            Time.fixedDeltaTime * rotationSpeed));

    }
    [PunRPC]
    void RPC_StopMovement()
    {
        hasMovementInput = false;
        myRB.velocity = Vector3.zero;
        myRB.angularVelocity = Vector3.zero;
        currentVelocity = 0f;

        PH.isAttacking = false;
        PH.isRunning = false;
        PH.myAnim.SetBool("IsRunning", false);
        PH.myAnim.SetBool("IsAttacking", false);
    }

    private void GetInput()
    {
        input.x = Input.GetAxisRaw("P1_Horizontal");
        input.y = Input.GetAxisRaw("P1_Vertical");

        //attack1 = Input.GetButtonDown("P1_Attack1");
        jump = Input.GetButtonDown("P1_Jump");
    }

    private void DrawDebugLine()
    {
        if (!drawDebugLine)
        {
            return;
        }
        Debug.DrawRay(transform.position, Vector3.ClampMagnitude(new Vector3(input.x, 0, input.y), 1f) * 5f, Color.yellow);
    }
}
