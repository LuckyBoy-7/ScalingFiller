using System.Collections;
using System.Collections.Generic;
using Lucky.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public int levelNumber = 0;
    public int currentLevel = 0;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int level)
    {
        currentLevel = level;
        SceneManager.LoadScene($"Level{currentLevel}");
    }

    public void LoadNextLevel()
    {
        currentLevel += 1;
        if (currentLevel == levelNumber + 1)
            SceneManager.LoadScene($"End");
        else
            SceneManager.LoadScene($"Level{currentLevel}");
    }

    public void Restart()
    {
        LoadLevel(currentLevel);
    }
}