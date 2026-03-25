using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vitamin;

[AddComponentMenu("Vitamin/Demo/Mod Test")]
public class VModTest : MonoBehaviour
{
    public RawImage image;
    public VMod Mod;
    public string ModName;
    public string ModName2;
    public Material Mat;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestMod()
    {
        Mod.LoadPngToRaw(image, ModName);
        Mod.LoadPngToMat(Mat, ModName2);
        
    }
}
