using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 1f;
    private float damagedHealthShrinkTimer;

    [Tooltip("Optional; Either assign a reference in the Editor (that implements IGetHealthSystem) or manually call SetHealthSystem()")]
    [SerializeField] private GameObject getHealthSystemGameObject;

    [Tooltip("Image to show the Health Bar, should be set as Fill, the script modifies fillAmount")]
    [SerializeField] private Image barImage;
    [Tooltip("Image to show the Damaged Health Bar, should be set as Fill, the script modifies fillAmount")]
    [SerializeField] private Image damagedBarImage;

    private HealthSystem healthSystem;

    Vector3 originalPos;
    private bool isShaking = false;
    private float shakeAmount = 250f;
    private float shakeDuration = 0.25f;

    private void Start() {
        if (HealthSystem.TryGetHealthSystem(getHealthSystemGameObject, out HealthSystem healthSystem)) {
            SetHealthSystem(healthSystem);
            damagedBarImage.fillAmount = barImage.fillAmount;
        } else {
            Debug.LogError("Failed to get HealthSystem from GameObject");
        }

        originalPos = transform.position;
    }

    private void Update() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if(damagedHealthShrinkTimer < 0) {
            if(barImage.fillAmount < damagedBarImage.fillAmount) {
                float shrinkSpeed = 1f;
                damagedBarImage.fillAmount -= shrinkSpeed * Time.deltaTime;
            }
        }
    }

    // Set the Health System for this Health Bar
    public void SetHealthSystem( HealthSystem healthSystem ) {
        if (this.healthSystem != null) {
            this.healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
        this.healthSystem = healthSystem;

        UpdateHealthBar();

        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealed( object sender, System.EventArgs e ) {
        damagedBarImage.fillAmount = barImage.fillAmount;
    }

    // Event fired from the Health System when Health Amount changes, update Health Bar
    private void HealthSystem_OnHealthChanged( object sender, System.EventArgs e ) {
        UpdateHealthBar();
        if (!isShaking) {
            StartCoroutine(ShakeNow());
        }
    }

    // Update Health Bar using the Image fillAmount based on the current Health Amount
    private void UpdateHealthBar() {
        barImage.fillAmount = healthSystem.GetHealthNormalized();
        damagedHealthShrinkTimer = DAMAGED_HEALTH_SHRINK_TIMER_MAX;
    }

    // Clean up events when this Game Object is destroyed
    private void OnDestroy() {
        if (healthSystem != null) {
            healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
        }
    }

    private IEnumerator ShakeNow() {
        originalPos = transform.position;
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < shakeDuration) {
            Vector3 newPos = originalPos + (Vector3)Random.insideUnitCircle * shakeAmount * Time.deltaTime;
            transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
        isShaking = false;
    }
}
