using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSkyLightSourcesIcoSphere : MonoBehaviour {

    public GameObject lowRes;
    public GameObject midRes;
    public GameObject highRes;

    public Material mat;
    private GameObject icoSphere;
    private GameObject[] skyLights;

    [Range(0, 2)]
    public int resolution;


	// Use this for initialization
	void Start () {

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localPosition = new Vector3(0, 0, 0);
        icoSphere = new GameObject();
        icoSphere.AddComponent<MeshFilter>();
        if (resolution == 0)
        {
            icoSphere = lowRes;
        }
        if (resolution == 1)
        {
            icoSphere = midRes;
        }
        if (resolution == 2)
        {
            icoSphere = highRes;
        }

        int nbr_lights = icoSphere.GetComponent<MeshFilter>().mesh.vertices.Length;
        
        Vector3[] lightPos = new Vector3[nbr_lights];
        lightPos = icoSphere.GetComponent<MeshFilter>().mesh.vertices;

        lightPos = removeDuplicates(lightPos);

        nbr_lights = lightPos.Length;

        Debug.Log("after filtering: " + nbr_lights.ToString());
        skyLights = new GameObject[nbr_lights];
        for (int i = 0; i < nbr_lights; i++)
        {
            skyLights[i] = new GameObject("skyLight_" + i.ToString());
            skyLights[i].GetComponent<Transform>().SetParent(this.GetComponent<Transform>());
            skyLights[i].AddComponent<Light>();
            Light lightComponent = skyLights[i].GetComponent<Light>();
            lightComponent.intensity = 1.0f / nbr_lights;
            lightComponent.type = LightType.Directional;
            lightComponent.color = Color.blue;
            lightComponent.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;
            lightComponent.shadows = LightShadows.Soft;
            lightComponent.renderMode = LightRenderMode.ForcePixel;

            skyLights[i].transform.localPosition = 10 * lightPos[i];
            skyLights[i].transform.LookAt(cube.transform);
        }


        Destroy(cube);

        mat.SetInt("_NbrSkyLights", nbr_lights);
    }
	
    private Vector3[] removeDuplicates(Vector3[] vec)
    {
        Vector3[] newVec = new Vector3[vec.Length];

        for(int i = 0; i < newVec.Length; i++)
        {
            newVec[i] = new Vector3(0, 0, 0);
        }

        for (int i = 0; i < newVec.Length; i++)
        {
            if(!isInVec(newVec, vec[i]))
            {
                newVec[i] = vec[i];
            }
        }

        int count = 0;
        for (int i = 0; i < newVec.Length; i++)
        {
            if (!(newVec[i] == new Vector3(0,0,0)))
            {
                count++;
            }
        }

        Vector3[] returnVec = new Vector3[count];
        count = 0;

        for (int i = 0; i < newVec.Length; i++)
        {
            if (!(newVec[i] == new Vector3(0, 0, 0)))
            {
                returnVec[count] = newVec[i];
                count++;
            }
        }

        return returnVec;
    }

    private bool isInVec(Vector3[] vec, Vector3 value)
    {
        for(int i = 0; i < vec.Length; i++)
        {
            if(vec[i] == value)
            {
                return true;
            }
        }

        return false;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
