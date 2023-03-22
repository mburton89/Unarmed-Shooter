using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerShip>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
    }
}
