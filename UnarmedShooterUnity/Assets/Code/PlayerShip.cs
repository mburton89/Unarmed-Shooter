using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : Ship
{
    void Update()
    {
        FollowMouse();
        HandleInput();

        SpeedGauge.Instance.speedtext.SetText((rigidbody2D.velocity.magnitude * 10).ToString("#.##") + " MPH");
    }

    void HandleInput()
    {
        if (Input.GetMouseButton(1))
        {
            Thrust();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //checks to see if boost is available
            if (currentBoostEnergy == maxBoost)
            {
                StartCoroutine(Boost());
            }
            else
            {
                //replace with some sort of UI interaction/sound alert
                print("Boost not ready");
            }
        }
    }
    void FollowMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        Vector2 directionToFace = new Vector2(
            mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = directionToFace;
    }

    private IEnumerator Boost()
    {
        maxSpeed += boostSpeed;
        acceleration += boostSpeed;
        currentBoostEnergy = 0;
        boostUpSent = false;
        yield return new WaitForSeconds(0.3f);
        maxSpeed -= boostSpeed;
        acceleration -= boostSpeed;
    }
}
