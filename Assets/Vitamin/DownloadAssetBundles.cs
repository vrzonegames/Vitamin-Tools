using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class DownloadAssetBundles : MonoBehaviour
{
    public Transform At;
    
    public List<Mod> Mods;

    public List<GameObject> OBJs;
    
    public List<SceneMod> scenes = null;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAssets());
    }

    public void LoadScene(string sceneName)
    {
        EditorSceneManager.LoadSceneAsyncInPlayMode(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:SceneAsset " + sceneName, new string[] { "Assets" })[0]), new LoadSceneParameters());
    }

    private IEnumerator DownloadAssets()
    {
        string modpath = Application.persistentDataPath + "/Mods";
        
        Directory.CreateDirectory(modpath);
        
        string[] files = Directory.GetDirectories(modpath);

        for (int i = 0; i < files.Length; i++)
        {
            string objFilepath = "file:///" + files[i] + "/obj";
            
            string scenemodFilepath = "file:///" + files[i] + "/scenemod";
            
            string sceneFilepath = "file:///" + files[i] + "/scene";
            
            
            
            
            
            

            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(objFilepath))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogWarning("Error downloading AssetBundle At : " + objFilepath + "  : " + www.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    for (int j = 0; j < bundle.GetAllAssetNames().Length; j++)
                    {
                        Mods.Add(bundle.LoadAsset(bundle.GetAllAssetNames()[i]) as Mod);
                        OBJs.Add(Mods[i].Prefab);
                    }
                    bundle.Unload(false);
                    yield return new WaitForEndOfFrame();
                }
            }
            
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(sceneFilepath))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogWarning("Error downloading AssetBundle At : " + sceneFilepath + "  : " + www.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    for (int j = 0; j < bundle.GetAllAssetNames().Length; j++)
                    {
                        bundle.LoadAsset(bundle.GetAllAssetNames()[i]);
                    }
                    bundle.Unload(false);
                    yield return new WaitForEndOfFrame();
                }
            }
            
            

            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(scenemodFilepath))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogWarning("Error downloading AssetBundle At : " + scenemodFilepath + "  : " + www.error);
                }
                else
                {
                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                    for (int j = 0; j < bundle.GetAllAssetNames().Length; j++)
                    {
                        scenes.Add(bundle.LoadAsset(bundle.GetAllAssetNames()[i]) as SceneMod);

                        //LoadScene(scenes[i].SceneName); //Load Scene On Start
                    }
                    bundle.Unload(false);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }
}
