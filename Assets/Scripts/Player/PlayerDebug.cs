using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour
{
    private PhotonView PV;
    private PlayerHandler PH;
    private PlayerAttack PA;
    public bool attack = false;
    public float pause = 2.5f;
    public Transform target;
    private bool isAttacking = false;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
        PA = GetComponent<PlayerAttack>();
    }

    void ToggleAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            //Start attack
            PA.attack1 = true;
            StartCoroutine(WaitClick());
            StartCoroutine(Wait(pause));
        }
        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }
    void Update()
    {
        if (attack)
        {
            ToggleAttack();
        }
    }
    private IEnumerator WaitClick()
    {
        yield return new WaitForSeconds(0.01f);
        PA.attack1 = false;
    }
    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }
}
