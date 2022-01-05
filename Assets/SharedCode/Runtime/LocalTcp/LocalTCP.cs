using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class LocalTCP
{
    public static System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(1252);

    public static int port = 61305;
    public static char terminatingChar = '|';
    public static char terminatingCharReplacement = 'I';
    static string EncryptionKey = "DJ_BR@V0!";

    public static Byte[] Encode(string str, bool addTerminatingChar = true)
    {
        try
        { 
            if (addTerminatingChar) str += terminatingChar;
            return encoding.GetBytes(str);
        }
        catch
        {
            return new Byte[0];
        }
    }

    public static string Decode(Byte[] bytes)
    {
        string s = string.Empty;
        try
        {
            s = encoding.GetString(bytes, 0, bytes.Length); 
        }
        catch
        { 
        }

        return s;
    }

    public static string Encrypt(string clearText)
    {
        byte[] clearBytes = encoding.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public static string Decrypt(string cipherText)
    {
        cipherText = cipherText.Replace(" ", "+");
        try
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = encoding.GetString(ms.ToArray());
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("Decrypt Failed: {0}\n{1}", cipherText, e.Message);
        } 
        return cipherText;
    }

    public static string ArrayToJson(string[] strings)
    {
        return JsonUtility.ToJson(new ArrayMessage(strings));
    }
    public static string[] ArrayFromJson(string s)
    {
        try
        {
            ArrayMessage am = JsonUtility.FromJson<ArrayMessage>(s);
            if (am != null)
            {
                return am.strings;
            }
        }
        catch { }
        return null;
    }
    [Serializable]
    public class ArrayMessage
    {
        public string[] strings;
        public ArrayMessage(params string[] sa)
        {
            strings = sa;
        }
    }

    //public static TcpState GetState(this TcpClient tcpClient)
    //{
    //    var foo = IPGlobalProperties.GetIPGlobalProperties()
    //      .GetActiveTcpConnections()
    //      .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint));
    //    return foo != null ? foo.State : TcpState.Unknown;
    //}
}
