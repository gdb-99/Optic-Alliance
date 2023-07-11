using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBarSlot : MonoBehaviour {
    private bool isUsable = true;

    private void Start() {
        isUsable = true;
    }

    public void SetIsUsable(bool isUsable) {
        this.isUsable = isUsable;
    }

    public bool GetIsUsable() {
        return isUsable;
    }

}
