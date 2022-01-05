#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using UnityEditor.Callbacks;
using System.IO;

[CreateAssetMenu()]
public class BuildPreset : ScriptableObject
{
    [Serializable]
    public class SceneSettings
    {
        public string sceneName;
        public string scenePath;
        public bool includeInBuild = true;
    }

    public string companyName;
    public string productName;
    public string bundleId;
    public Texture2D icon;
    public BuildTarget targetPlatform;
    public string version;
    public int buildNumber;

    public bool desktopMode;
    public bool multiWindow;
    public bool storeVersion;
    public bool stringBuildChanged;

    public ResolutionDialogSetting resolutionDialog;
    public bool fullScreen;
    public bool resizableWindow;
    public bool forceSingleInstance;
    public int defaultScreenWidth;
    public int defaultScreenHeight;

    public SceneSettings[] scenes;

    public string filePath;
    public string fileSubPath;
    public string fileName;

    //public ServerConfig serverConfig;

    public string buildExt
    {
        get
        {
            switch (targetPlatform)
            {
                case BuildTarget.Android:
                    return ".apk";
                case BuildTarget.StandaloneWindows:
                    return ".exe";
                default:
                    return string.Empty;
            }
        }
    }

    public void Apply()
    {
        //GameStatics.MultiWindowSupported = multiWindow;
        //GameStatics.DesktopMode = desktopMode;
        if (!string.IsNullOrEmpty(companyName)) PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        if (!bundleId.IsNullOrEmpty()) PlayerSettings.applicationIdentifier = bundleId;
        PlayerSettings.bundleVersion = version;

        PlayerSettings.displayResolutionDialog = resolutionDialog;
        PlayerSettings.defaultIsFullScreen = fullScreen;
        PlayerSettings.resizableWindow = resizableWindow;
        PlayerSettings.forceSingleInstance = forceSingleInstance;
        if (defaultScreenWidth > 0) PlayerSettings.defaultScreenWidth = defaultScreenWidth;
        if (defaultScreenHeight > 0) PlayerSettings.defaultScreenHeight = defaultScreenHeight;

        List<string> defines = new List<string>();
        BuildTargetGroup targetGroup = BuildTargetGroup.Standalone;

        switch (targetPlatform)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                targetGroup = BuildTargetGroup.Standalone;
                defines.Add("DESKTOP_MODE");
                break;
            case BuildTarget.Android:
                targetGroup = BuildTargetGroup.Android;
                PlayerSettings.Android.bundleVersionCode = buildNumber;
                break;
            default:
                break;
        }

        if (multiWindow) defines.Add("MULTI_WINDOW_MODE");
        if (storeVersion) defines.Add("STORE_MODE");
        if (stringBuildChanged) defines.Add("STRING_BUILD_CHANGE");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, string.Join(";", defines.ToArray()));
        if (icon != null) PlayerSettings.SetIconsForTargetGroup(targetGroup, new Texture2D[] { icon, icon, icon, icon, icon, icon }, IconKind.Any);
    }

    public void Build(bool run)
    {
        Apply();

        List<string> _scenes = new List<string>();
        for (int i = 0; i < scenes.Length; i++)
        {
            if (scenes[i].includeInBuild)
            {
                _scenes.Add(string.Format("Assets/{0}/{1}.unity", scenes[i].scenePath, scenes[i].sceneName));
            }
        }

        if (string.IsNullOrEmpty(filePath)) filePath = EditorUtility.SaveFolderPanel("Choose Root Location of all Builds", Application.dataPath, "");
        string _path = string.Empty;
        switch (targetPlatform)
        {
            case BuildTarget.Android:
                _path = string.Format("{0}/{1}[{3}]{2}", filePath, fileName, buildExt, version);
                break;

            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                if (string.IsNullOrEmpty(fileSubPath)) _path = string.Format("{0}[{3}]{4}/{1}{2}", filePath, fileName, buildExt, version, (storeVersion ? "[Store]" : ""));
                else _path = string.Format("{0}[{3}]{5}/{4}/{1}{2}", filePath, fileName, buildExt, version, fileSubPath, (storeVersion ? "[Store]" : ""));
                break;

            default:
                break;
        }
        Debug.Log(_path);
        if (string.IsNullOrEmpty(_path))
        {
            Debug.LogError("Path Error");
            return;
        }
        // return;
        BuildOptions _buildOptions = new BuildOptions();
        _buildOptions = run ? BuildOptions.AutoRunPlayer : BuildOptions.ShowBuiltPlayer;
        BuildPipeline.BuildPlayer(_scenes.ToArray(), _path, targetPlatform, _buildOptions);
    }

    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        {
            string dataPath = pathToBuiltProject.Remove(pathToBuiltProject.Length - 4, 4) + "_data";
            Debug.LogFormat("dataPath: {0}", dataPath);
            string[] files = Directory.GetFiles(Path.Combine(dataPath, "Plugins"), "*.dll");
            for (int i = 0; i < files.Length; i++)
            {
                string targetPath = Path.Combine(dataPath, "Mono");
                targetPath = Path.Combine(targetPath, Path.GetFileName(files[i]));
                Debug.LogFormat("Moving [{0}] to [{1}]", files[i], targetPath);
                File.Move(files[i], targetPath);
            }
        }
    }
}

[CanEditMultipleObjects]
[CustomEditor(typeof(BuildPreset))]
public class BuildPresetEditor : Editor
{
    BuildPreset script;
    void OnEnable()
    {
        script = target as BuildPreset;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //script.scene = EditorGUILayout.ObjectField(script.scene, typeof(Scene));

        //if (!string.IsNullOrEmpty(script.serverConfig.m_ip))
        //{
        //    bool isUsing = ServerConfig.IsUsing(script.serverConfig);
        //    var oldEnabled = GUI.enabled;
        //    var oldColor = GUI.backgroundColor;
        //    if (isUsing) GUI.enabled = false;
        //    else GUI.backgroundColor = Color.red;
        //    if (GUILayout.Button("Apply Server Config"))
        //    {
        //        ServerConfigWindow.Apply(script.serverConfig);
        //    }
        //    if (isUsing) GUI.enabled = oldEnabled;
        //    else GUI.backgroundColor = oldColor;
        //}

        if (GUILayout.Button("Apply"))
        {
            script.Apply();
        }
        if (GUILayout.Button("Build"))
        {
            script.Build(false);
        }
        //if (GUILayout.Button("Build And Run"))
        //{
        //    script.Build(true);
        //}
        if (GUILayout.Button("Open in Explorer"))
        {
            string p = string.Format("{0}[{1}]{2}", script.filePath, script.version, (script.storeVersion ? "[Store]" : ""));
            Application.OpenURL(p);
            Debug.Log(p);
        }
    }
}
#endif

//inputfield color change add this below line in android manifest application tag
// android:theme="@style/UnityThemeSelector"