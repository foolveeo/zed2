using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ARKitConversion : MonoBehaviour {

    private Matrix4x4 ARKitWorld_2_ARKitCam;
    //public Vector3 eulerAngles_ARKitCam_2_ZEDCam;
    
    struct TCPStrings
    {
        public Matrix4x4 ARWorld_2_ARCam;
        public Matrix4x4 ARCam_2_ZEDCam;
        public float azimuthAngle;
        public float zenithAngle;
    }
    
    public GameObject ZEDCamera;
    public GameObject ZEDWorldCoord;
    public GameObject tcpManagerARKit;
    public GameObject tcpManagerPython;
    public GameObject dirLight;
    public GameObject session;

    private string sessionID;

    private bool done;

    // computed runtime
    private Matrix4x4 ZEDCam_2_ZEDWorld;
    private Matrix4x4 ARKitCam_2_ZEDCam;
    private string ARKitWorld_2_ARKitCam_string;
    private string eulerAngles_ARKitCam_2_ZEDCam_string;
    private int i;
    private string tcpMsg;
    private string vgis8Folder;
    private string tcpFile;
    

    void Start ()
    {
        sessionID = session.GetComponent<session>().sessionID;

        vgis8Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        vgis8Folder = vgis8Folder.Remove(26);
        vgis8Folder += "VGIS8/Sessions/";

        tcpFile = vgis8Folder + sessionID + "/" + "ZED/tcpMessage.txt";

        string[] lines = File.ReadAllLines(tcpFile);


        tcpMsg = lines[0];

        TCPStrings tcpValues;
        tcpValues = setValues(tcpMsg);
        dirLight.GetComponent<SunDirectionAR>().azimuth = tcpValues.azimuthAngle;
        dirLight.GetComponent<SunDirectionAR>().zenith = tcpValues.zenithAngle;

        ARKitWorld_2_ARKitCam = tcpValues.ARWorld_2_ARCam;
        Matrix4x4 iPhoneOrientation = Matrix4x4.Inverse(Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90)));

        ARKitCam_2_ZEDCam = tcpValues.ARCam_2_ZEDCam;

        ZEDCam_2_ZEDWorld = Matrix4x4.Rotate(ZEDCamera.GetComponent<Transform>().transform.rotation);
        ARKitWorld_2_ARKitCam = Matrix4x4.Inverse(ARKitWorld_2_ARKitCam);
        ARKitCam_2_ZEDCam = Matrix4x4.Inverse(ARKitCam_2_ZEDCam);
        Matrix4x4 mat = ((iPhoneOrientation * ARKitWorld_2_ARKitCam) * ARKitCam_2_ZEDCam) * ZEDCam_2_ZEDWorld;

        ZEDWorldCoord.GetComponent<Transform>().transform.rotation = rotationFromMatrix(mat);
    }
	
	// Update is called once per frame
	void Update ()
    {

    }
    
    private TCPStrings setValues(string _string)
    {
        TCPStrings retVal;
        Matrix4x4 ARMat = new Matrix4x4();
        Matrix4x4 ZEDMat = new Matrix4x4();
        float azimuth;
        float zenith;

        string[] strings = _string.Split('?');
        string ARMatString = strings[1];
        string ZEDMatString = strings[0];
        string angles = strings[2];
        

        string[] ARMatRows = ARMatString.Split('!');
        string[] ZEDMatRows = ZEDMatString.Split('!');


        int i = 0;
        int j = 0;
        foreach (var row in ARMatRows)
        {
            if(row != "" && row != "\n")
            {
                string[] columns = row.Split('=');
                foreach (var col in columns)
                {
                    ARMat[i, j] = float.Parse(col);
                    j++;
                }
                i++;
                j = 0;
            }
        }



        i = 0;
        j = 0;
        foreach (var row in ZEDMatRows)
        {
            if (row != "" && row != "\n")
            {
                string[] columns = row.Split('=');
                foreach (var col in columns)
                {
                    ZEDMat[i, j] = float.Parse(col);
                    j++;
                }
                i++;
                j = 0;
            }
        }

        string[] azZen = angles.Split('!');
        azimuth = float.Parse(azZen[0]);
        zenith = float.Parse(azZen[1]);

        retVal.ARCam_2_ZEDCam = ZEDMat;
        retVal.ARWorld_2_ARCam = ARMat;
        retVal.zenithAngle = zenith;
        retVal.azimuthAngle = azimuth;


        return retVal;

    }

    private Quaternion rotationFromMatrix(Matrix4x4 mat)
    {
        
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + mat.m00 + mat.m11 + mat.m22)) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + mat.m00 - mat.m11 - mat.m22)) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - mat.m00 + mat.m11 - mat.m22)) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - mat.m00 - mat.m11 + mat.m22)) / 2;
        q.x *= Mathf.Sign(q.x * (mat.m21 - mat.m12));
        q.y *= Mathf.Sign(q.y * (mat.m02 - mat.m20));
        q.z *= Mathf.Sign(q.z * (mat.m10 - mat.m01));
        return q;

    }

    private Matrix4x4 stringToMatrix4x4(string matrixString)
    {
        Matrix4x4 mat4x4 = new Matrix4x4();
        
        string[] rows = matrixString.Split(';');

        int i = 0;
        int j = 0;
        foreach (var row in rows)
        {
            string[] columns = row.Split(',');
            foreach (var col in columns)
            {
                mat4x4[i, j] = float.Parse(col);
                j++;
            }
            i++;
            j = 0;
        }
        

        return mat4x4;
    }

    private Vector3 StringToEulerAngles(string str)
    {
        float x, y, z;
        
        char[] trim = { ']', ')', '(', '[', ';' };
        str.Trim(trim);
        string[] components = str.Split(',');

        x = float.Parse(components[0]);
        y = float.Parse(components[1]);
        z = float.Parse(components[2]);


        Vector3 eulAng = new Vector3(x, y, z);
        return eulAng;
    }
}
