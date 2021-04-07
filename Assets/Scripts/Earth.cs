using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour {

    private Transform self;
    private Vector3 target;
    private Vector3 initail;

	// Use this for initialization
	void Start () {
        self = GameObject.Find("moving_spikes_b").GetComponent<Transform>();
        initail = self.position;
        target = new Vector3(initail.x, initail.y + 0.15f, initail.z);

        StartCoroutine("StartSpike");
	}

    IEnumerator StartSpike()
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
            self.position = Vector3.Lerp(self.position, target, Time.deltaTime * 10);
        }
    }
    IEnumerator Down()
    {
        while (true)
        {
            self.position = Vector3.Lerp(self.position, initail, Time.deltaTime * 10);
        }
    }










	
	// Update is called once per frame
	void Update () {
		
	}
}
