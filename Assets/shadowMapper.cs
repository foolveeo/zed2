using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowMapper : MonoBehaviour {

    public Material mat;
    private GameObject meshHolder;
    private MeshRenderer meshRenderer;

    // Use this for initialization
    void Start () {
        meshHolder = GameObject.Find("[ZED Mesh Holder]");
       
    }
	
	// Update is called once per frame
	void Update () {

        if(meshHolder != null)
        {
            foreach (Transform child in meshHolder.transform)
            {
                meshRenderer = child.GetComponent<MeshRenderer>();
                meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
                meshRenderer.receiveShadows = true;
                meshRenderer.material = mat;
            }
        }
        else
        {
            Debug.Log("Mesh Holder is equal to null!");
            meshHolder = GameObject.Find("[ZED Mesh Holder]");
        }
		
	}
}
