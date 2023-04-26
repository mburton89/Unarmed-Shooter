using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : MonoBehaviour
{
    public int size = 5;
    public centipedeTail tail;

    // Start is called before the first frame update
    void Start()
    {
        tail = GetComponent<centipedeTail>();
        tail.AddTail(size);
        GetComponent<SpriteRenderer>().sortingOrder = size;
    }
}
