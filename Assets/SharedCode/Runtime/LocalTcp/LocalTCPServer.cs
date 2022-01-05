using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine.UI; 

//pubilc int m_index = 0;

//define client struct
//{ 

// pubilc int nUnique_index = 0;
// string szGameName = "";
// port = 0;
// TcpClient ClientSocket = NULL;
// StringBuilder strBuffer = NULL;
//} 

public class LocalTCPServer : MonoBehaviour
{
    public int port = 61301;
    TcpListener serverSocket = null;
    int clientsCount;
    public List<TcpClient> clientSockets = new List<TcpClient>(); 
    List<StringBuilder> clientStrBuffers = new List<StringBuilder>(); 
    public event Action<string> MessageReceived;
    public event Action<string, int, LocalTCPServer> MessageReceivedFromClient;
    public event Action<string[], int, LocalTCPServer> ArrayMessageReceivedFromClient;
    public event Action<string[]> BroadcastReceived;

    void OnEnable()
    {
        try
        {
            serverSocket = new TcpListener(port);
            serverSocket.Start();
            StartCoroutine(Clean_c());
            enabled = true;
            Log("Server Started");
        }
        catch (Exception e)
        {
            Log("Failed to start local server : " + e.Message);
            enabled = false;
        }
    }

    int available = 0;
    StringBuilder sb = new StringBuilder(); 
    void Update()
    {
        //if there is any waiting then accept....
        if (serverSocket.Pending())
        {
            AddClient(serverSocket.AcceptTcpClient());  
            Log("Accepted connection from client");
        }
         
        for (int i = clientSockets.Count-1; i >= 0; i--)        
        {
            if (clientSockets[i] == null) continue;
            if (clientStrBuffers[i] == null) continue;

            available = 0;
            try
            {
                available = clientSockets[i].Available;
            }
            catch
            {
                RemoveClient(i);
                continue;
            }

            if (available > 0)
            {
                NetworkStream networkStream = clientSockets[i].GetStream();

                //Byte[] data = new Byte[1024];
                Byte[] data = new Byte[available];
                Int32 bytes = networkStream.Read(data, 0, data.Length);
                //clientStrBuffers[i].Append(Encoding.ASCII.GetString(data, 0, bytes));
                clientStrBuffers[i].Append(LocalTCP.Decode(data));
            }
            if (clientStrBuffers[i].Length > 0)
            {
                sb = new StringBuilder();
                for (int s = 0; s < clientStrBuffers[i].Length; s++)
                {
                    if (clientStrBuffers[i][s].Equals(LocalTCP.terminatingChar))
                    {
                        clientStrBuffers[i].Remove(0, s + 1);
                        string m = LocalTCP.Decrypt(sb.ToString());
                        if (m.Equals("c"))
                        {
                            RemoveClient(i);
                            continue;
                        }
                        else if (m.Equals("p"))
                        {
                            
                        }
                        else 
                        {
                            Log("Message Received : " + m);

                            string[] sa = LocalTCP.ArrayFromJson(m);
                            if (sa != null)
                            {
                                if (sa.Length > 0 && sa[0].Equals("__bc__"))
                                {
                                    if (BroadcastReceived != null)
                                    {
                                        string[] sab = new string[sa.Length - 1];
                                        for (int sai = 1; sai < sa.Length; sai++) sab[sai - 1] = sa[sai];
                                        BroadcastReceived(sab);
                                    }
                                    SendArray(sa);
                                }
                                else
                                {
                                    if (ArrayMessageReceivedFromClient != null) ArrayMessageReceivedFromClient(sa, i, this);
                                }
                            }
                            
                            if (MessageReceived != null) MessageReceived(m);
                            if (MessageReceivedFromClient != null) MessageReceivedFromClient(m, i, this); 
                        }
                        break;
                    }
                    else sb.Append(clientStrBuffers[i][s]);
                }
            }
        } 
    }

    IEnumerator Clean_c()
    {
        while (true)
        {
            Send("p");
            yield return new WaitForSeconds(2);
        }
    }

    void OnDisable()
    {
        Send("c");

        for (int i = 0; i < clientSockets.Count; i++)
        {
            if (clientSockets[i] != null)
                clientSockets[i].Close();
        }

        if (serverSocket != null)
        {
            serverSocket.Stop();
        }

        Log("Stop"); 
    }

    public void Broadcast(params string[] strings)
    {
        if (clientsCount == 0) return;

        string[] sab = new string[strings.Length + 1];
        sab[0] = "__bc__";
        for (int sai = 0; sai < strings.Length; sai++) sab[sai + 1] = strings[sai];
        SendArray(sab);
    } 

    public void SendArray(params string[] strings)
    {
        if (clientsCount == 0) return;

        for (int i = 0; i < clientSockets.Count; i++)
            SendArray(i, strings);
    }
    public bool SendArray(int clientIndex, params string[] strings)
    {
        string msg = LocalTCP.ArrayToJson(strings);
        msg = msg.Replace(LocalTCP.terminatingChar, LocalTCP.terminatingCharReplacement);
        Byte[] b = LocalTCP.Encode(LocalTCP.Encrypt(msg));
         
        return Send(clientIndex, b);
    }

    public void Send(string msg)
    {
        if (clientsCount == 0) return;

        Log(string.Format(DateTime.Now.ToShortTimeString() + " Sending All: {0}", msg));
        Log(string.Format(DateTime.Now.ToShortTimeString() + " Sending All Enc: {0}", LocalTCP.Encrypt(msg)));

        msg = msg.Replace(LocalTCP.terminatingChar, LocalTCP.terminatingCharReplacement);
        Byte[] b = LocalTCP.Encode(LocalTCP.Encrypt(msg));

        for (int i = 0; i < clientSockets.Count; i++)
            Send(i, b);
    }

    public bool Send(int clientIndex, string msg)
    {
        if (clientsCount == 0) return false;
        
        Log(string.Format(DateTime.Now.ToShortTimeString() + " Sending: {0}", msg));
        msg = msg.Replace(LocalTCP.terminatingChar, LocalTCP.terminatingCharReplacement);
        Byte[] b = LocalTCP.Encode(LocalTCP.Encrypt(msg));
        return Send(clientIndex, b);
    }

    public bool Send(int clientIndex, Byte[] data)
    {
        bool sent = false;
        if (clientSockets.Count > clientIndex && clientSockets[clientIndex] != null)
        {
            try
            {
                NetworkStream networkStream = clientSockets[clientIndex].GetStream();

                // NetworkStream networkStream =  GetClient(clientIndex).GetStream(); 

                networkStream.Write(data, 0, data.Length);
                networkStream.Flush();
                //Log(string.Format(DateTime.Now.ToShortTimeString() + " Sent\nto: {0}\nmsg: {1}", clientIndex, LocalTCP.Decode(data)));
                sent = true;
            }
            catch (Exception e)
            {
                Log("connection client {0} lost: {1}", clientIndex, e.Message);
                //Log(e.GetType().ToString() + e.Message);
                RemoveClient(clientIndex);
            }
        }

        if (!sent) clientSockets[clientIndex] = null;
        return sent;
    }

    void AddClient(TcpClient c)
    {
        if (c == null) return;

        clientSockets.Add(c);
        clientStrBuffers.Add(new StringBuilder());

        clientsCount++;
        Log("Client Added: " + (clientSockets.Count-1));
    }

    void RemoveClient(int i)
    {
        clientSockets[i] = null;
        clientStrBuffers[i] = null;
        clientsCount--;
        Log("Client Removed: " + i);
    }

    /*
    //Client GetClient(int index)
    //{
    //    for (int i =0 ; i < clientSockets.Count ; i++)
    //    {
             if(clientSockets[i].Client.index == index)
             {  
               return clientSockets[i].Client;
               }

    //    clientSockets.RemoveAt(i);
    //    clientStrBuffers.RemoveAt(i);
    //    Log("Client Removed: " + i);
    //}
    */

    public Text debugText;
    List<string> logs = new List<string>();
    void Log(string s, params object[] args)
    {
        return;
        s = string.Format(s, args);
        if (debugText != null)
        {
            logs.Add(s);
            StringBuilder sb = new StringBuilder();
            int m = logs.Count - 1;
            if (m > 10) m = 10;
            for (int i = m; i >= 0; i--)
            {
                sb.Append(logs[i]);
                sb.Append("\n");
            }
            debugText.text = sb.ToString();
        }
        Debug.Log("Server : " + s);
    } 
}