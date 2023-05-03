using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : EnemyShip
{
    public GameObject spawnObject;
    bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting && Random.Range(1,4) < 4)
        {
            Instantiate(spawnObject, gameObject.transform.position, gameObject.transform.rotation);
        }
        
    }
}
