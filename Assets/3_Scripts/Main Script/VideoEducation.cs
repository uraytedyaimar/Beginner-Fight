using System;
using System.Collections;
using UnityEngine;

public class VideoEducation : MonoBehaviour {
    public static VideoEducation Instance { get; private set; }

    // Component
    private Rigidbody2D rb;
    private Animator animator;

    // Game Input
    [SerializeField] private GameInputVideoEducation gameInput;

    // Bool
    [SerializeField] private bool salam;
    [SerializeField] private bool pembukaan;
    [SerializeField] private bool pukulan;
    [SerializeField] private bool tendangan;
    [SerializeField] private bool blokir;

    private const string SALAM = "Salam";
    private const string PEMBUKAAN = "Pembukaan";
    private const string PUKULAN = "Pukulan";
    private const string TENDANGAN = "Tendangan";
    private const string BLOKIR = "Blokir";

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        Salam();
        Pembukaan();
        Punch();
        Kick();
        Block();
    }

    private void Salam() {
        if(salam || gameInput.PlayerSalam()) {
            animator.SetBool(SALAM, true);
        } else {
            animator.SetBool(SALAM, false);
        }
    }

    private void Pembukaan() {
        if(pembukaan || gameInput.PlayerPembukaan()) {
            animator.SetBool(PEMBUKAAN, true);
        } else {
            animator.SetBool(PEMBUKAAN, false);
        }
    }

    private void Punch() {
        if(pukulan || gameInput.PlayerPunch()) {
            animator.SetBool(PUKULAN, true);
        } else {
            animator.SetBool(PUKULAN, false);
        }
    }

    private void Kick() {
        if(tendangan || gameInput.PlayerKick()) {
            animator.SetBool(TENDANGAN, true);
        } else {
            animator.SetBool(TENDANGAN, false);
        }
    }

    private void Block() {
        if(blokir || gameInput.PlayerBlock()) {
            animator.SetBool(BLOKIR, true);
        } else {
            animator.SetBool(BLOKIR, false);
        }
    }
}
