using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelWinUI : MonoBehaviour
{
    public Button nextLevelButton, exitButton;

    public static LevelWinUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        exitButton.onClick.AddListener(ExitLevel);
    }

    void NextLevel()
    {
        // currently just loads the main scene again
        SceneManager.LoadScene(1);
    }

    void ExitLevel()
    {
        SceneManager.LoadScene(0);
    }
}
