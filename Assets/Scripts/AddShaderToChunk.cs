using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShaderToChunk : MonoBehaviour {

    private GameObject zedMesh;
    private GameObject chunk1;
    public Material mat;
    private bool done;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        if(!done)
        {
            zedMesh = GameObject.Find("Chunk0");
            chunk1 = GameObject.Find("Chunk1");
        }
        

        if (zedMesh != null && chunk1 == null && done == false)
        {
            print("adding component");
            try
            {
                Destroy(zedMesh.GetComponent<MeshRenderer>());
            }
            catch
            {
                print("not able to remove the component MeshRenderer");
            }
            zedMesh.AddComponent<MeshRenderer>();
            zedMesh.GetComponent<MeshRenderer>().material = mat;
            this.GetComponent<MeshFilter>().mesh = zedMesh.GetComponent<MeshFilter>().mesh; 
            
            done = true;
        }
    }
}
