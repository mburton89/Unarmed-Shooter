using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    public bool canBePickedUp;
    public int healthToBeGained;

    private void Start()
    {
        canBePickedUp = false;
        StartCoroutine(InvlunOnCreation());
    }
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerShip>() && collision.GetComponent<PlayerShip>().currentHealth != collision.GetComponent<PlayerShip>().maxHealth && canBePickedUp)
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

    IEnumerator InvlunOnCreation()
    {
        yield return new WaitForSeconds(1f);
        canBePickedUp = true;
        yield break;
    }

}
