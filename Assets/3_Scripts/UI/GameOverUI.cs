using System;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private GameObject koUI;
    [SerializeField] private GameObject youWinUI;
    [SerializeField] private GameObject youLoseUI;

    private float koTimer = 2f;

    private void Start() {
        KOHide();
        YouWinHide();
        YouLoseHide();

        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
    }

    private void Update() {
        GameOver();
    }

    private void GameHandler_OnStateChanged( object sender, EventArgs e ) {
        if (GameHandler.Instance.ReadyState()) {
            ResetKOTimer();
        }
    }

    private void GameOver() {
        if (GameHandler.Instance.IsWaitingToGameOver()) {
            if (Player.Instance.Die()) {
                KOShow();
                if (RoundManager.Instance.EndOrNot() == false) {
                    // Kalau belum round terakhir, langsung skip 2 detiknya
                } else {
                    koTimer -= Time.deltaTime;
                    if (koTimer < 0f) {
                        KOHide();
                        YouLoseShow();
                    }
                }
            } else if (Enemy.Instance.Die()) {
                KOShow();
                if (RoundManager.Instance.EndOrNot() == false) {
                    // Kalau belum round terakhir, langsung skip 2 detiknya
                } else {
                    koTimer -= Time.deltaTime;
                    if (koTimer < 0f) {
                        KOHide();
                        YouWinShow();
                    }
                }
            } else {
                Debug.Log("Cek siapa yang memiliki darah terbanyak maka dia yang menang");
            }
        } else {
            KOHide();
            YouWinHide();
            YouLoseHide();
        }
    }

    private void KOShow() {
        koUI.gameObject.SetActive(true);
    }

    private void KOHide() {
        koUI.gameObject.SetActive(false);
    }

    private void YouWinShow() {
        youWinUI.gameObject.SetActive(true);
    }

    private void YouWinHide() {
        youWinUI.gameObject.SetActive(false);
    }

    private void YouLoseShow() {
        youLoseUI.gameObject.SetActive(true);
    }

    private void YouLoseHide() {
        youLoseUI.gameObject.SetActive(false);
    }

    public void ResetKOTimer() {
        koTimer = 2f;
    }
}
