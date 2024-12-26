using System.Collections;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    private const string IS_WALKING_FORWARD = "IsWalkingForward";
    private const string IS_WALKING_BACKWARD = "IsWalkingBackward";
    private const string IS_DASH_FORWARD = "IsDashForwardTriggered";
    private const string IS_DASH_BACKWARD = "IsDashBackwardTriggered";
    private const string IS_PUNCHING = "IsPunching";
    private const string IS_KICKING = "IsKicking";
    private const string IS_BLOCKING = "IsBlocking";
    private const string IS_DEAD_TRIGGERED = "IsDeadTriggered";
    private const string IS_DAMAGED_TRIGGERED = "IsDamagedTriggered";

    private Animator animator;

    private bool isPunching;
    private bool isKicking;

    private float punchDuration = 0.4f; // Durasi animasi pukulan
    private float kickDuration = 0.7f; // Durasi animasi tendangan

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Walk(float dir) {
        ResetAnimationState();
        animator.SetBool(IS_WALKING_FORWARD, dir > 0);
        animator.SetBool(IS_WALKING_BACKWARD, dir < 0);
    }

    public void Dash( float dir ) {
        animator.SetTrigger(dir > 0 ? IS_DASH_FORWARD : IS_DASH_BACKWARD);
    }

    public void StartPunching() {
        ResetAnimationState();
        animator.SetTrigger(IS_PUNCHING);
        isPunching = true;
        StartCoroutine(ResetPunching());
    }

    public void StartKicking() {
        ResetAnimationState();
        animator.SetTrigger(IS_KICKING);
        isKicking = true;
        StartCoroutine(ResetKicking());
    }

    public void StartBlocking() {
        ResetAnimationState();
        animator.SetBool(IS_BLOCKING, true);
    }

    public void StopBlocking() {
        animator.SetBool(IS_BLOCKING, false);
    }

    public bool IsPunching() {
        return isPunching;
    }

    public bool IsKicking() {
        return isKicking;
    }

    public bool IsBlocking() {
        return animator.GetBool(IS_BLOCKING);
    }

    public void IsDamaged() {
        animator.SetTrigger(IS_DAMAGED_TRIGGERED);
    }

    public void IsDead() {
        animator.SetTrigger(IS_DEAD_TRIGGERED);
    }

    private IEnumerator ResetPunching() {
        yield return new WaitForSeconds(punchDuration);
        isPunching = false;
    }

    private IEnumerator ResetKicking() {
        yield return new WaitForSeconds(kickDuration);
        isKicking = false;
    }

    public void PlayPunchSound() {
        SoundManager.Instance.PlayPunchSound();
    }

    public void PlayKickSound() {
        SoundManager.Instance.PlayKickSound();
    }

    public void ResetAnimationState() {
        animator.SetBool(IS_WALKING_FORWARD, false);
        animator.SetBool(IS_WALKING_BACKWARD, false);
        animator.ResetTrigger(IS_DASH_FORWARD);
        animator.ResetTrigger(IS_DASH_BACKWARD);
        animator.ResetTrigger(IS_PUNCHING);
        animator.ResetTrigger(IS_KICKING);
        animator.SetBool(IS_BLOCKING, false);
        animator.ResetTrigger(IS_DEAD_TRIGGERED);
        animator.ResetTrigger(IS_DAMAGED_TRIGGERED);

        isPunching = false;
        isKicking = false;
    }
}
