using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createSkyLightSources : MonoBehaviour {

    public int nbr_lights;
    public int nbr_layers;
    private GameObject[] skyLights;
    private GameObject cube;
    private GameObject plane;
    public Material mat;

	// Use this for initialization
	void Start () {

        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        cube.GetComponent<Transform>().SetParent(this.GetComponent<Transform>());
        plane.GetComponent<Transform>().SetParent(this.GetComponent<Transform>());

        cube.GetComponent<MeshRenderer>().material = mat;
        plane.GetComponent<MeshRenderer>().material = mat;

        cube.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        plane.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        cube.GetComponent<MeshRenderer>().receiveShadows = true;
        plane.GetComponent<MeshRenderer>().receiveShadows = true;


        cube.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        plane.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);

        skyLights = new GameObject[nbr_lights];

        int nbr_light_per_layer = (nbr_lights - 1) / nbr_layers;
        float angle_bw_lights_in_same_layer = Mathf.PI * 2 / nbr_light_per_layer;
        float angle_bw_different_layers = Mathf.PI / (2 * (nbr_layers + 1));
        float phase = angle_bw_lights_in_same_layer / nbr_layers;
        float x, y, z, phi, theta;

        for (int i=0; i < nbr_lights; i++)
        {
            skyLights[i] = new GameObject("skyLight_" + i.ToString());
            skyLights[i].GetComponent<Transform>().SetParent(this.GetComponent<Transform>());
            skyLights[i].AddComponent<Light>();
            Light lightComponent = skyLights[i].GetComponent<Light>();
            lightComponent.intensity = 1.0f / nbr_lights;
            lightComponent.type = LightType.Directional;
            lightComponent.color = Color.white;
            lightComponent.shadowResolution = UnityEngine.Rendering.LightShadowResolution.VeryHigh;
            lightComponent.shadows = LightShadows.Soft;
            lightComponent.renderMode = LightRenderMode.ForcePixel;



            if(i == 0)
            {
                x = 0;
                y = 100;
                z = 0;
            }
            else
            {
                int layer_index = (i - 1) / nbr_light_per_layer;
                int light_index_in_layer = (i - 1) % nbr_light_per_layer;

                theta = angle_bw_lights_in_same_layer * light_index_in_layer + phase * layer_index;
                phi = angle_bw_different_layers * (layer_index + 1);

                x = 100 * Mathf.Cos(phi) * Mathf.Cos(theta);
                y = 100 * Mathf.Sin(phi);
                z = 100 * Mathf.Cos(phi) * Mathf.Sin(theta);
            }

            skyLights[i].transform.localPosition = new Vector3(x, y, z);
            skyLights[i].transform.LookAt(cube.transform);

            


        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
