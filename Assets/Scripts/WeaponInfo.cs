using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public Weapon weapon;
    public Transform weaponOwner;

    public int damage;
    public float attackSpeed;
    void OnEnable()
    {
        weaponOwner = this.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent;
    }

    void Start()
    {
        damage = weapon.damage;
        attackSpeed = weapon.attackSpeed;
    }
}
