using System;
using System.Collections;
using UnityEngine;

public class ParryHandler : MonoBehaviour
{
    private PlayerController playerController;
    private CapsuleCollider parryBox;
    void Start()
    {
        playerController = this.transform.GetComponent<PlayerController>();
        parryBox = playerController.playerParryBox;
    }

    public void ParryBegin()
    {
        ActivateParry();
    }

    private void ActivateParry()
    {
        Debug.Log("Parry Frame started");
        parryBox.enabled = true;
        playerController.playerHitBox.enabled = false;
        StartCoroutine(WaitForParry());
    }

    private IEnumerator WaitForParry()
    {
        yield return new WaitForSeconds(0.075f);
        parryBox.enabled = false;
        playerController.playerHitBox.enabled = true;
        Debug.Log("Parry Frame ended");
    }
}
