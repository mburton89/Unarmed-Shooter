using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPowerUp : MonoBehaviour
{

    public bool canBePickedUp;
    private void Start()
    {
        canBePickedUp = false;
        StartCoroutine(InvlunOnCreation());
    }
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
            Destroy(gameObject);
        }
    }

    IEnumerator InvlunOnCreation()
    {
        yield return new WaitForSeconds(1f);
        canBePickedUp = true;
        yield break;
    }
}
