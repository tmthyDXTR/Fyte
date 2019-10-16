using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public Weapon weapon;
    public Transform weaponCarrier;
    void OnEnable()
    {
        weaponCarrier = this.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent;
    }

    void Update()
    {
        
    }
}
