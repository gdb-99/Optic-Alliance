using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetController : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5.0f;
    [SerializeField] Texture2D rotateCursor;

    bool isPointed = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPointed) {
            transform.Rotate(rotationSpeed * Time.deltaTime * new Vector3(0, Input.GetAxis("Mouse X"), 0), Space.Self);
        }
        else
        {
            transform.Rotate(Time.deltaTime * new Vector3(0, 5, 0), Space.Self);
        }
    }

    private void OnMouseDown()
    {
        if(Time.timeScale != 0.0f){
        isPointed = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        }
    }
    private void OnMouseUp()
    {
        isPointed = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnMouseEnter()
    {
        if(Time.timeScale != 0.0f){
            Cursor.SetCursor(rotateCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
