using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Walkman : Item {

    public bool isInCoolDown = false;
    private static float cooldownTime;
    private static Walkman activeWalkman;

    private bool isOn = false;
    private AudioSource audioItem;
    private AudioSource notUsable;
    private List<EnemyPatrolling2> distractedGuards = new List<EnemyPatrolling2>();



    private void Drop() {
        if(activeWalkman != null) {
            Debug.Log("DESTROYING ACTIVE WALKMAN");
            Destroy(activeWalkman.gameObject);
        }
        activeWalkman = this;
        transform.parent = null;
        Vector3 dropPosition = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = dropPosition;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        playerItemController.DropActiveItem();
    }

    public override void Use() {
        if(activeWalkman != null) {
            if (activeWalkman.isInCoolDown) {
                notUsable.Play();
                return;
            }
        }

        Drop();
        Debug.Log("Placing walkman to distract guards, it will play in 3 seconds");
        StartCoroutine(DelayedStartWalkman());
        isInCoolDown = true;
        playerItemController.StartCooldown(cooldownTime, () => { isInCoolDown = false; });
        
    }

    private void Start() {
        //if(activeWalkman == null) {
        //    activeWalkman = this;
        //}
        cooldownTime = 20f;
        isInCoolDown = false;
        audioItem = GetComponents<AudioSource>()[0];
        notUsable = GetComponents<AudioSource>()[1];
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
                distractedGuards.Add(guard);
                guard.Distract(transform.position);
                guard.OnDistractionReached += TurnOffWalkman;
            }
        }
    }

    private void TurnOffWalkman(object sender, System.EventArgs e) {
        isOn = false;
        foreach (EnemyPatrolling2 guard in distractedGuards) {
            guard.OnDistractionReached -= TurnOffWalkman;
        }
        Destroy(gameObject);
    }

    private void OnDestroy() {
        foreach(EnemyPatrolling2 guard in distractedGuards) {
            guard.OnDistractionReached -= TurnOffWalkman;
        }
    }

    IEnumerator DelayedStartWalkman() {
        audioItem.Play();
        yield return new WaitForSeconds(3f);
        Debug.Log("PLAYING !!!!");
        isOn = true;
    }


}
