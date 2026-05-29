using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using Debug = UnityEngine.Debug;


public class CreateAssetBundles : EditorWindow
{
    public Mod mod;

    public string Path;
    
    [MenuItem("Vitamin Tools/Create Asset Bundles")]
    private static void AssetBundleWindow()
    {
        GetWindow<CreateAssetBundles>("Create Assets Bundles");
    }
    
    void OnEnable()
    {

    }
    
    void OnGUI ()
    {
        //Window Code
        GUILayout.Space(20);
        
        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Vitamin/PNG/Vitamin_Tools-Logo.png", typeof(Texture));
        GUILayout.Box(banner, GUILayout.Width(position.width), GUILayout.Height(98));
        
        GUILayout.Space(20);

        GUILayout.Space(20);

        Build();
    }
    
    

    public void Build()
    {
        if (GUILayout.Button("Build Mod"))
        {
            BuildAllAssetBundles();
        }
    }
    
    
    private static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.dataPath + "/AssetBundles";

        if (!Directory.Exists(assetBundleDirectory))
            return;
        FileUtil.DeleteFileOrDirectory(assetBundleDirectory);

        Directory.CreateDirectory(assetBundleDirectory);
        
        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
            Process.Start(assetBundleDirectory);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        
        Debug.Log(assetBundleDirectory);
    }
    
}