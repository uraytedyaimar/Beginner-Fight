using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RootNode {
    public SubNode NearNode; // Node untuk animasi ketika jarak dekat
    public SubNode MedNode;  // Node untuk animasi ketika jarak sedang
    public SubNode FarNode;  // Node untuk animasi ketika jarak jauh
    public SubNode AnyNode;  // Node untuk animasi umum yang bisa digunakan di semua jarak
}

[System.Serializable]
public class SubNode : WeightageClass {
    public List<AnimName> AnimList; // Daftar animasi yang terkait dengan node ini
}

[System.Serializable]
public class WeightageClass {
    public float Weightage; // Bobot yang digunakan untuk menentukan relevansi node atau animasi
}

[System.Serializable]
public class AnimName : WeightageClass {
    public string AnimNameStr; // Nama animasi
}

public class EnemyAI : MonoBehaviour {
    public static EnemyAI Instance { get; private set; }

    private Rigidbody2D rb;
    private CharacterAnimation characterAnimation;

    [SerializeField] private float decisionTime; // Waktu jeda antara keputusan AI
    [SerializeField] private Transform otherPlayer; // Referensi ke pemain lain
    [SerializeField] private RootNode decisionTree; // Pohon keputusan untuk menentukan animasi berdasarkan jarak

    private bool isActive = false;
    private bool isAttacking = false;
    private float moveSpeed = 5;

    // Dash
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    private bool isDashing;
    private Vector2 dashDirection;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    private void OnEnable() {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void OnDisable() {
        GameHandler.Instance.OnStateChanged -= GameHandler_OnStateChanged;
    }

    private void GameHandler_OnStateChanged( object sender, System.EventArgs e ) {
        if( GameHandler.Instance.IsGamePlaying() && Player.Instance.Die() == false && Enemy.Instance.Die() == false) {
            isActive = true;
        } else {
            isActive = false;
        }
    }

    private void Start() {
        StartCoroutine(AIDecisionLoop()); // Memulai loop pengambilan keputusan AI
    }

    private IEnumerator AIDecisionLoop() {
        while (true) { // Loop yang berjalan terus-menerus
            if (isActive && !isAttacking) {
                yield return new WaitForSeconds(decisionTime); // Menunggu selama decisionTime sebelum membuat keputusan berikutnya
                MakeDecision(); // Membuat keputusan untuk memilih animasi berdasarkan jarak
            } else {
                yield return null;
            }
        }
    }

    private void MakeDecision() {
        if (!isActive) return;

        if (characterAnimation.IsPunching() || characterAnimation.IsKicking()) {
            return; // Jika sedang memukul atau menendang, jangan ambil keputusan baru
        }

        float distance = Vector3.Distance(transform.position, otherPlayer.position);
        SubNode selectedNode = SelectNodeBasedOnDistance(distance);
        string selectedAnim = SelectAnimation(selectedNode, distance);

        // Hentikan animasi yang sudah ada
        characterAnimation.ResetAnimationState();

        switch (selectedAnim) {
            case "IsPunching":
                rb.velocity = new Vector2(0, rb.velocity.y);
                characterAnimation.StartPunching();
                break;
            case "IsKicking":
                rb.velocity = new Vector2(0, rb.velocity.y);
                characterAnimation.StartKicking();
                break;
            case "IsBlocking":
                rb.velocity = new Vector2(0, rb.velocity.y);
                characterAnimation.StartBlocking();
                break;
            case "IsWalkingForward":
                rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
                characterAnimation.Walk(1);
                break;
            case "IsWalkingBackward":
                rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);
                characterAnimation.Walk(-1);
                break;
            case "IsDashForwardTriggered":
                isDashing = true;
                dashTime = dashDuration;
                dashDirection = new Vector2(-1, rb.velocity.y);
                characterAnimation.Dash(1);
                break;
            case "IsDashBackwardTriggered":
                isDashing = true;
                dashTime = dashDuration;
                dashDirection = new Vector2(1, rb.velocity.y);
                characterAnimation.Dash(-1);
                break;
            default:
                rb.velocity = Vector2.zero;
                break;
        }
    }

    private void Update() {
        if (isDashing) {
            DashMovement();
        }
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

    private SubNode SelectNodeBasedOnDistance( float distance ) {
        float minDistance = Mathf.Abs(distance - decisionTree.AnyNode.Weightage);
        SubNode selectedNode = decisionTree.AnyNode;

        float nearDistance = Mathf.Abs(distance - decisionTree.NearNode.Weightage);
        if (nearDistance < minDistance) {
            minDistance = nearDistance;
            selectedNode = decisionTree.NearNode;
        }

        float medDistance = Mathf.Abs(distance - decisionTree.MedNode.Weightage);
        if (medDistance < minDistance) {
            minDistance = medDistance;
            selectedNode = decisionTree.MedNode;
        }

        float farDistance = Mathf.Abs(distance - decisionTree.FarNode.Weightage);
        if (farDistance < minDistance) {
            selectedNode = decisionTree.FarNode;
        }
        return selectedNode;
    }

    private string SelectAnimation( SubNode node, float distance ) {
        string selectedAnim = node.AnimList[0].AnimNameStr;
        float minDistance = Mathf.Abs(distance - node.AnimList[0].Weightage);

        foreach (var anim in node.AnimList) {
            float currentDistance = Mathf.Abs(distance - anim.Weightage);
            if (currentDistance < minDistance) {
                minDistance = currentDistance;
                selectedAnim = anim.AnimNameStr;
            }
        }
        return selectedAnim;
    }
}
