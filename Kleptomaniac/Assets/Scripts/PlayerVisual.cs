using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    private Animator animator;
    private Player player;
    private const string IS_WALKING = "IsWalking";

    private void Start() {
        animator = GetComponent<Animator>();
        player = Player.Instance;
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }

}
