using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryBox : MonoBehaviour
{
    [SerializeField] private bool isBusy;
    PlayerController playerController;

    [SerializeField] private Transform hitter;
    [SerializeField] PlayerController hitterController;

    void OnEnable()
    {
        playerController = this.transform.parent.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isBusy)
        {
            if (other.gameObject.layer == 10) // Weapon Layer 10 
            {
                this.isBusy = true;
                hitter = other.GetComponent<WeaponInfo>().weaponCarrier;
                hitterController = hitter.GetComponent<PlayerController>();
                hitterController.SelfStun();
                this.isBusy = false;
            }
        }        
    }
    //void OnTriggerStay(Collider other)
    //{
    //    if (!isBusy)
    //    {
    //        if (other.gameObject.layer == 10) // Weapon Layer 10 
    //        {
    //            this.isBusy = true;
    //            hitter = other.GetComponent<WeaponInfo>().weaponCarrier;
    //            hitterController = hitter.GetComponent<PlayerController>();
    //            hitterController.SelfStun();
    //            this.isBusy = false;
    //        }
    //    }
    //}
}
