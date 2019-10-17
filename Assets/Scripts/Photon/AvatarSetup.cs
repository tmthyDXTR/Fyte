using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public string playerName;
    public int weaponValue;
    public GameObject myWeapon;
    //private PlayerController playerController;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        if (PV.IsMine)
        {
            //playerController = GetComponent<PlayerController>();
            PV.RPC("RPC_AddWeapon", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedWeapon);
            PV.RPC("RPC_ChangeObjName", RpcTarget.AllBuffered, PlayerInfo.PI.myPlayerName);
        }
    }
    [PunRPC]
    void RPC_ChangeObjName(string name)
    {
        playerName = name;
    }
    [PunRPC]
    void RPC_AddWeapon(int weaponInt)
    {
        weaponValue = weaponInt;
        myWeapon = Instantiate(PlayerInfo.PI.allWeapons[weaponInt],
            transform.position,
            transform.rotation,
            transform);
        //playerController.enabled = true;
        //PV.ObservedComponents.Clear();
        ////PV.ObservedComponents.Add(myWeapon.GetComponent<PhotonRigidbodyView>());
        //PV.ObservedComponents.Add(myWeapon.GetComponent<PhotonTransformView>());
        //PV.ObservedComponents.Add(myWeapon.transform.GetChild(0).GetComponent<PhotonAnimatorView>());
    }
}
