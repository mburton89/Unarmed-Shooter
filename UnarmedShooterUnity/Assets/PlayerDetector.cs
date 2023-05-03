using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    EnemyShip ship;

    private void Start()
    {
        ship = GetComponentInParent<EnemyShip>();
        if (ship == null)
        {
            ship = GetComponentInParent<Bee>().GetComponent<EnemyShip>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>())
        {
            ship.playerNear = true;
            ship.PlayerInRange();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>())
        {
            ship.playerNear = false;
        }
    }
}
