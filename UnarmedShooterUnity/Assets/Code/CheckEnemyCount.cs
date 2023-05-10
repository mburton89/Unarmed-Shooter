using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckEnemyCount : MonoBehaviour
{
    public static CheckEnemyCount Instance;
    public string nextSceneName;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InvokeRepeating("CHeckEnemies", 0, 3);
    }

    public void CHeckEnemies()
    {
        EnemyShip[] enemyShips = FindObjectsOfType<EnemyShip>();
        int enemyShipsCount = enemyShips.Length;
        if(enemyShipsCount <= 0)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}

