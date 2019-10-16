using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;
    public int mySelectedWeapon;
    public GameObject[] allWeapons;

    private void OnEnable()
    {
        if (PlayerInfo.PI == null)
        {
            PlayerInfo.PI = this;
        }
        else
        {
            if (PlayerInfo.PI != this)
            {
                Destroy(PlayerInfo.PI.gameObject);
                PlayerInfo.PI = this;
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if(PlayerPrefs.HasKey("MyWeapon"))
        {
            mySelectedWeapon = PlayerPrefs.GetInt("MyWeapon");
        }
        else
        {
            mySelectedWeapon = 0;
            PlayerPrefs.SetInt("MyWeapon", mySelectedWeapon);
        }
    }

}
