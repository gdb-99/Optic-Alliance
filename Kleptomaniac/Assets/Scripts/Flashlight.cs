using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item {

    private bool isSwitchedOn;
    private Transform spotlight;
    private AudioSource audioItem;

    private void Start() {
        isSwitchedOn = false;
        spotlight = transform.Find("Light");
        spotlight.gameObject.SetActive(isSwitchedOn);
        audioItem = GetComponent<AudioSource>();
    }

    public override void Use() {
        audioItem.Play();
        isSwitchedOn = !isSwitchedOn;
        spotlight.gameObject.SetActive(isSwitchedOn);
    }
}