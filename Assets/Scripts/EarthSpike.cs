using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSpike : MonoBehaviour {

    private Transform myPosition;
    private Transform sonPosition;

    private Vector3 target;
    private Vector3 normal;

    // Use this for initialization
    void Start()
    {
        myPosition = gameObject.GetComponent<Transform>();
        sonPosition = myPosition.FindChild("moving_spikes_b");

        normal = sonPosition.position;
        target = normal + new Vector3(0, 0.125f, 0);

        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        while (true)
        {
            StartCoroutine("Up");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Up");
            StartCoroutine("Down");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Down");
        }
    }

    IEnumerator Up()
    {
        while (true)
        {
            sonPosition.position = Vector3.Lerp(sonPosition.position, target, Time.deltaTime * 10);
            yield return null;
        }
    }
    IEnumerator Down()
    {
        while (true)
        {
            sonPosition.position = Vector3.Lerp(sonPosition.position, normal, Time.deltaTime * 10);
            yield return null;
        }
    }
}
