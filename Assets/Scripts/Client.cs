using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion
    
    public string ip;
    public int port;
    public string receivedMsg;
    public Button sendCheckerboardBtn;
    public GameObject recorder;
    private byte[] bytesScreenshotPNG;
    private string savedReceivedMsg;

    // Use this for initialization 	
    void Start()
    {
        receivedMsg = "";
        ConnectToTcpServer();
        bytesScreenshotPNG = new byte[16777216];
        if (sendCheckerboardBtn != null)
        {
            sendCheckerboardBtn.onClick.AddListener(SendCheckerboardScreenShot);
        }
    }
    // Update is called once per frame
    void Update()
    {
        bytesScreenshotPNG = recorder.GetComponent<screenshots>().bytesPNG;
        receivedMsg = savedReceivedMsg;
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient(ip, port);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incomingData = new byte[length];
                        Array.Copy(bytes, 0, incomingData, 0, length);
                        // Convert byte array to string message. 						
                        savedReceivedMsg = Encoding.UTF8.GetString(incomingData);
                        Debug.Log("server message received as: " + savedReceivedMsg);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    


    private void SendCheckerboardScreenShot()
    {
        Debug.Log("Sending screenshot");
        if (socketConnection == null)
        {
            
            Debug.Log("not really...no TCP connection");
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.                 
                stream.Write(bytesScreenshotPNG, 0, bytesScreenshotPNG.Length);
                Debug.Log("Client sent screenshot, size is " + bytesScreenshotPNG.Length.ToString());
              
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
