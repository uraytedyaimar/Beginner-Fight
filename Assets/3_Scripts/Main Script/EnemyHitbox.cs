using UnityEngine;

public class EnemyHitbox : MonoBehaviour {
    private const string PLAYER_HURTBOX = "PlayerHurtBox";

    [SerializeField] private CharacterAnimation characterAnimation;

    private bool hasHit = false;

    [SerializeField] private float blockedDamage = 1;
    [SerializeField] private float punchDamage = 3;
    [SerializeField] private float kickDamage = 5;

    private float punchDuration = 0.4f; // Durasi animasi pukulan
    private float kickDuration = 0.7f; // Durasi animasi tendangan

    private void OnTriggerEnter2D( Collider2D collision ) {
        if (collision.gameObject.layer == LayerMask.NameToLayer(PLAYER_HURTBOX) && !hasHit) {
            HandleHit();
        }
    }

    private void HandleHit() {
        if (!hasHit) {
            hasHit = true;
            float resetDuration = GetAnimationDuration();

            if (Player.Instance.IsBlocking()) {
                Player.Instance.Damage(blockedDamage);
            } else {
                if (characterAnimation.IsPunching()) {
                    Player.Instance.Damage(punchDamage);
                } else if (characterAnimation.IsKicking()) {
                    Player.Instance.Damage(kickDamage);
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
