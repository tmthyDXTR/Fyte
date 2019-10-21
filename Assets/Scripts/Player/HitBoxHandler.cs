using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxHandler : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    private PlayerHitBox PHB;
    public CapsuleCollider hitBox;
    public bool isBusy = false;

    void Start()
    {
        PHB = PV.transform.GetComponent<PlayerHitBox>();
        hitBox = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // Weapon Layer
        {
            if (!isBusy)
            {
                //Debug.Log(PV.ViewID + " got hit by " + other.gameObject.name);
                isBusy = true;
                PHB.hitting = other.GetComponent<WeaponInfo>();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 10) // Weapon Layer
        {
            if (!isBusy)
            {
                //Debug.Log(PV.ViewID + " got hit by " + other.gameObject.name);
                isBusy = true;
                PHB.hitting = other.GetComponent<WeaponInfo>();
            }
        }
    }
}