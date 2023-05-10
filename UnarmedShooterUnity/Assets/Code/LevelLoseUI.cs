using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoseUI : MonoBehaviour
{
    public Button retryButton, exitButton;

    public static LevelLoseUI Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        retryButton.onClick.AddListener(RetryLevel);
        exitButton.onClick.AddListener(ExitLevel);
    }

    void RetryLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void ExitLevel()
    {
        SceneManager.LoadScene(0);
        print("Button clicked!");
    }
}
