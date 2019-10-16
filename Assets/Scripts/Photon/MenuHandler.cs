using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public void OnClickWeaponPick(int weaponInt)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedWeapon = weaponInt;
            PlayerPrefs.SetInt("MyWeapon", weaponInt);
        }
    }
}
