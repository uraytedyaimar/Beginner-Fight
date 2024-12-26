using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IGetHealthSystem {
    public static Player Instance { get; private set; }

    // Const
    private const int PLAYER_HITBOX = 0;

    // Component
    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private CharacterAnimation characterAnimation;
    // [SerializeField] private ComboManager comboManager;

    // Status
    private bool isDead = false;

    // Game Input
    [SerializeField] private GameInput gameInput;
    private Vector2 moveInputVector;

    // Move
    [SerializeField] private float moveSpeed;

    // Hitstop
    private Coroutine hitStopCoroutine;
    private float lastHitTime;
    private float hitStopDuration = 0.2f; // Duration of hitstop in seconds

    // Dash
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTime;
    private Vector2 dashDirection;

    // Reset
    private Vector2 initialPosition = new Vector2(-2.5f, -2.138f);

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        characterAnimation = GetComponent<CharacterAnimation>();
        healthSystem = new HealthSystem(100);
    }

    private void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDead += HealthSystem_OnDead;
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;

        HitboxController.Instance.DeactivateHitbox(PLAYER_HITBOX);
    }

    private void GameHandler_OnStateChanged( object sender, EventArgs e ) {
        if ( GameHandler.Instance.IsWaitingToGameOver() == true || isDead == true ) {
            rb.velocity = Vector2.zero;
        }
    }

    private void Update() {
        if (GameHandler.Instance.IsGamePlaying() == false || isDead == true) {
            rb.velocity = Vector2.zero;
            return;
        }

        moveInputVector = gameInput.GetPlayerMovement();

        Punch();
        Kick();
        Block();
        Dash();

        if (isDashing == true) {
            DashMovement();
        }
    }

    private void FixedUpdate() {
        if (GameHandler.Instance.IsGamePlaying() == false || isDead == true) {
            rb.velocity = Vector2.zero;
            return;
        }

        HandleMovements();
    }

    private void HealthSystem_OnDamaged( object sender, System.EventArgs e ) {
        if (IsBlocking() == false) {
            lastHitTime = Time.time;
            if (hitStopCoroutine == null) {
                hitStopCoroutine = StartCoroutine(HitStopAnimation());
            }
        }
    }

    private void HealthSystem_OnDead( object sender, System.EventArgs e ) {
        if (isDead) return;

        isDead = true;
        characterAnimation.ResetAnimationState();
        characterAnimation.IsDead();
        RoundManager.Instance.EndRound(2);
    }

    private void HandleMovements() {
        if (isDashing == true || isDead == true || GameHandler.Instance.IsGamePlaying() == false) {
            return;
        }

        if (!characterAnimation.IsPunching() && !characterAnimation.IsKicking() && !characterAnimation.IsBlocking()) {
            rb.velocity = new Vector2(moveInputVector.x * moveSpeed, rb.velocity.y);
            characterAnimation.Walk(moveInputVector.x);
        } else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Punch() {
        if (gameInput.PlayerPunch() && !characterAnimation.IsPunching() && !characterAnimation.IsKicking() && !gameInput.PlayerBlock() && isDashing == false) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            characterAnimation.StartPunching();
        }
    }

    private void Kick() {
        if (gameInput.PlayerKick() && !characterAnimation.IsKicking() && !characterAnimation.IsPunching() && !gameInput.PlayerBlock() && isDashing == false) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            characterAnimation.StartKicking();
        }
    }

    private void Block() {
        if (gameInput.PlayerBlock()) {
            rb.velocity = new Vector2(0, rb.velocity.y);
            characterAnimation.StartBlocking();
        } else {
            characterAnimation.StopBlocking();
        }
    }

    private void Dash() {
        if (gameInput.PlayerDash() && isDashing == false && !characterAnimation.IsPunching() && !characterAnimation.IsKicking() && !gameInput.PlayerBlock()) {
            StartDash();
        }
    }

    private void StartDash() {
        isDashing = true;
        dashTime = dashDuration;
        dashDirection = new Vector2(moveInputVector.x, rb.velocity.y);
        characterAnimation.Dash(moveInputVector.x);
    }

    private void DashMovement() {
        if (dashTime > 0) {
            rb.velocity = dashDirection * dashSpeed;
            dashTime -= Time.deltaTime;
        } else {
            rb.velocity = Vector2.zero;
            isDashing = false;
        }
    }

    public void Damage( float damage ) {
        healthSystem.Damage(damage);
    }

    public bool IsBlocking() {
        return characterAnimation.IsBlocking();
    }

    private IEnumerator HitStopAnimation() {
        characterAnimation.IsDamaged();
        yield return new WaitForSeconds(hitStopDuration);
        hitStopCoroutine = null; // Reset coroutine flag after hitstop duration
    }

    public bool Die() {
        return isDead;
    }

    public void ResetStatus() {
        // Reset Status Hidup/Mati
        isDead = false;
        // Reset Darah
        healthSystem.HealComplete();
        // Reset Animasi
        characterAnimation.ResetAnimationState();
        // Reset Posisi
        rb.position = initialPosition;
        // Reset Input
        moveInputVector = Vector2.zero;
    }

    public void ActiveHitbox() {
        HitboxController.Instance.ActivateHitbox(PLAYER_HITBOX);
    }

    public void DeactiveHitbox() {
        HitboxController.Instance.DeactivateHitbox(PLAYER_HITBOX);
    }

    public HealthSystem GetHealthSystem() {
        return healthSystem;
    }
}
