using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    [SerializeField] protected ItemSO itemSO;
    protected PlayerItemController playerItemController;

    public abstract void Use();

    public void SetPlayerItemController(PlayerItemController playerItemController) {
        this.playerItemController = playerItemController;
    }

}