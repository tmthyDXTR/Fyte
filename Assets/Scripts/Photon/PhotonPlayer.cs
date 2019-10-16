using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(PV.IsMine)
        {
            // First player spawns at left spawn point
            int spawn;
            if (PV.ViewID == (int)1001)
            {
                spawn = 0;
                CreateAvatar(spawn);
                PV.RPC("SetPlayerName", RpcTarget.AllBuffered, "Player1");
            }
            // Secondd player at right spawn point
            else
            {
                spawn = 1;
                CreateAvatar(spawn);
                PV.RPC("SetPlayerName", RpcTarget.AllBuffered, "Player2");
            }


            //myAvatar.GetComponent<InputHandler>().enabled = true;
            //myAvatar.GetComponent<PlayerController>().enabled = true;
            //myAvatar.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            //myAvatar.transform.GetChild(0).GetComponent<AttackHandler>().enabled = true;
            //myAvatar.transform.GetChild(0).GetComponent<EnduranceHandler>().enabled = true;
            //myAvatar.transform.GetChild(0).GetComponent<ParryHandler>().enabled = true;
            //myAvatar.transform.GetChild(1).GetComponent<ParryBox>().enabled = true;
            //myAvatar.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(3).GetComponent<HitBox>().enabled = true;
            //myAvatar.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetComponent<WeaponInfo>().enabled = true;
        }
    }
    [PunRPC]
    void SetPlayerName(string name)
    {        
        myAvatar.name = name;        
    }

    private void CreateAvatar(int spawn)
    {
        myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                        GameSetup.GS.spawnPoints[spawn].position,
                        GameSetup.GS.spawnPoints[spawn].rotation,
                        0);
    }
}
