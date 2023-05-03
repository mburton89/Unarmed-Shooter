using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragonfly : MonoBehaviour
{
    [SerializeField] CircleCollider2D playerDetector;
    [SerializeField] float waitTime;
    [SerializeField] float dashTime;
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    EnemyShip dragonfly;

    bool charging;
    bool canSpray;

    [SerializeField] ParticleSystem acidParticles;
    [SerializeField] float sprayWaitTime;


    // Start is called before the first frame update
    void Start()
    {
        charging = false;
        canSpray = true;
        dragonfly = GetComponent<EnemyShip>();
    }

    public void PlayerInRange()
    {
        if (!charging)
        {
            StartCoroutine(ChargeDash());
        }
        if (canSpray) { StartCoroutine(ChargeSpray()); }
    }

    private IEnumerator ChargeDash()
    {
        float hAccel = dragonfly.acceleration;
        float hSpeed = dragonfly.maxSpeed;
        charging = true;
        yield return new WaitForSeconds(waitTime);
        dragonfly.acceleration = acceleration;
        dragonfly.maxSpeed = maxSpeed;
        yield return new WaitForSeconds(dashTime);
        dragonfly.acceleration = hAccel;
        dragonfly.maxSpeed = hSpeed;
        charging = false;
    }

    private IEnumerator ChargeSpray()
    {
        canSpray = false;
        acidParticles.Emit(2);
        yield return new WaitForSeconds(sprayWaitTime);
        canSpray = true;
    }
}
