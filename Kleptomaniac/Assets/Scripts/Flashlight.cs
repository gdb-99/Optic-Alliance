using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item {

    private bool isSwitchedOn = false;
    private Transform spotlight;

    private void Start() {
        spotlight = transform.Find("Light");
        spotlight.gameObject.SetActive(isSwitchedOn);
    }

    public override void Use() {
        isSwitchedOn = !isSwitchedOn;
        spotlight.gameObject.SetActive(isSwitchedOn);
    }
}