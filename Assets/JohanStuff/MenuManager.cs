using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public List<string> levelNames;
    public string mainMenuName;
    public int currentLevelIndex;

    public static MenuManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(int level)
    {
        currentLevelIndex = level;
        string levelName = levelNames[level];

        if (level == -1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainMenuName);
            return;
        }

        if (levelNames.Contains(levelName))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError($"Level '{levelName}' not found in levelNames list.");
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex < levelNames.Count)
        {
            LoadLevel(currentLevelIndex++);
        }
        else
        {
            Debug.Log("No more levels to load, therefore win condition?");
        }
    }
}
