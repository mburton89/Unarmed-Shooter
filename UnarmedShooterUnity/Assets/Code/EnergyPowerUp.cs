using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPowerUp : MonoBehaviour
{ 
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 0, -2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerShip>() && collision.GetComponent<PlayerShip>().currentBoostEnergy != collision.GetComponent<PlayerShip>().maxBoost)
        {
            collision.GetComponent<PlayerShip>().currentBoostEnergy = collision.GetComponent<PlayerShip>().maxBoost;
            DoomguyHealthManager.Instance.ShowHappy();
            Destroy(gameObject);
        }
    }
}
