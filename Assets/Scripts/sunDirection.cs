using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDirection : MonoBehaviour
{


    public float azimuth;
    public float zenith;

    public GameObject directionalLight;
    public GameObject arrow;

    // Use this for initialization
    void Start()
    {

        Vector3 rot = new Vector3();
        directionalLight.GetComponent<Transform>().Rotate(rot);


    }

    // Update is called once per frame
    void Update()
    {

    }
}
