using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//setting this as static means that this class is not attached to any specific instance of an object
//you cannot have any instances of this class contructed
public static class Loader
{

    public enum Scene
    {
        Playground,
        MainMenu,
        LoadingScene
    }
    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}