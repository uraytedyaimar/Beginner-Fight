using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour {
    public static RoundManager Instance { get; private set; }

    private int totalRounds = 3;
    private int currentRound = 1;

    private bool isFirstRound = true;

    private int player1Score = 0;
    private int player2Score = 0;
    private int winCondition = 2;

    private int round1Audio = 0;
    private int round2Audio = 1;
    private int finalRoundAudio = 2;

    [SerializeField] private Image playerRound1;
    [SerializeField] private Image playerRound2;
    
    [SerializeField] private Image enemyRound1;
    [SerializeField] private Image enemyRound2;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        UpdateUI();
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void Update() {
        if (isFirstRound == true) {
            isFirstRound = false;
            // dimainkan di frame di update pertama karena volume nya didapat pada start
            SoundManager.Instance.PlayRoundSound(round1Audio);
        }
    }

    private void GameHandler_OnStateChanged( object sender, System.EventArgs e ) {
        if (GameHandler.Instance.ReadyState()) {
            UpdateUI();
            ResetStatus();

            if (currentRound == 2) {
                SoundManager.Instance.PlayRoundSound(round2Audio);
            }
            if (currentRound == 3) {
                SoundManager.Instance.PlayRoundSound(finalRoundAudio);
            }
        }
    }

    public void EndRound( int winner ) {
        if (winner == 1) {
            player1Score++;
        }
        else if (winner == 2) {
            player2Score++;
        }
        currentRound++;
    }

    private void UpdateUI() {
        switch(player1Score) {
            case 1:
                playerRound1.color = Color.green;
                break;
            case 2:
                playerRound1.color = Color.green;
                playerRound2.color = Color.green;
                break;
        }
        switch (player2Score) {
            case 1:
                enemyRound1.color = Color.green;
                break;
            case 2:
                enemyRound1.color = Color.green;
                enemyRound2.color = Color.green;
                break;
        }
    }

    public bool EndOrNot() {
        return player1Score == winCondition || player2Score == winCondition || currentRound > totalRounds;
    }

    public bool IsPlayerWinOrNot() {
        return player1Score == winCondition;
    }

    private void ResetStatus() {
        Player.Instance.ResetStatus();
        Enemy.Instance.ResetStatus();
    }
}
