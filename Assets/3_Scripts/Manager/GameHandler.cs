using System;
using UnityEngine;

public class GameHandler : MonoBehaviour {
    public static GameHandler Instance { get; private set; }

    public event EventHandler OnStateChanged;

    private enum State {
        Ready,
        FightAudio,
        GamePlaying,
        WaitingToGameOver,
        GameOver
    }

    private State state;
    private float readyTimer = 2f;
    private float fightTimer = 2f;
    private float gamePlayingTimer = 120f;
    private float gameOverCountdown = 5f;

    private void Awake() {
        Instance = this;
        state = State.Ready;
    }

    private void Update() {
        switch(state) {
            case State.Ready:
                readyTimer -= Time.deltaTime;
                if(readyTimer < 0f) {
                    state = State.FightAudio;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.FightAudio:
                fightTimer -= Time.deltaTime;
                if (fightTimer < 0f) {
                    state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f || Player.Instance.Die() == true || Enemy.Instance.Die() == true) {
                    state = State.WaitingToGameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.WaitingToGameOver:
                gameOverCountdown -= Time.deltaTime;
                if (gameOverCountdown < 0f) {
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                if ( RoundManager.Instance.EndOrNot() == true ) {
                    if (RoundManager.Instance.IsPlayerWinOrNot() == true) {
                        LevelCompleteUI.Instance.Show();
                        LevelCompleteUI.Instance.UnlockNewLevel();
                    } else {
                        LevelFailedUI.Instance.Show();
                    }
                } else {
                    StartNewRound();
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
        }
    }

    public void StartNewRound() {
        state = State.Ready;
        readyTimer = 1f;
        fightTimer = 2f;
        gamePlayingTimer = 120f;
        gameOverCountdown = 5f;
    }

    public bool ReadyState() {
        return state == State.Ready;
    }

    public bool FightState() {
        return state == State.FightAudio;
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsWaitingToGameOver() {
        return state == State.WaitingToGameOver;
    }
}
