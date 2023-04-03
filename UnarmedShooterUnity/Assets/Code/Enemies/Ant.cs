using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    EnemyShip thisEnemy;

    [SerializeField] Waypoints waypointList;
    Transform currentWaypoint;
    [SerializeField] int startWithWaypointID = 0;
    [SerializeField] bool spawnAtWaypoint = true;

    [SerializeField] float distanceThreshold = 0.1f;


    
    // Start is called before the first frame update
    void Start()
    {
        thisEnemy = GetComponent<EnemyShip>();
        currentWaypoint = waypointList.GetNextWaypoint(currentWaypoint, startWithWaypointID);

        if (spawnAtWaypoint)
        {
            transform.position = currentWaypoint.position;
            currentWaypoint = waypointList.GetNextWaypoint(currentWaypoint);
        }
    }

    // Update is called once per frame
    void Update()
    {
        thisEnemy.target = currentWaypoint.transform;
        if (Vector2.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypointList.GetNextWaypoint(currentWaypoint);
        }
    }

}
