using System.Collections;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    PlayerController playerController;
    Animator anim;
    public GameObject hittingObject;
    public int damage = 1;

    public delegate void OnPlayer1Hit();
    public static event OnPlayer1Hit OnP1DamageTaken;

    public delegate void OnPlayer2Hit();
    public static event OnPlayer2Hit OnP2DamageTaken;

    public Player player;
    public enum Player
    {
        One,
        Two,
    }
    void Start()
    {
        anim = this.transform.parent.parent.parent.parent.parent.parent.GetComponent<Animator>();
        playerController = transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Weapon Layer 10
        {
            if (!playerController.isHit)
            {
                GetHit(other);
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10) // Weapon Layer 10
        {
            if (!playerController.isHit)
            {
                GetHit(other);
            }

        }
    }

    private void GetHit(Collider other)
    {
        playerController.health -= damage;
        if (player == Player.One)
        {
            if (OnP1DamageTaken != null) OnP1DamageTaken();
        }
        if (player == Player.Two)
        {
            if (OnP2DamageTaken != null) OnP2DamageTaken();
        }
        playerController.isHit = true;
        anim.Play("HitFront");
        playerController.LockPosition(true);
        playerController.canRun = false;
        playerController.canJump = false;
        playerController.canParry = false;
        playerController.canAttack = false;
        Debug.Log("Hit by " + other.gameObject.name + " for " + damage + " Damage");
        StartCoroutine(WaitAfterHit());
    }

    IEnumerator WaitAfterHit()
    {        
        yield return new WaitForSeconds(1f);
        playerController.LockPosition(false);
        playerController.isHit = false;
        playerController.canRun = true;
        playerController.canJump = true;
        playerController.canParry = true;
        playerController.canAttack = true;
    }

}