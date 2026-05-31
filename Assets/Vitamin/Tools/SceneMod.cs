using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "New Scene Mod", menuName = "Vitamin Tool/Scene Mod")]
public class SceneMod : ScriptableObject
{
    public string ModName;
    public string SceneName;
    public string addressableKey;
}
