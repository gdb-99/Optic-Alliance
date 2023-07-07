using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPRemote : Item {

    private CamRotation cctvTarget;

    private void Update() {
        SearchForNearbyCCTV();
    }


    public override void Use() {
        Debug.Log("Disabilitating surveillance CCTV");
        TurnOffNearbyCCTV();
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

}
