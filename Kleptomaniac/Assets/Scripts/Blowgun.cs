using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blowgun : Item {

    private EnemyPatrolling2 guardTarget;
    private AudioSource audioItem;
    private AudioSource notUsable;
    private int maxBullets;
    private int numBullets;

    private void Start()
    {
        maxBullets = itemSO.numberOfUses;
        numBullets = itemSO.numberOfUses;
        audioItem = GetComponents<AudioSource>()[0];
        notUsable = GetComponents<AudioSource>()[1];
    }

    private void Update() {
        SearchForNearbyGuard();
    }

    public override void Use() {
        if(numBullets > 0 && guardTarget != null) {
            audioItem.Play();
            PutGuardToSleep();
            numBullets--;
            playerItemController.NotifyItemCounterDecreased(numBullets, maxBullets);
        } else {
            notUsable.Play();
        }
        
    }

    private void PutGuardToSleep() {
        guardTarget?.Sleep();
    }

    private void SearchForNearbyGuard() {
        Vector3 capsuleBase = new Vector3(transform.position.x, 0, transform.position.z);
        RaycastHit[] raycastHits = Physics.CapsuleCastAll(capsuleBase, capsuleBase + Vector3.up * 2, 2, transform.forward, itemSO.targetRange);

        foreach (RaycastHit raycastHit in raycastHits) {
            bool guardDetected = raycastHit.collider.TryGetComponent(out EnemyPatrolling2 guard);
            if (guardDetected) {
                guardTarget = guard;
                guardTarget.GetComponent<HighlightTarget>().ToggleHighlight(true);
                break;
            } else {
                guardTarget?.GetComponent<HighlightTarget>().ToggleHighlight(false);
                guardTarget = null;
            }
        }
    }

    private void OnDisable() {
        if(guardTarget != null) {
            guardTarget?.GetComponent<HighlightTarget>().ToggleHighlight(false);
        }
        guardTarget = null;
    }

}