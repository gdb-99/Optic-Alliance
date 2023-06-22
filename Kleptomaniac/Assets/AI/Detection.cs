using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    CamRotation camRotationScript;

    string playerTag;

    Transform lens;

    // Start is called before the first frame update
    void Start()
    {
        camRotationScript = GetComponentInParent<CamRotation>();
        lens = transform.parent.GetComponent<Transform>();
        playerTag = GameObject.FindGameObjectWithTag("Player").tag;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col) //era OnTriggerStay
    {
        if (col.gameObject.CompareTag(playerTag))
        {
            if (camRotationScript != null)
            {
                camRotationScript.CurrentCameraState = CamRotation.CameraState.Aware;

                Vector3 direction = col.transform.position - lens.position;
                RaycastHit hit;

                if (Physics.Raycast(lens.transform.position, direction.normalized, out hit, 1000))
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider.gameObject.CompareTag(playerTag))
                    {
                        camRotationScript.CurrentCameraState = CamRotation.CameraState.Aware;
                        camRotationScript.SetPlayerTransform(hit.collider.transform);
                        Debug.Log(camRotationScript.CurrentCameraState);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        camRotationScript.CurrentCameraState = CamRotation.CameraState.Idle;
        Debug.Log(camRotationScript.CurrentCameraState);
    }
}
