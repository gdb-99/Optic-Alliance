using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private Vector3 myDirection;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("A player object already exists");
        }
        Instance = this;
    }

    private void Start() {
        SetLookDirection();
    }

    private void Update() {

        SetLookDirection();

        Move();
        //Interact();

    }

    //private void Interact() {
    //    //float interactDistance = 2f;

    //    //if(Physics.Raycast(transform.position, myDirection, out RaycastHit raycastHit, interactDistance)) {
    //    //    if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
    //    //        if(baseCounter != selectedCounter) {
    //    //            SetSelectedCounter(baseCounter);
    //    //        }   
    //    //    } else {
    //    //        SetSelectedCounter(null);
    //    //    }
    //    //} else {
    //    //    SetSelectedCounter(null);
    //    //}
    //}

    private void Move() {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
       

        Vector3 moveDir = myDirection * inputVector.y + Quaternion.Euler(0, 90, 0) * myDirection * inputVector.x;
        moveDir = moveDir.normalized;

        float moveDistance = moveSpeed * Time.deltaTime;
        
        isWalking = moveDir != Vector3.zero;
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        if (CanMove(moveDir)) {
            transform.position += moveDir * moveDistance;
            return;
        }

        Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
        if (CanMove(moveDirX)) {
            transform.position += moveDirX * moveDistance;
            return;
        }

        Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
        if (CanMove(moveDirZ)) {
            transform.position += moveDirZ * moveDistance;
            return;
        }
        return;
    }

    private void SetLookDirection() {
        myDirection = Camera.main.transform.forward;
        myDirection.y = 0;
        myDirection = myDirection.normalized;
    }

    private bool CanMove(Vector3 moveDir) {
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        return canMove;
    }

    public bool IsWalking() {
        return isWalking;
    }

}
