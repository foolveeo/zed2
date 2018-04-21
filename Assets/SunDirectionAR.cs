using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDirectionAR : MonoBehaviour
{

    // angle received from python code on the sun direction 
    public float azimuth;
    public float zenith;
    public float radius;

    private Vector3 sunPosition;

    // worldCoordinates is an empty object with coordinate system consistent with ARKit coordinate system
    public GameObject worldCoordinates;
    // directional Light is the lamp that simulate the sun, it is used for shadow mapping
    public GameObject directionalLight;
    // 3D object to visualize the sun direction
    public GameObject arrow;
    public GameObject cameraAR;
    // Use this for initialization
    void Start()
    {
       // worldCoordinates.GetComponent<Transform>().transform.position = cameraAR.GetComponent<Transform>().transform.position + new Vector3(0,0,5);
        /*
        Vector3 rot = new Vector3(-zenith, azimuth);
        directionalLight.GetComponent<Transform>().Rotate(rot);
        arrow.GetComponent<Transform>().Rotate(rot);
        

        
        sunPosition = computeSunPosition(azimuth, zenith, radius);
        arrow.GetComponent<Transform>().LookAt(sunPosition);
        directionalLight.GetComponent<Transform>().position = sunPosition;
        directionalLight.GetComponent<Transform>().LookAt(new Vector3());
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 rot = new Vector3(-zenith, azimuth);
        directionalLight.GetComponent<Transform>().Rotate(rot);
        arrow.GetComponent<Transform>().Rotate(rot);
        
        sunPosition = computeSunPosition(azimuth, zenith, radius);
        arrow.GetComponent<Transform>().LookAt(sunPosition);
        directionalLight.GetComponent<Transform>().position = sunPosition;
        directionalLight.GetComponent<Transform>().LookAt(new Vector3());
        */

        arrow.GetComponent<Transform>().transform.rotation = Quaternion.Euler(zenith, azimuth - 90, 0);
        directionalLight.GetComponent<Transform>().transform.rotation = Quaternion.Euler(zenith + 90, azimuth - 90, 0);
    }

    private Vector3 computeSunPosition(float _azimuth, float _zenith, float _radius)
    {
        // X, Y, Z, coord respectevely East, Up, South
        Vector3 sunPos;
        float sunX, sunY, sunZ;

        sunY = _radius * Mathf.Cos(_zenith);
        sunZ = _radius * Mathf.Sin(_zenith) * (-Mathf.Cos(_azimuth));
        sunX = _radius * Mathf.Sin(_zenith) * (-Mathf.Sin(_azimuth));

        sunPos = new Vector3(sunX, sunY, sunZ);

        return sunPos;

    }
}
