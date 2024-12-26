using UnityEngine;

public class HitboxController : MonoBehaviour {

    public static HitboxController Instance { get; private set; }

    // Array untuk menyimpan hitbox
    [SerializeField] private GameObject[] hitboxes;

    private void Awake() {
        Instance = this;
    }

    // Method untuk mengaktifkan hitbox
    public void ActivateHitbox( int index ) {
        if (index >= 0 && index < hitboxes.Length) {
            hitboxes[index].SetActive(true);
        }
    }

    // Method untuk menonaktifkan hitbox
    public void DeactivateHitbox( int index ) {
        if (index >= 0 && index < hitboxes.Length) {
            hitboxes[index].SetActive(false);
        }
    }
}
