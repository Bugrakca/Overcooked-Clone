using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        _targetScene = targetScene;

        SceneManager.LoadScene((int)Scene.LoadingScene);
        
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene((int)_targetScene);
    }
}