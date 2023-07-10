using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Detection : MonoBehaviour
{
    CamRotation camRotationScript;
    [SerializeField] private List<EnemyPatrolling2> reactingGuards = new List<EnemyPatrolling2>();

    private AudioSource audioItem;
    private bool isPlayerDetected;
    string playerTag;

    public event EventHandler<OnSwitchItemEventArgs> OnPlayerDetected;
    public class OnSwitchItemEventArgs : EventArgs
    {
        public Detection camera;
        public Transform playerTransform;
    }

    Transform lens;

    // Start is called before the first frame update
    void Start()
    {
        audioItem = GetComponent<AudioSource>();
        camRotationScript = GetComponentInParent<CamRotation>();
        lens = transform.parent.GetComponent<Transform>();
        playerTag = GameObject.FindGameObjectWithTag("Player").tag;
        isPlayerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider col) //era OnTriggerStay
    {
        if (col.gameObject.CompareTag(playerTag))
        {
            Debug.Log("PLAYER ENTERED COLLIDER");
            if (camRotationScript != null)
            {
                //camRotationScript.CurrentCameraState = CamRotation.CameraState.Aware;

                Vector3 direction = col.transform.position - lens.position;
                RaycastHit hit;

                if (Physics.Raycast(lens.transform.position, direction.normalized, out hit, 1000))
                {
                    Debug.Log("OBJECT RAYCASTED = " + hit.collider.name);

                    if (hit.collider.gameObject.CompareTag(playerTag))
                    {
                        Debug.Log("PLAYER HAS BEEN DETECTED");
                        //camRotationScript.CurrentCameraState = CamRotation.CameraState.Aware;
                        camRotationScript.SetPlayerTransform(hit.collider.transform);
                        Debug.Log(camRotationScript.CurrentCameraState);
                        if (OnPlayerDetected != null)
                        {
                            OnPlayerDetected.Invoke(this, new OnSwitchItemEventArgs { camera = this, playerTransform = hit.collider.transform }); // + codice univoco
                            camRotationScript.PlayerDetected();
                            if (!isPlayerDetected) {
                                GameManager.Instance.quest2 = false;
                                audioItem.Play();
                                isPlayerDetected = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        camRotationScript.PlayerNoLongerDetected();
        isPlayerDetected = false;
        //camRotationScript.CurrentCameraState = CamRotation.CameraState.Idle;
        //camRotationScript.PlayerDetected();
        //Debug.Log(camRotationScript.CurrentCameraState);
    }

    public List<EnemyPatrolling2> GetReactingGuards()
    {
        return reactingGuards;
    }

    public void AddReactingGuard(EnemyPatrolling2 guard)
    {
        reactingGuards.Add(guard);
    }

    public void RemoveReactingGuard(EnemyPatrolling2 guard)
    {
        reactingGuards.Remove(guard);
    }
}