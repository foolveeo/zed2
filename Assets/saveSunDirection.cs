using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveSunDirection : MonoBehaviour {
    public GameObject zed;
    public GameObject worldARKit;
    public GameObject sun;
    public GameObject session;

    public bool save;

    private string vgis8Folder;
    private string folderPath;
    private string sessionID;


	// Use this for initialization
	void Start () {
        sessionID = session.GetComponent<session>().sessionID;
        vgis8Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        vgis8Folder = vgis8Folder.Remove(26);
        vgis8Folder += "VGIS8/";

        folderPath = vgis8Folder + "Sessions/" + sessionID + "/sunDirection/";

    }
	
	// Update is called once per frame
	void Update () {

        Matrix4x4 ARworld2ZedWorld = Matrix4x4.Rotate(worldARKit.GetComponent<Transform>().rotation);
        Debug.Log(ARworld2ZedWorld.ToString());
        float azimuth = sun.GetComponent<SunDirectionAR>().azimuth;
        float zenith = sun.GetComponent<SunDirectionAR>().zenith;
        
        Vector4 sunDir = new Vector4();
        Vector4 sunDirView = new Vector4();

        sunDir.x = Mathf.Sin(Mathf.Deg2Rad * zenith) * Mathf.Sin(Mathf.Deg2Rad * azimuth);
        sunDir.z = Mathf.Sin(Mathf.Deg2Rad * zenith) * Mathf.Cos(Mathf.Deg2Rad * azimuth);
        sunDir.y = Mathf.Cos(Mathf.Deg2Rad * zenith);
        sunDir.w = 0.0f;


        Debug.Log(sunDir.ToString());
        Matrix4x4 zedWorld2zedCam = zed.GetComponent<Transform>().worldToLocalMatrix;
        Matrix4x4 ARworld2ZedCam = zedWorld2zedCam * ARworld2ZedWorld;
        Debug.Log(zedWorld2zedCam.ToString());
        sunDirView = ARworld2ZedCam * sunDir;
        Debug.Log(sunDirView.ToString());

        string sunString = sunDirView.x.ToString() + "?" + sunDirView.y.ToString() + "?" + sunDirView.z.ToString();
        if (save)
        {
            string filePath = folderPath + "sunDirection_" + Time.frameCount.ToString() + ".txt";
            System.IO.File.WriteAllText(filePath, sunString);
        }
	}
}
