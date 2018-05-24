using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class screenshots : MonoBehaviour
{

    public byte[] bytesPNG;
    public bool saveColor;
    public bool saveDepth;
    public bool saveNormal;
    public bool saveShadow;
    public GameObject session;
    private string sessionID;
    private string fileName;
    string vgis8Folder;

    // Take a shot immediately
    IEnumerator Start()
    {
        sessionID = session.GetComponent<session>().sessionID;
        vgis8Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        vgis8Folder = vgis8Folder.Remove(26);
        vgis8Folder += "VGIS8/Sessions/";
        fileName = vgis8Folder + sessionID + "/";
        if (saveColor)
        {

            fileName += "images/color/rgb_";
        }
        else
        {
            if(saveDepth)
            {
                fileName += "images/depth/depth_";
            }
            else
            {
                if (saveNormal)
                {
                    fileName += "images/normals/normals_";
                }
                else
                {
                    fileName += "images/shadows/shadows_";
                }
            }
        }
        bytesPNG = new byte[16777216];
        StartCoroutine(UploadPNG());
        yield return null;
    }


    void Update()
    {
        StartCoroutine(UploadPNG());
    }

    void saveScreenshot()
    {
        string nameScreenShot = fileName + Time.frameCount.ToString() + ".png";
        File.WriteAllBytes(nameScreenShot, bytesPNG);
    }

    private IEnumerator UploadPNG()
    {   
        
        // We should only read the screen buffer after rendering is complete
        yield return (new WaitForEndOfFrame());

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;

        
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        
        // Encode texture into PNG
        bytesPNG = tex.EncodeToPNG();
        Destroy(tex);
        
        
       
        if (saveDepth || saveColor || saveNormal || saveShadow)
        {
            saveScreenshot();
        }
        
        yield return null;
    }

}





