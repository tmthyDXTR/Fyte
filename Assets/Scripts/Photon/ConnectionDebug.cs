using UnityEngine;
using TMPro;
using Photon.Pun;

public class ConnectionDebug : MonoBehaviour
{
    private PhotonView PV;
    private TextMeshProUGUI pingText;
    void Start()
    {
        pingText = GetComponent<TextMeshProUGUI>();       
    }

    void Update()
    {
        pingText.text = PhotonNetwork.GetPing().ToString() + " ms";
    }
}
