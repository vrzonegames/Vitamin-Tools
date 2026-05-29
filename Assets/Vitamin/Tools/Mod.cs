using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Obj Mod", menuName = "Vitamin Tool/Obj Mod")]
public class Mod : ScriptableObject
{
    public string ModName;
    public GameObject Prefab;
}
