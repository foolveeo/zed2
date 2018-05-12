using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Server : MonoBehaviour {

    #region private members 	
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    #endregion

    public string receivedMsg;
    public string msgToSend;
    private string savedReceivedMsg;
    private string savedMsgToSend;    

    public string ip;
    public int port;

    
    void Start()
    {
        savedMsgToSend = null;
        savedReceivedMsg = null;
        receivedMsg = null;


        // Start TcpServer background thread 		
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        //tcpListenerThread.IsBackground = true; 		
        tcpListenerThread.Start();

    }

    // Update is called once per frame
    void Update()
    {
        if(savedReceivedMsg != null)
        {
            if(savedReceivedMsg != receivedMsg)
            {
                receivedMsg = savedReceivedMsg;
            }
        }
        if(savedMsgToSend == null)
        {
            if (msgToSend != null)
            {
                savedMsgToSend = msgToSend;
                SendMessage();
            }
            else
            {
                return;
            }
            
        }
        else
        {
            if(savedMsgToSend != msgToSend)
            {
                savedMsgToSend = msgToSend;
                SendMessage();
            }
            else
            {
                return;
            }

        }
    }

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            tcpListener = new TcpListener(IPAddress.Parse(ip), port);
            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);
                            // Convert byte array to string message. 							
                            savedReceivedMsg = Encoding.UTF8.GetString(incomingData);
                            Debug.Log("client message received as: " + savedReceivedMsg);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    /// <summary> 	
    /// Send message to client using socket connection. 	
    /// </summary> 	
    private void SendMessage()
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite && savedMsgToSend != null)
            {
                string serverMessage = savedMsgToSend;
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.UTF8.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
}
