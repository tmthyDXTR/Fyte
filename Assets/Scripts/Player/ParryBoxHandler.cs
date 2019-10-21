using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryBoxHandler : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    private PlayerParryBox PPB;
    public CapsuleCollider parryBoxCol;

    public bool isBusy = false;

    void Start()
    {
        PPB = PV.transform.GetComponent<PlayerParryBox>();
        parryBoxCol = GetComponent<CapsuleCollider>();
    }

    
}