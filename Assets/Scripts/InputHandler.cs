using System;
using Photon.Pun;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private PhotonView PV;

    [Header("Input")]
    public Vector2 vector;
    public bool attackFast;
    public bool jumpBack;
    public bool parry;
    public Player player;
    public enum Player
    {
        One,
        Two,
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (PV.IsMine)
        {
            GetMovementInput();
            DrawDebugLine();
        }
    }

    private void DrawDebugLine()
    {
        Debug.DrawRay(transform.position, new Vector3(vector.x, 0, vector.y) * 50f, Color.yellow);
    }

    private void GetMovementInput()
    {
        if (player == Player.One)
        {
            vector.x = Input.GetAxisRaw("P1_Horizontal");
            vector.y = Input.GetAxisRaw("P1_Vertical");

            attackFast = Input.GetButtonDown("P1_AttackFast");
            jumpBack = Input.GetButtonDown("P1_JumpBack");
            parry = Input.GetButtonDown("P1_Parry");
        }
        if (player == Player.Two)
        {
            vector.x = Input.GetAxisRaw("P2_Horizontal");
            vector.y = Input.GetAxisRaw("P2_Vertical");

            attackFast = Input.GetButtonDown("P2_AttackFast");
            jumpBack = Input.GetButtonDown("P2_JumpBack");
            parry = Input.GetButtonDown("P2_Parry");
        }
    }
}