using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public int healthToBeGained;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>() && collision.GetComponent<PlayerShip>().currentHealth != collision.GetComponent<PlayerShip>().maxHealth)
        {
            collision.GetComponent<PlayerShip>().currentHealth += healthToBeGained;
            Destroy(gameObject);
            if(collision.GetComponent<PlayerShip>().currentHealth >= collision.GetComponent<PlayerShip>().maxHealth)
            {
                collision.GetComponent<PlayerShip>().currentHealth = collision.GetComponent<PlayerShip>().maxHealth;
            }
            HUD.Instance.DisplayHealth(collision.GetComponent<PlayerShip>().currentArmor, collision.GetComponent<PlayerShip>().currentHealth);
        }
    }

}
