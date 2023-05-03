using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeID : MonoBehaviour
{
    public int ID = 0;
    public Transform Pivot;
    bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDestroy()
    {
        if (!isQuitting)
        {
            if (GetComponentInParent<centipedeTail>() != null)
            {
                GetComponentInParent<centipedeTail>().RemoveTail(ID);
            }
        }
        
    }

    private void Start()
    {
        CentipedeID[] children = transform.parent.GetComponentsInChildren<CentipedeID>();
        
        if(ID == 0)
        {
            Pivot = transform.parent.transform;
        }
        else
        {
            foreach (CentipedeID child in children)
            {
                if (child.ID == ID-1)
                {
                    Pivot = child.gameObject.transform;
                    
                }
            }
        }
    }

    private void Update()
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.transform.parent.rotation.z * -1.0f);
        if(Pivot != null)
        {

            Vector2 directionToFace = new Vector2(
            Pivot.position.x - transform.position.x, Pivot.position.y - transform.position.y);

            transform.up = directionToFace;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
