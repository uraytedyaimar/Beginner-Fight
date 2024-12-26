using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    public static PauseUI Instance { get; private set; }

    [SerializeField] private Button pauseUI;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake() {
        Instance = this;

        pauseUI.onClick.AddListener(() => {
            PauseUI.Instance.Show();
        });
        
        closeButton.onClick.AddListener(() => {
            PauseUI.Instance.Hide();
        });
        
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
        Time.timeScale = 1f;
    }
}
