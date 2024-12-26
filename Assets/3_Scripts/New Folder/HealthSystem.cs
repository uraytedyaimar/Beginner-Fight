using System;
using UnityEngine;

public class HealthSystem {
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private float health;
    private float healthMax;

    // Construct a HealthSystem, receives the health max and sets current health to that value
    public HealthSystem( float healthMax ) {
        this.healthMax = healthMax;
        health = healthMax;
    }

    // Get the current health
    public float GetHealth() {
        return health;
    }

    // Get the current max amount of health
    public float GetHealthMax() {
        return healthMax;
    }

    // Get the current Health as a Normalized value (0-1)
    public float GetHealthNormalized() {
        return health / healthMax;
    }

    // Deal damage to this HealthSystem
    public void Damage( float damageAmount ) {
        health -= damageAmount;
        if (health < 0) {
            health = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }

    // Kill this HealthSystem
    public void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    // Test if this Health System is dead
    public bool IsDead() {
        return health <= 0;
    }

    // Heal this HealthSystem
    public void Heal( float healAmount ) {
        health += healAmount;
        if (health > healthMax) {
            health = healthMax;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    // Heal this HealthSystem to the maximum health amount
    public void HealComplete() {
        health = healthMax;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    // Set the current Health amount, doesn't set above healthAmountMax or below 0
    public void SetHealth( float health ) {
        if (health > healthMax) {
            health = healthMax;
        }
        if (health < 0) {
            health = 0;
        }
        this.health = health;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);

        if (health <= 0) {
            Die();
        }
    }

    // Set the Health Amount Max, optionally also set the Health Amount to the new Max
    public void SetHealthMax( float healthMax, bool fullHealth ) {
        this.healthMax = healthMax;
        if (fullHealth) health = healthMax;
        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Tries to get a HealthSystem from the GameObject
    /// The GameObject can have either the built in HealthSystemComponent script or any other script that creates
    /// the HealthSystem and implements the IGetHealthSystem interface
    /// </summary>
    /// <param name="getHealthSystemGameObject">GameObject to get the HealthSystem from</param>
    /// <param name="healthSystem">output HealthSystem reference</param>
    /// <param name="logErrors">Trigger a Debug.LogError or not</param>
    /// <returns></returns>
    public static bool TryGetHealthSystem( GameObject getHealthSystemGameObject, out HealthSystem healthSystem, bool logErrors = false ) {
        healthSystem = null;

        if (getHealthSystemGameObject != null) {
            if (getHealthSystemGameObject.TryGetComponent(out IGetHealthSystem getHealthSystem)) {
                healthSystem = getHealthSystem.GetHealthSystem();
                if (healthSystem != null) {
                    return true;
                } else {
                    if (logErrors) {
                        Debug.LogError($"Got HealthSystem from object but healthSystem is null! Should it have been created? Maybe you have an issue with the order of operations.");
                    }
                    return false;
                }
            } else {
                if (logErrors) {
                    Debug.LogError($"Referenced Game Object '{getHealthSystemGameObject}' does not have a script that implements IGetHealthSystem!");
                }
                return false;
            }
        } else {
            // No reference assigned
            if (logErrors) {
                Debug.LogError($"You need to assign the field 'getHealthSystemGameObject'!");
            }
            return false;
        }
    }
}
