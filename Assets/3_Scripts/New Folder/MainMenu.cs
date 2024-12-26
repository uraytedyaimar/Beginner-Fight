using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    [SerializeField] private Button playButton;
    [SerializeField] private Button trainingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button settingsUI;

    private void Awake() {
        playButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.Gameplay);
        });

        trainingButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.Training);
        });

        settingsUI.onClick.AddListener(() => {
            SettingsUI.Instance.Show();
        });

        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
