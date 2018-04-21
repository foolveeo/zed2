using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRendererToCube : MonoBehaviour {

    public GameObject gameObj;
    public Material mat;

    // Use this for initialization
    void Start () {
        gameObj.AddComponent<MeshRenderer>();
        gameObj.GetComponent<MeshRenderer>().material = mat;
        Destroy( gameObj.GetComponent<MeshRenderer>()   );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
