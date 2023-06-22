using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    Transform camGFX;


    bool startNextRotation = true;
    public bool rotRight;
    public Quaternion initialRotation;
    public Quaternion startPoint;
    public Quaternion endPoint;
    [SerializeField]  public float yaw;
    public float pitch;
    public float stopTime;
    public float secondsToRot;
    public float rotSwitchTime;
    private Transform playerTransform;

    public CameraState CurrentCameraState { get; set; }

    public enum CameraState
    {
        Idle,
        Aware
    }

    // Start is called before the first frame update
    void Start()
    {
        camGFX = transform.GetChild(0);
        camGFX.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
        startPoint = transform.rotation;
        endPoint = Quaternion.Euler(0, yaw, 0) * startPoint;
        SetUpStartRotation();
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(CurrentCameraState);
        switch (CurrentCameraState)
        {
            case CameraState.Idle:

                if (startNextRotation && rotRight)
                {
                    StartCoroutine(Rotate(yaw, secondsToRot, startPoint));
                }
                else if (startNextRotation && !rotRight)
                {
                    StartCoroutine(Rotate(-yaw, secondsToRot, endPoint));
                }
                break;
            case CameraState.Aware:
                //QUI

                if(playerTransform != null)
                {
                    StopAllCoroutines();
                    transform.LookAt(playerTransform);
                    startNextRotation = true;
                    //tornaAllAngoloDiPartenza();
                }
                break;

        }
    }



    IEnumerator Rotate (float yaw, float duration, Quaternion initAngle)
    {

        if (CurrentCameraState == CameraState.Idle)
        {
            startNextRotation = false;

            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                transform.rotation = initAngle * Quaternion.AngleAxis(timer / duration * yaw, Vector3.up);
                yield return null;
            }

            yield return new WaitForSeconds(rotSwitchTime);

            startNextRotation = true;
            rotRight = !rotRight;
        }
    }

    void SetUpStartRotation()
    {
        if(rotRight)
        {
            transform.forward = Quaternion.Euler(0, -yaw / 2, 0) * transform.forward;
        }
        else
        {
            transform.forward = Quaternion.Euler(0, yaw / 2, 0) * transform.forward;
        }
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

}
