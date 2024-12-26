using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IGetHealthSystem {
    public static Enemy Instance { get; private set; }

    private const int ENEMY_HITBOX = 1;

    private Rigidbody2D rb;
    private HealthSystem healthSystem;
    private CharacterAnimation characterAnimation;

    private bool isDead = false;

    // Hitstop
    private Coroutine hitStopCoroutine;
    private float lastHitTime;
    private float hitStopDuration = 0.12f; // Duration of hitstop in seconds

    // Reset
    private Vector2 initialPosition = new Vector2(2.5f, -2.138f);

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        characterAnimation = GetComponent<CharacterAnimation>();
        healthSystem = new HealthSystem(100);
    }

    private void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnDead += HealthSystem_OnDead;

        HitboxController.Instance.DeactivateHitbox(ENEMY_HITBOX);
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
        rb.velocity = Vector2.zero;
        characterAnimation.ResetAnimationState();
        characterAnimation.IsDead();        
        RoundManager.Instance.EndRound(1);
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

    public HealthSystem GetHealthSystem() {
        return healthSystem;
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
        rb.velocity = Vector2.zero;
    }

    public void ActiveHitbox() {
        HitboxController.Instance.ActivateHitbox(ENEMY_HITBOX);
    }

    public void DeactiveHitbox() {
        HitboxController.Instance.DeactivateHitbox(ENEMY_HITBOX);
    }
}
