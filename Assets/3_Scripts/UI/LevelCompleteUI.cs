using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteUI : MonoBehaviour {
    public static LevelCompleteUI Instance { get; private set; }

    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    private void Awake() {
        Instance = this;

        nextLevelButton.onClick.AddListener(() => {
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

    public void UnlockNewLevel() {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex")) {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
