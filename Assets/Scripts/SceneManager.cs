using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    public enum DefaultLevels { Menu = 2, Congratz = 0, Gameover = 1}

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (SceneUtility.GetBuildIndexByScenePath($"Level {currentSceneIndex -1 }") != -1) LoadLevel(currentSceneIndex + 1);
        else LoadCongratz();
    }

    public void ReloadCurrentLevel()
    {
        LoadLevel(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        LoadLevel((int)DefaultLevels.Menu);
    }

    public void LoadGameover()
    {
        LoadLevel((int)DefaultLevels.Gameover);
    }

    public void LoadCongratz()
    {
        LoadLevel((int)DefaultLevels.Congratz);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
