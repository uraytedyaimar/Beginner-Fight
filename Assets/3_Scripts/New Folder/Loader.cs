using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        MainMenu,
        Loading,
        Training,
        LevelSelect,
        Gameplay,
    }

    public static Scene targetScene;

    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
