using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ARKitConversion : MonoBehaviour {

    public Matrix4x4 ARKitWorld_2_ARKitCam;
    public Vector3 eulerAngles_ARKitCam_2_ZEDCam;
    
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

    public bool start;
    public bool stop;

    // computed runtime
    private Matrix4x4 ZEDCam_2_ZEDWorld;
    private Matrix4x4 ARKitCam_2_ZEDCam;
    private string ARKitWorld_2_ARKitCam_string;
    private string eulerAngles_ARKitCam_2_ZEDCam_string;
    private int i;
    private string tcpMsg;




    void Start ()
    {
        tcpMsg = "";
        

        print("Rotation matrix around y -90:\n");
        print(Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, -90, 0))).ToString());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(start)
        {

            tcpMsg = tcpManagerPython.GetComponent<Client>().receivedMsg;
            Debug.Log(tcpMsg);
            // arkit facing same world direction and zed rotated 
            //tcpMsg = "0.9639079 0.02977452 -0.2645658 0\t-0.03074987 0.999527 0.0004550568 0\t0.2644542 0.00769673 0.9643675 0\t0 0 0 1?1 0 0 0\t0 1 0 0\t0 0 1 0\t0 0 0 1";

            //tcpMsg = "1 0 0 0\t0 1 0 0\t1 0 0 0\t0 0 0 1?0 0 -1 0\t0 1 0 0\t1 0 0 0\t0 0 0 1";
            // identity transformation
            //tcpMsg = "1 0 0 0\t0 1 0 0\t0 0 1 0\t0 0 0 1?1 0 0 0\t0 1 0 0\t0 0 1 0\t0 0 0 1";

            //tcpMsg = "-0.06967151 0.009611169 -0.9975237 0\t-0.02294369 0.9996736 0.01123437 0\t0.9973061 0.02366959 -0.06942832 0\t0 0 0 1?0.70710 0.0000000 -0.7071068 0\t0.0000000 1.0000000 0.0000000 0\t0.7071068 0.0000000 0.7071068 0\t0 0 0 1";

            //tcpMsg = "-0.0171262 0.238426 -0.9710096 0\t0.9998026 -0.005695939 -0.01903269 0\t-0.01006874 -0.971144 -0.2382814 0\t0 0 0 1?0.993546989 0.054356941 -0.099547494 0.020323315\t-0.041275985 0.990779334 0.129044971 0.057290808\t0.105644090 -0.124103322 0.986629460 -0.067694663\t0.000000000 0.000000000 0.000000000 1.000000000";
            if (tcpMsg != "")
            {
                TCPStrings tcpValues;
                tcpValues = setValues(tcpMsg);
                dirLight.GetComponent<SunDirectionAR>().azimuth = tcpValues.azimuthAngle;
                dirLight.GetComponent<SunDirectionAR>().zenith = tcpValues.zenithAngle;

                ARKitWorld_2_ARKitCam = tcpValues.ARWorld_2_ARCam;
                Matrix4x4 iPhoneOrientation = Matrix4x4.Inverse( Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90)));
                
                ARKitCam_2_ZEDCam = tcpValues.ARCam_2_ZEDCam;

                ZEDCam_2_ZEDWorld = Matrix4x4.Rotate(ZEDCamera.GetComponent<Transform>().transform.rotation);
                ARKitWorld_2_ARKitCam = Matrix4x4.Inverse(ARKitWorld_2_ARKitCam);
                ARKitCam_2_ZEDCam = Matrix4x4.Inverse(ARKitCam_2_ZEDCam);
                Matrix4x4 mat = ((iPhoneOrientation * ARKitWorld_2_ARKitCam) * ARKitCam_2_ZEDCam) * ZEDCam_2_ZEDWorld;

                ZEDWorldCoord.GetComponent<Transform>().transform.rotation = rotationFromMatrix(mat);
            }
            
            if(stop)
            {
                start = false;
                ZEDWorldCoord.GetComponent<Transform>().transform.rotation = new Quaternion();
            }
        }
        else
        {

        }

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
