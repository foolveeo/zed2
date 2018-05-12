using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class screenshots : MonoBehaviour
{

    public byte[] bytesPNG;
    public bool isSavingScreenShots;

    // Take a shot immediately
    IEnumerator Start()
    {
        bytesPNG = new byte[522834];
        isSavingScreenShots = false;
        StartCoroutine(UploadPNG());
        yield return null;
    }


    void Update()
    {

        StartCoroutine(UploadPNG());
    }

    void saveScreenshot()
    {
        // Create a Web Form
        string nameScreenShot = "rgb" + Time.frameCount.ToString() + ".png";
        File.WriteAllBytes(Application.dataPath + "/imgs/" + nameScreenShot, bytesPNG);
        print(Application.dataPath);
    }

    private IEnumerator UploadPNG()
    {
        
        // We should only read the screen buffer after rendering is complete
        yield return (new WaitForEndOfFrame());

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;


        Debug.Log("screen width: " + Screen.width.ToString() + "screen height: " + Screen.height.ToString());

        Debug.Log("Called UploadPng");

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        bytesPNG = tex.EncodeToPNG();
        Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        //
        if(isSavingScreenShots)
        {
            saveScreenshot();
        }
        
        yield return null;
    }

}





