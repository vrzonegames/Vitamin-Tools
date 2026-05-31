using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class DownloadMods : MonoBehaviour
{
    

    public GameObject ToDestroy;
    

    // Start is called before the first frame update
    void Start()
    {
        InstallObjects();
    }
    
    
    private AsyncOperationHandle<SceneInstance> loadHandle;


    public void LoadModScene(SceneMod mod)
    {
        Addressables.LoadScene(mod.addressableKey, LoadSceneMode.Additive);
        Destroy(ToDestroy);
    }
    

    async void InstallObjects()
    {
        Debug.Log("Installing Objects");
        if (!Directory.Exists(@"" + Application.persistentDataPath + "Mods"))
        {
            Directory.CreateDirectory(@"" + Application.persistentDataPath + "Mods");
        }

        string[] mods = Directory.GetDirectories(@"" + Application.dataPath + "/Mods");

        foreach (string modPath in mods)
        {
            string modPath2 = modPath.Replace(@"\", "/");
            Debug.Log("Mod Path 2 : " + modPath2);

            LoadModScene(await LoadMaps(modPath2));
        }
    }


    public static async Task<SceneMod> LoadMaps(string modPath)
    {
        string[] files = Directory.GetFiles(@"" + modPath);
        
        

        string platform = "";
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            platform = "StandaloneWindows";
        }
        

        else if (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer)
        {
            platform = "StandaloneLinux";
        }

        string contentCatalogPath = "";
        files = Directory.GetFiles(@"" + modPath + "/" + platform);
        
        Debug.Log("platform : " + Application.platform);
        foreach (string file in files)
        {
            string[] splitFileName = file.Split(".");
            string fileExtension = splitFileName[splitFileName.Length - 1];
            if (fileExtension == "json")
            {
                string finalPath = file;
                finalPath = finalPath.Replace(@"\", "/");
                contentCatalogPath = finalPath;
                Debug.Log("contentCatalogPath : " + contentCatalogPath);
            }
        }

        if (contentCatalogPath == "")
        {
            return null;
        }
        
        StreamReader reader = new StreamReader(contentCatalogPath);
        string contentCatalog = reader.ReadToEnd();
        reader.Close();
        Debug.Log("directory name : " + Path.GetFileName(modPath));
        string modDirectoryName = Path.GetFileName(modPath);
        contentCatalog = contentCatalog.Replace("{LOCAL_FILE_NAME}", modDirectoryName);
        
        StreamWriter writer = new StreamWriter(contentCatalogPath);
        writer.Write(contentCatalog);
        writer.Close();
        
        AsyncOperationHandle<IResourceLocator> loadContentCatalogAsync = Addressables.LoadContentCatalog(contentCatalogPath);
        await loadContentCatalogAsync.Task;
        IResourceLocator resourceLocator = loadContentCatalogAsync.Result;
        resourceLocator.Locate("Map", typeof(SceneInstance), out IList<IResourceLocation> locations);
        if (locations[0] != null)
        {
            SceneMod map = ScriptableObject.CreateInstance<SceneMod>();
            map.addressableKey = locations[0].PrimaryKey;

            return map;
        }

        return null;
    }
}
