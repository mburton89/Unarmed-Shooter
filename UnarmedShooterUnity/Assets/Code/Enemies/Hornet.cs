using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet : MonoBehaviour
{

    [SerializeField] CircleCollider2D playerDetector;
    [SerializeField] float waitTime;
    [SerializeField] float dashTime;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    EnemyShip hornet;

    bool charging;


    // Start is called before the first frame update
    void Start()
    {
        charging = false;
        hornet = GetComponent<EnemyShip>();
    }

    public void PlayerInRange()
    {
        if (!charging)
        {
            StartCoroutine(ChargeDash());
        }
    }

    private IEnumerator ChargeDash()
    {
        float hAccel = hornet.acceleration;
        float hSpeed = hornet.maxSpeed;
        charging = true;
        yield return new WaitForSeconds(waitTime);
        hornet.acceleration = acceleration;
        hornet.maxSpeed = maxSpeed;
        yield return new WaitForSeconds(dashTime);
        hornet.acceleration = hAccel;
        hornet.maxSpeed = hSpeed;
        charging = false;
    }
}
