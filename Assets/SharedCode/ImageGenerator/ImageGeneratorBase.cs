using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageGeneratorBase : MonoBehaviour
{
    string _directory = string.Empty;
    string directory
    {
        get
        {
            if (string.IsNullOrEmpty(_directory))
            {
                _directory = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "ImageGeneratorOutput");
                if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);
            }
            return _directory;
        }
    }

    public string fileNameFormat = "{0}.png";
    public Vector2 res;
    public void Generate(object arg)
    {
        if (res.x < 1 || res.y < 1) res = new Vector2(Screen.height, Screen.width);
        string path = Path.Combine(directory, string.Format(fileNameFormat, arg));
        //ScreenCapture.CaptureScreenshot(path);
        TakeScreenshot(path);
        Debug.Log(path); 
    }

    public string TakeScreenshot(string path)
    {
        Camera myCamera = Camera.main;
        int resWidthN = (int)res.x;
        int resHeightN = (int)res.y;
        RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
        myCamera.targetTexture = rt;

        TextureFormat tFormat; 
        tFormat = TextureFormat.ARGB32;

        Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
        myCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
        myCamera.targetTexture = null;
        RenderTexture.active = null;
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = path;

        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        return filename;
    }
}
