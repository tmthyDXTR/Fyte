using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private PlayerHandler myPlayer;
    private Rigidbody myRB;

    public float movementSpeed;

    [Header("Input")]
    public bool hasMovementInput;
    public float currentVelocity;
    public Vector2 vector;
    public bool jump;
    public bool parry;

    [Header("Debug")]
    [SerializeField] private bool drawDebugLine = true;


    void Start()
    {
        PV = GetComponent<PhotonView>();
        myPlayer = GetComponent<PlayerHandler>();
        myRB = GetComponent<Rigidbody>();
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
        if (vector != Vector2.zero)
        {
            ApplyMovement();
        }
        else
        {
            StopMovement();
        }
    }

    private void ApplyMovement()
    {
        hasMovementInput = true;
        //Move the rigidbody
        Vector3 lookDir = new Vector3(vector.x, 0, vector.y) * 50 - transform.position;
        Vector3 moveInput = new Vector3(vector.x, 0, vector.y);
        moveInput = moveInput.normalized * 75f * Time.deltaTime * movementSpeed;
        myRB.velocity = moveInput;
        currentVelocity = myRB.velocity.magnitude;
        transform.LookAt(lookDir);
    }

    public void StopMovement()
    {
        hasMovementInput = false;
        myRB.velocity = Vector3.zero;
        currentVelocity = 0.001f;
    }

    private void GetInput()
    {
        vector.x = Input.GetAxisRaw("P1_Horizontal");
        vector.y = Input.GetAxisRaw("P1_Vertical");
        //attack1 = Input.GetButtonDown("P1_Attack1");
        jump = Input.GetButtonDown("P1_Jump");
        parry = Input.GetButtonDown("P1_Parry");
    }

    private void DrawDebugLine()
    {
        if (!drawDebugLine)
        {
            return;
        }
        Debug.DrawRay(transform.position, new Vector3(vector.x, 0, vector.y) * 50f, Color.yellow);
    }
}
