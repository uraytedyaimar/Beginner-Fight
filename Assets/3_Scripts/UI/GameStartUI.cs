using TMPro;
using UnityEngine;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fightText;

    private void Start() {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;
        Hide();
    }

    private void GameHandler_OnStateChanged( object sender, System.EventArgs e ) {
        if (GameHandler.Instance.FightState()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
