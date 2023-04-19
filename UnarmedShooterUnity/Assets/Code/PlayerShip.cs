using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShip : Ship
{

    public Image boostImage;
    public Sprite boostReadySprite;
    public Sprite boostNotReadySprite;
    public Image masterUI;

    public float shakeAmount = 30f;

    bool isUIMoving = false;



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
        
        //checks to see if boost is available
        if (currentBoostEnergy >= maxBoost)
        {
            boostImage.sprite = boostReadySprite;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Boost());
            }
        }
        else
        {
            boostImage.sprite = boostNotReadySprite;



            //replace with some sort of UI interaction/sound alert
            print("Boost not ready");
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

    public void TakePlayerDamage()
    {
        if (!isUIMoving)
        {
            isUIMoving = true;
            StartCoroutine(UIonDamage());
            StartCoroutine(UIShake());
        }
    }


    private IEnumerator UIonDamage()
    {

        masterUI.color = Color.red;
        yield return new WaitForSeconds(.25f);
        masterUI.color = Color.white;

    }

    private IEnumerator UIShake()
    {

        float shakeTimer =0;
        Vector3 originalPosition = masterUI.rectTransform.localPosition;


        while(shakeTimer<.25f)
        {
            float randomX=Random.Range(originalPosition.x - shakeAmount, originalPosition.x);

            float randomY = Random.Range(originalPosition.y, originalPosition.y + shakeAmount);



            //set the position of masterUI to random position
            Vector3 randomPosition=new Vector3(randomX,randomY);

            masterUI.rectTransform.localPosition = randomPosition;


            shakeTimer = shakeTimer + Time.deltaTime;
            yield return new WaitForEndOfFrame();


        }

        masterUI.rectTransform.localPosition = originalPosition;
        //set the position of th emasterUI to the original position
        isUIMoving = false;
    }


}
