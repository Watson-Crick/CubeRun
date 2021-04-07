using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour {

    private Transform self;
    private Vector3 target;
    private Vector3 initail;

    // Use this for initialization
    void Start()
    {
        self = GameObject.Find("smashing_spikes_b").GetComponent<Transform>();
        initail = self.position;
        target = new Vector3(initail.x, initail.y - 0.6f, initail.z);

        StartCoroutine("StartSpike");
    }

    IEnumerator StartSpike()
    {
        while (true)
        {
            StartCoroutine("Down");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Down");
            StartCoroutine("Up");
            yield return new WaitForSeconds(2.0f);
            StopCoroutine("Up");
        }
    }


    IEnumerator Down()
    {
        while (true)
        {
            self.position = Vector3.Lerp(self.position, target, Time.deltaTime * 10);
        }
    }
    IEnumerator Up()
    {
        while (true)
        {
            self.position = Vector3.Lerp(self.position, initail, Time.deltaTime * 10);
        }
    }
}
