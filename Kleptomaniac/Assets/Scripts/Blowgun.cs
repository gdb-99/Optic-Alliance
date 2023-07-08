using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowgun : Item {

    private EnemyPatrolling2 guardTarget;
    private AudioSource audioItem;
    private int numBullets = 5;

    private void Start()
    {
        audioItem = GetComponent<AudioSource>();
    }

    private void Update() {
        SearchForNearbyGuard();
    }

    public override void Use() {
        if(numBullets > 0 && guardTarget != null) {
            Debug.Log("Putting guard to sleep");
            audioItem.Play();
            PutGuardToSleep();
            numBullets--;
        } else {
            Debug.Log("No more ammo");
        }
        
    }

    private void PutGuardToSleep() {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);

        //foreach (Collider collider in colliders) {
        //    bool guardDetected = collider.TryGetComponent(out EnemyPatrolling2 guard);
        //    if (guardDetected) {
        //        guard.Sleep();
        //    }
        //}
        guardTarget?.Sleep();
    }

    private void SearchForNearbyGuard() {
        //Collider[] colliders = Physics.OverlapSphere(transform.position, itemSO.targetRange);
        Vector3 capsuleBase = new Vector3(transform.position.x, 0, transform.position.z);
        RaycastHit[] raycastHits = Physics.CapsuleCastAll(capsuleBase, capsuleBase + Vector3.up * 2, 2, transform.forward, itemSO.targetRange);

        foreach (RaycastHit raycastHit in raycastHits) {
            bool guardDetected = raycastHit.collider.TryGetComponent(out EnemyPatrolling2 guard);
            if (guardDetected) {
                Debug.Log("FOUND GUARD IN RANGE");
                guardTarget = guard;
                guardTarget.GetComponent<HighlightTarget>().ToggleHighlight(true);
                break;
            } else {
                guardTarget?.GetComponent<HighlightTarget>().ToggleHighlight(false);
                guardTarget = null;
            }
        }
    }

}