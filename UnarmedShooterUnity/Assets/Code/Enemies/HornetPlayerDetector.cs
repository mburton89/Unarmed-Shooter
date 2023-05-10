using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornetPlayerDetector : MonoBehaviour
{
    Hornet hornet;

    private void Start()
    {
        hornet = GetComponentInParent<Hornet>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>())
        {
            hornet.PlayerInRange();
        }
    }
}
