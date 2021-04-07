using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    private Transform myPosition;

    private PlayControl myPlayer;

    public bool startFollow = false;

    private Vector3 startPosition; 

	void Start () {
        startPosition = gameObject.GetComponent<Transform>().position;

        myPosition = gameObject.GetComponent<Transform>();

        myPlayer = GameObject.Find("cube_box").GetComponent<PlayControl>();       
	}

    void Update()
    {
        if(startFollow)
        {
            CameraFollow();
        }
        
    }

    public void CameraFollow()
    {
        Vector3 pos = new Vector3(myPosition.position.x, myPosition.position.y, myPlayer.GetComponent<Transform>().position.z - 0.75f);
        myPosition.position = Vector3.Lerp(myPosition.position, pos, Time.deltaTime);
    }

    public void ResetCamera()
    {
        myPosition.position = startPosition;
    }

    
}
