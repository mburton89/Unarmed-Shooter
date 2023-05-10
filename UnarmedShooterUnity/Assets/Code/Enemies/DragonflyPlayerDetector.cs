using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonflyPlayerDetector : MonoBehaviour
{
    Dragonfly dragonfly;

    private void Start()
    {
        dragonfly = GetComponentInParent<Dragonfly>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>())
        {
            dragonfly.PlayerInRange();
        }
    }
}
