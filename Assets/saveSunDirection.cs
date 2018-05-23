using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class saveSunDirection : MonoBehaviour {
    public GameObject zed;
    public GameObject worldARKit;
    public GameObject sun;
    public string sessionID;

    public bool save;

    private string vgis8Folder;
    private string folderPath;
	// Use this for initialization
	void Start () {

        vgis8Folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        vgis8Folder = vgis8Folder.Remove(26);
        vgis8Folder += "VGIS8/";

        folderPath = vgis8Folder + "Sessions/" + sessionID + "sunDirection/";

    }
	
	// Update is called once per frame
	void Update () {

        Matrix4x4 ARworld2ZedWorld = zed.GetComponent<Transform>().localToWorldMatrix;
        float azimuth = sun.GetComponent<SunDirectionAR>().azimuth;
        float zenith = sun.GetComponent<SunDirectionAR>().zenith;

        Vector4 g = new Vector4(0, -1, 0, 0);
        Vector4 sunDir = new Vector4();
        Vector4 sunDirView = new Vector4();

        sunDir.x = Mathf.Sin(Mathf.Deg2Rad * azimuth);
        sunDir.z = Mathf.Cos(Mathf.Deg2Rad * azimuth);
        sunDir.y = Mathf.Cos(Mathf.Deg2Rad * zenith);
        sunDir.w = 0.0f;

        Matrix4x4 zedWorld2zedCam = zed.GetComponent<Transform>().worldToLocalMatrix;
        Matrix4x4 ARworld2ZedCam = ARworld2ZedWorld * zedWorld2zedCam;

        sunDirView = ARworld2ZedCam * sunDir;

        string sunString = "x: " + sunDirView.x.ToString() + "\ny: " + sunDirView.y.ToString() + "\nz: " + sunDirView.z.ToString();
        if (save)
        {
            string filePath = folderPath + "sunDirection_" + Time.frameCount.ToString() + ".txt";
            System.IO.File.WriteAllText(filePath, sunString);
        }
	}
}
