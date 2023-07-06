using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item {

    private bool isSwitchedOn = false;

    public override void Use() {
        isSwitchedOn = !isSwitchedOn;
        Debug.Log("Flashlight ON = " + isSwitchedOn);
    }
}