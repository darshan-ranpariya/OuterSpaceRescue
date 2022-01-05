using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using UnityEngine.UI;

public class LocalTCPClient : MonoBehaviour
{
    public enum State { Uninitialized, Connecting, Connected, Failed, Disconnected }
    //public int my_unique_index = 0;

    public State state = State.Uninitialized;
    public int port = 61301;
    public bool connected { get { return state == State.Connected; } }
    TcpClient clientSocket = null; 
    public event Action<bool> ConnectionStateChanged;
    public event Action<string> MessageReceived;
    public event Action<string[]> ArrayMessageReceived;
    public event Action<string[]> BroadcastReceived;
    public bool connectOnEnable = false; 
    public bool quitOnDisconnect = false;

    public List<string> pendingMsgs = new List<string>();


    void OnEnable()
    {
        if (connectOnEnable) Connect();
        StartCoroutine("p");
    }

    public void Connect()
    {
        if (connected) return;

        state = State.Connecting;
        clientSocket = new TcpClient();
        try
        {
            clientSocket.Connect("127.0.0.1", port);
            state = State.Connected;
            Log("pendingMsgs {0}", pendingMsgs.Count);
            foreach (var m in pendingMsgs) Send(m);
            pendingMsgs.Clear();
            Log("Client Started "+port);
        }
        catch (Exception)
        {
            Log("Failed to Start Client");
            state = State.Failed;
        }
        FireConnectionStateChange();
    }

    public void Disconnect()
    {
        if (clientSocket != null)
        {
            Send("c");
            clientSocket.Close();
            clientSocket = null;
            state = State.Disconnected;
            FireConnectionStateChange();
            Log("Close");
        }
    }

    StringBuilder strBuffer = new StringBuilder();
    StringBuilder sb = new StringBuilder();
    void Update()
    {
        if (!connected) return;

        if (clientSocket != null && !clientSocket.Connected)
        {
            Disconnect(); 
            return;
        } 

        if (clientSocket!=null && clientSocket.Connected && clientSocket.Available > 0)
        {
            //Log("Available Bytes: " + clientSocket.Available);

            NetworkStream networkStream = clientSocket.GetStream();

            Byte[] data = new Byte[clientSocket.Available];
            Int32 bytes = networkStream.Read(data, 0, data.Length);
            //strBuffer.Append(Encoding.ASCII.GetString(data, 0, bytes));
            strBuffer.Append(LocalTCP.Decode(data));
        }
        if (strBuffer.Length > 0)
        {
            sb = new StringBuilder();
            for (int i = 0; i < strBuffer.Length; i++)
            {
                if (strBuffer[i].Equals(LocalTCP.terminatingChar))
                {
                    strBuffer.Remove(0, i+1);
                    string m = LocalTCP.Decrypt(sb.ToString());
                    Log(DateTime.Now.ToShortTimeString() + " Message Received : " + m);
                    if (m.Equals("c"))
                    {
                        Disconnect();
                    }
                    else if(m.Equals("p"))
                    {
                        //just server hitting this client to verify
                    }
                    else
                    {
                        string[] sa = LocalTCP.ArrayFromJson(m);
                        if (sa != null)
                        {
                            if (sa.Length > 0 && sa[0].Equals("__bc__"))
                            {
                                string[] sab = new string[sa.Length - 1];
                                for (int sai = 1; sai < sa.Length; sai++) sab[sai - 1] = sa[sai];
                                if (BroadcastReceived != null) BroadcastReceived(sab);
                            }
                            else
                            {
                                if (ArrayMessageReceived != null) ArrayMessageReceived(sa);
                            }
                        }
                        if (MessageReceived != null) MessageReceived(m); 
                    }
                    break;
                }
                else sb.Append(strBuffer[i]);
            }
        }
    }

    void OnDisable()
    {
        Disconnect();
    }

    public void Broadcast(params string[] strings)
    {
        string[] ss = new string[strings.Length+1];
        ss[0] = "__bc__";
        for (int i = 0; i < strings.Length; i++)
        {
            ss[i + 1] = strings[i];
        }
        Send(LocalTCP.ArrayToJson(ss));
    }

    public void SendArray(params string[] strings)
    { 
        Send(LocalTCP.ArrayToJson(strings));
    }

    public void Send(string msg)
    {
        if (!connected)
        {
            pendingMsgs.Add(msg);
            if (state == State.Uninitialized) Connect();
            return;
        }

        try
        {
            msg = msg.Replace(LocalTCP.terminatingChar, LocalTCP.terminatingCharReplacement);
            Byte[] data = LocalTCP.Encode(LocalTCP.Encrypt(msg));
            if (clientSocket != null && clientSocket.Connected)
            {
                NetworkStream stream = clientSocket.GetStream();
                stream.Write(data, 0, data.Length);
                if(data.Length > 2) Log("Sent " + msg);
            }
        }
        catch (Exception e)
        {
            Log("Local Tcp Client : Failed to send msg " + e.Message); 
        }
    }



    void FireConnectionStateChange()
    {
        if (ConnectionStateChanged != null) ConnectionStateChanged(connected);
        if (quitOnDisconnect && state == State.Disconnected)
        {
            QuitApplication();
        }
    }
    void QuitApplication()
    {
        Application.Quit();
        //new Delayed.Action(() =>
        //{
        //    if (Application.isPlaying)
        //    {
        //        QuitApplication();
        //    }
        //}, .5f);
    } 
    IEnumerator p()
    {
        while (true)
        {
            if (enabled && connected)
            {
                //Send("p");
            }
            yield return new WaitForSeconds(1);
        }
    }

    public Text debugText;
    List<string> logs = new List<string>();
    void Log(string s, params object[] args)
    {
        return;
        s = string.Format(s, args);
        if (debugText!=null)
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
