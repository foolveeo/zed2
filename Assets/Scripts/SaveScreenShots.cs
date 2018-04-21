using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class saveScreenShots : MonoBehaviour {




	// Take a shot immediately
	IEnumerator Start()
	{
        StartCoroutine(UploadPNG());
        yield return null;
    }


	void Update()
	{

		StartCoroutine (UploadPNG ());
	}



	private IEnumerator UploadPNG()
	{
		// We should only read the screen buffer after rendering is complete
		yield return (new WaitForEndOfFrame());

		// Create a texture the size of the screen, RGB24 format
		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();

		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		Destroy(tex);

		// For testing purposes, also write to a file in the project folder
		// 


		// Create a Web Form
		string nameScreenShot = "shadowMap" + Time.frameCount.ToString() + ".png";
		File.WriteAllBytes(Application.dataPath + "/imgs/" + nameScreenShot, bytes);
		print (Application.dataPath);


		yield return null;
	}
		
}




