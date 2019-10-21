using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelHandler : MonoBehaviour
{
    private PhotonView PV;
    public PlayerHandler PH;
    public Image hpBar;
    public Image stBar;
    public TextMeshProUGUI playerNameText;
    public string playerName;

    void OnEnable()
    {

    }

    void Update()
    {
        if (PH != null)
        {
            UpdateHP();
            UpdateST();
        }
    }

    void UpdateHP()
    {
        hpBar.rectTransform.localScale = new Vector3(Mathf.Clamp01((float)PH.health / (float)PH.healthMax), 1, 1);
    }
    void UpdateST()
    {
        stBar.rectTransform.localScale = new Vector3(Mathf.Clamp01((float)PH.stamina / (float)PH.staminaMax), 1, 1);
    }
}
