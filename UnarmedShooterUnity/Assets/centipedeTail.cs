using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centipedeTail : MonoBehaviour
{
    public Transform CentipedeTailManager;
    public Transform centipedeObject;
    public float circleDiameter;

    private List<Transform> tail = new List<Transform>();
    private List<int> ID = new List<int>();
    private List<Vector2> positions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        positions.Add(CentipedeTailManager.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = ((Vector2)CentipedeTailManager.position - positions[0]).magnitude;

        if(distance > circleDiameter)
        {
            Vector2 direction = ((Vector2)CentipedeTailManager.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= circleDiameter;
        }

        for (int i = 0; i < tail.Count; i++)
        {
            if (tail[i] != null)
            {
                tail[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circleDiameter);
            }
            
        }
    }

    public void AddTail(int amount = 1)
    {
        for (int i=0; i<amount; i++)
        {
            Transform centTail = Instantiate(centipedeObject, positions[positions.Count - 1], Quaternion.identity, transform);
            //centTail.gameObject.GetComponent<CentipedeID>().ID = ID.Count + i;
            centTail.gameObject.GetComponent<CentipedeID>().ID = i;
            centTail.gameObject.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<Centipede>().size-1-i;
            tail.Add(centTail);
            //ID.Add(ID.Count+i);
            ID.Add(i);
            positions.Add(centTail.position);
        }
    }

    public void RemoveTail(int tailID = 0)
    {
        for (int i = ID.Count-1; i >= tailID; i--)
        {
            var temp = tail[i].gameObject;

            positions.Remove(tail[i].position);
            tail.RemoveAt(i);
            
            ID.Remove(i);

            Destroy(temp);
        }
        FindObjectOfType<EnemyShipSpawner>().CountEnemyShips();
    }
}
