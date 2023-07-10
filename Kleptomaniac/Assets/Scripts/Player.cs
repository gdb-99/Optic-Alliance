using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject interactPanel;

    private PlayerItemController playerItemController;

    private bool isWalking;
    private float playerRadius = .7f;
    private float playerHeight = 2f;
    private Vector3 myDirection;

    private Interactable selectedInteractable;

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("A player object already exists");
        }
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        playerItemController = GetComponent<PlayerItemController>();
        SetLookDirection();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnUseAction += GameInput_OnUseAction;
        gameInput.OnSwitchItem += GameInput_OnSwitchItem;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        selectedInteractable?.Interact();
        // throw new NotImplementedException();
    }

    private void GameInput_OnSwitchItem(object sender, GameInput.OnSwitchItemEventArgs e) {
        playerItemController.SwitchActiveItem(e.itemIndex);
    }

    private void GameInput_OnUseAction(object sender, EventArgs e) {
        playerItemController.UseActiveItem();
    }

    private void Update() {

        SetLookDirection();

        Move();
        Interact();

    }

    private void Interact()
    {
        float interactDistance = 2f;

        RaycastHit[] raycastHits = Physics.CapsuleCastAll(transform.position, transform.position + Vector3.up * 2 * playerHeight, playerRadius, myDirection, interactDistance);

        if (raycastHits.Length == 0)
        {
            selectedInteractable = null;
        }
        else
        {
            foreach (RaycastHit raycastHit in raycastHits)
            {
                if (raycastHit.transform.gameObject.TryGetComponent(out Interactable interactable))
                {
                    if (interactable != selectedInteractable)
                    {
                        selectedInteractable = interactable;
                        interactPanel.SetActive(true);
                        Debug.Log("FOUND INTERACTABLE");
                    }
                    break;
                }
                else
                {
                    selectedInteractable = null;
                    interactPanel.SetActive(false);
                }
            }
        }
    }

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
        if (!isWalking) {
            transform.forward = Vector3.Slerp(transform.forward, myDirection, Time.deltaTime * rotateSpeed);
        }
    }

    private bool CanMove(Vector3 moveDir) {
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance, -1, QueryTriggerInteraction.Ignore);
        return canMove;
    }

    public bool IsWalking() {
        return isWalking;
    }

}
