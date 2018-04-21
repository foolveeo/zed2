using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARKitConversion : MonoBehaviour {
    // implement code to calibrate the world direction t the beginning of the session
    // TCP with unity app

    // at first, using the 1/4 screw accessory, the orientation of worldCoordinate will be thematrix from the ARKit app  that maps the woorldCoordinate in ARKit (respect to gravity and compass heading) to camera coordinate
    // this matrix applied to the zed camera direction will give the woorldCoordinates in the zed application

    // the upgrade will use openCV calibration


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3 ARKitCoordToUnityCoord(Vector3 vectorIn)
    {
        Vector3 vectorOut = new Vector3();
        
        return vectorOut;
    }
}
