using UnityEngine;
using UnityEngine.UI;

public class LevelFailedUI : MonoBehaviour {
    public static LevelFailedUI Instance { get; private set; }

    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake() {
        Instance = this;

        retryButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.Gameplay);
        });

        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenu);
        });

        settingsButton.onClick.AddListener(() => {
            SettingsUI.Instance.Show();
        });

        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }

    private void Start() {
        Hide();
    }

    public void Show() {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
