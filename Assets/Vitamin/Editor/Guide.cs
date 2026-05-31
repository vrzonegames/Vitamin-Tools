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

        GUILayout.Label("Make A SceneMod ScriptableObject Fill In The Name And Scene Path Than Open The Create Mods Window Fill In The ScriptableObject, And AddressableAssetGroup Than Hit Build", GUILayout.Width(position.width));


    }
    
    

    
    
    
}
