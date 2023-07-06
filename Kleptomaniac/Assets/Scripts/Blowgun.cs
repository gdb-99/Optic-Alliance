using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowgun : Item {
    public override void Use() {
        Debug.Log("Putting guard to sleep");
        PutGuardToSleep();
    }

    private void PutGuardToSleep() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);

        foreach (Collider collider in colliders) {
            bool guardDetected = collider.TryGetComponent(out EnemyPatrolling2 guard);
            if (guardDetected) {
                guard.Sleep();
            }
        }
    }

}