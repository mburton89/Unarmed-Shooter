using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Larvae : MonoBehaviour
{
    [SerializeField] float timeForLarvaeToUpgrade = 3.0f;
    [SerializeField] GameObject bee;

    private void Start()
    {
        StartCoroutine("Growing");
    }

    IEnumerator Growing()
    {
        yield return new WaitForSeconds(timeForLarvaeToUpgrade);
        Instantiate(bee, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
