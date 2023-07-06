using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkman : Item {

    private bool isOn = false;

    private void Drop() {
        transform.parent = null;
        Vector3 dropPosition = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = dropPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        playerItemController.DropActiveItem();
    }

    public override void Use() {
        Drop();
        isOn = true;
        Debug.Log("Placing walkman to distract guards");
    }

    private void Update() {
        if (isOn) {
            DistractGuards();
        }
    }

    private void DistractGuards() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);

        foreach (Collider collider in colliders) {
            bool guardDetected = collider.TryGetComponent(out EnemyPatrolling2 guard);
            if (guardDetected) {
                Debug.Log("GUARD ENTERED WALKMAN RANGE");
                guard.Distract(transform.position);
                guard.OnDistractionReached += TurnOffWalkman;
            }
        }
    }

    private void TurnOffWalkman(object sender, System.EventArgs e) {
        isOn = false;
    }

}
