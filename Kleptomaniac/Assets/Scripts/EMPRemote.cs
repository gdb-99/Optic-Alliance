using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRemote : Item {

    private static bool isInCoolDown;
    private static float cooldownTime;

    private CamRotation cctvTarget;
    private AudioSource audioItem;
    private AudioSource notUsable;

    private void Start()
    {
        isInCoolDown = false;
        cooldownTime = 20f;
        audioItem = GetComponents<AudioSource>()[0];
        notUsable = GetComponents<AudioSource>()[1];
    }

    private void Update() {
        SearchForNearbyCCTV();
    }


    public override void Use() {
        if (isInCoolDown || cctvTarget == null) {
            notUsable.Play();
        } else {
            GameManager.Instance.quest3 = false;
            audioItem.Play();
            Debug.Log("Disabilitating surveillance CCTV");
            TurnOffNearbyCCTV();
            isInCoolDown = true;
            playerItemController.StartCooldown(cooldownTime, () => { EMPRemote.isInCoolDown = false; });
        }
        
    }

    private void TurnOffNearbyCCTV() {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);

        //foreach (Collider collider in colliders) {
        //    bool cctvDetected = collider.TryGetComponent(out CamRotation cctv);
        //    if (cctvDetected) {
        //        //STOP THE CAMERA FROM SURVEILLING
        //        Debug.Log("FOUND CCTV CAMERA IN RANGE");
        //    }
        //}
        Debug.Log("TURN OFF TARGET = " + cctvTarget);
        cctvTarget?.DisableCamera();
    }

    private void SearchForNearbyCCTV() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);

        foreach (Collider collider in colliders) {
            bool cctvDetected = collider.TryGetComponent(out CamRotation cctv);
            if (cctvDetected) {
                Debug.Log("FOUND CCTV CAMERA IN RANGE");
                cctvTarget = cctv;
                cctvTarget.GetComponent<HighlightTarget>().ToggleHighlight(true);
                break;
            } else {
                cctvTarget?.GetComponent<HighlightTarget>().ToggleHighlight(false);
                cctvTarget = null;
            }
        }
    }

    private void OnDisable() {
        if(cctvTarget != null) {
            cctvTarget?.GetComponent<HighlightTarget>().ToggleHighlight(false);
        }
        cctvTarget = null;
    }

}
