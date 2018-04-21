using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraCoord : MonoBehaviour {

    public GameObject camUnity;
    public GameObject camZED;

	// Use this for initialization
	void Start () {
        camUnity.GetComponent<Transform>().transform.position = camZED.GetComponent<Transform>().transform.position;
        camUnity.GetComponent<Transform>().transform.rotation = camZED.GetComponent<Transform>().transform.rotation;
        camUnity.GetComponent<Transform>().transform.localScale = camZED.GetComponent<Transform>().transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        camUnity.GetComponent<Transform>().transform.position = camZED.GetComponent<Transform>().transform.position;
        camUnity.GetComponent<Transform>().transform.rotation = camZED.GetComponent<Transform>().transform.rotation;
        camUnity.GetComponent<Transform>().transform.localScale = camZED.GetComponent<Transform>().transform.localScale;
    }
}
