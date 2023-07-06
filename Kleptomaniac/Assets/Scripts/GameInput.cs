using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

    public event EventHandler OnInteractAction;
    public event EventHandler OnUseAction;
    public event EventHandler<OnSwitchItemEventArgs> OnSwitchItem;

    public class OnSwitchItemEventArgs : EventArgs {
        public int itemIndex;
    }

    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Use.performed += Use_performed;
        playerInputActions.Player.SwitchItem.performed += SwitchItem_performed;
        //playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
    }

    //private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
    //    OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    //}

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void SwitchItem_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnSwitchItem?.Invoke(this, new OnSwitchItemEventArgs {
            itemIndex = (int)obj.ReadValue<float>()
        });
    }

    private void Use_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnUseAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector.normalized;
    }

}
