using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    Transform camGFX;


    bool startNextRotation = true;
    public bool rotRight;

    public float yaw;
    public float pitch;

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
                    StartCoroutine(Rotate(yaw, secondsToRot));
                }
                else if (startNextRotation && !rotRight)
                {
                    StartCoroutine(Rotate(-yaw, secondsToRot));
                }
                break;
            case CameraState.Aware:
                //QUI
                if(playerTransform != null)
                {
                    transform.LookAt(playerTransform);
                }
                break;

        }
        if(startNextRotation && rotRight)
        {
            StartCoroutine(Rotate(yaw, secondsToRot));
        }
        else if(startNextRotation && !rotRight)
        {
            StartCoroutine(Rotate(-yaw, secondsToRot));
        }
    }

    IEnumerator Rotate (float yaw, float duration)
    {
        startNextRotation = false;

        Quaternion initialRotation = transform.rotation;

        float timer = 0f;

        while(timer < duration)
        {
            timer += Time.deltaTime;
            transform.rotation = initialRotation * Quaternion.AngleAxis(timer / duration * yaw, Vector3.up);
            yield return null;
        }

        yield return new WaitForSeconds(rotSwitchTime);

        startNextRotation = true;
        rotRight = !rotRight;
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
