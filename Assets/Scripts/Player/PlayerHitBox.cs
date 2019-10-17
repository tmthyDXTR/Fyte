using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private PhotonView PV;
    [SerializeField] private HitBoxHandler hitBox;
    [SerializeField] private PlayerHandler PH;
    
    public WeaponInfo hitting;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PH = GetComponent<PlayerHandler>();
    }

    [PunRPC]
    void RPC_GetHit(int damage)
    {
        Debug.Log(PV.ViewID + " got hit by " + hitting.name + " for " + hitting.damage + " Damage");
        PH.health -= damage;
        PH.isHit = true;
        hitting = null;
        StartCoroutine(WaitForHit());
    }

    private IEnumerator WaitForHit()
    {
        yield return new WaitForSeconds(1f);
        hitBox.isBusy = false;
        PH.isHit = false;
    }
}
