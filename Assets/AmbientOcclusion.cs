using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class AmbientOcclusion : MonoBehaviour {


    public Material aoMaterial;
    public Material zedMaterial;
    //public Camera zed;
    private Texture2D depth;
    private Texture2D normals;
    private Color[] pixels;
    private sl.ZEDCamera zed;

    private void Start()
    {

        depth = new Texture2D(Screen.width, Screen.height);
        normals = new Texture2D(Screen.width, Screen.height);
        
        //depth = zed.CreateTextureImageType(sl.VIEW.DEPTH);
        //normals = zed.CreateTextureImageType(sl.VIEW.NORMALS);
        aoMaterial.SetTexture("_Depth", depth);
        aoMaterial.SetTexture("_Normals", normals);

    }

    private void Update()
    {
        
        zed = sl.ZEDCamera.GetInstance();
        if(zed != null)
        {
            depth = zed.CreateTextureImageType(sl.VIEW.DEPTH);
            normals = zed.CreateTextureImageType(sl.VIEW.NORMALS);
            aoMaterial.SetTexture("_Depth", depth);
            aoMaterial.SetTexture("_Normals", normals);

        }
        
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // the _MainTex of aoMaterial is source!
        Graphics.Blit(source, destination, aoMaterial);
        
    }
}
