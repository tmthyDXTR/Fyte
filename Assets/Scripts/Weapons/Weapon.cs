using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Weapons")]
public class Weapon : ScriptableObject
{
    public string name;
    public int damage;
    public float attackSpeed;

}
