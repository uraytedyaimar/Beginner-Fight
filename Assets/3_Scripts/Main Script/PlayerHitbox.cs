using UnityEngine;

public class PlayerHitbox : MonoBehaviour {
    private const string ENEMY_HURTBOX = "EnemyHurtBox";

    [SerializeField] private CharacterAnimation characterAnimation;

    private bool hasHit = false;

    [SerializeField] private float blockedDamage = 1;
    [SerializeField] private float punchDamage = 3;
    [SerializeField] private float kickDamage = 5;

    private float punchDuration = 0.33f; // Durasi animasi pukulan
    private float kickDuration = 0.45f; // Durasi animasi tendangan

    private void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.gameObject.layer == LayerMask.NameToLayer(ENEMY_HURTBOX) && !hasHit) {
            HandleHit();
        }
    }

    private void HandleHit() {
        if (!hasHit) {
            hasHit = true;
            float resetDuration = GetAnimationDuration();

            if (Enemy.Instance.IsBlocking()) {
                Enemy.Instance.Damage(blockedDamage);
            } else {
                if (characterAnimation.IsPunching()) {
                    Enemy.Instance.Damage(punchDamage);
                } else if (characterAnimation.IsKicking()) {
                    Enemy.Instance.Damage(kickDamage);
                }
            }
            Invoke(nameof(ResetHit), resetDuration);
        }
    }

    private void ResetHit() {
        hasHit = false;
    }

    private float GetAnimationDuration() {
        if (characterAnimation.IsPunching()) {
            return punchDuration;
        } else if (characterAnimation.IsKicking()) {
            return kickDuration;
        } else {
            return 0.1f; // Default jika tidak ada animasi serangan yang sedang berlangsung
        }
    }
}
