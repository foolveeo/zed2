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
    
    
    // directional Light is the lamp that simulate the sun, it is used for shadow mapping
    public GameObject directionalLight;
    // 3D object to visualize the sun direction
    public GameObject arrow;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        arrow.GetComponent<Transform>().transform.localRotation = Quaternion.Euler(zenith, azimuth, 0);
        directionalLight.GetComponent<Transform>().transform.localRotation = Quaternion.Euler(zenith + 90, azimuth, 0);
    }
    
}
