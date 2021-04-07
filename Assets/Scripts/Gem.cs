using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    private Transform self;


	// Use this for initialization
	void Start () {
        self = (Resources.Load("gem 2") as GameObject).GetComponent<Transform>().FindChild("gem 3");

	}

    void Update()
    {
        AwardRotate();

    }
	
    private void AwardRotate()
    {
        self.Rotate(Vector3.up);
    }

}
