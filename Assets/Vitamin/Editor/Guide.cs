using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using Debug = UnityEngine.Debug;


public class Guide : EditorWindow
{
    public Mod mod;

    public string Path;
    
    [MenuItem("Vitamin Tools/Guide")]
    private static void GuideWindow()
    {
        GetWindow<Guide>("Guide");
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

        GUILayout.TextField("Make 3 AssetBundles 'obj', 'scenemod', and 'scene'");


    }
    
    

    
    
    
}
