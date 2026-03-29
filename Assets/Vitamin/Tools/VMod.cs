using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Vitamin
{
    [AddComponentMenu("Vitamin/Mod Loader")]
    public class VMod : MonoBehaviour
    {
        public Path ModPath;

        public List<ModsPaths> ModPaths;

        public string ModsFilePath;

        public enum Path
        {
            LocalLow,
            Roaming,
            StreamingAssets
        }

        [System.Serializable]
        public class ModsPaths
        {
            public string Name;

            public string Path;

            public string FileName;
        }

        // Start is called before the first frame update
        void Start()
        {
            LoadAllPaths();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadAllPaths()
        {
            if (ModPath == Path.LocalLow)
            {
                LocalLow();
            }
            if (ModPath == Path.Roaming)
            {
                Roaming();
            }
            if (ModPath == Path.StreamingAssets)
            {
                StreamingAssets();
            }

            for (int i = 0; i < ModPaths.Count; i++)
            {
                if (!Directory.Exists(ModsFilePath + "/" + ModPaths[i].Path))
                {
                    Directory.CreateDirectory(ModsFilePath + "/" + ModPaths[i].Path);
                }
            }
        }

        private void LocalLow()
        {
            string P = Application.persistentDataPath + "/Mods";

            Directory.CreateDirectory(P);

            ModsFilePath = P;

            Debug.Log(P);
        }

        private void Roaming()
        {
            string OP = System.IO.Path.Combine(Application.persistentDataPath, "../", "../", "../Roaming") + "/" + Application.companyName + "/Mods";

            string P;

            P = System.IO.Path.GetFullPath( OP );

            Directory.CreateDirectory(P);

            ModsFilePath = P;

            Debug.Log(P);
        }

        private void StreamingAssets()
        {
            string P = Application.streamingAssetsPath + "/Mods";

            Directory.CreateDirectory(P);

            ModsFilePath = P;

            Debug.Log(P);
        }

        public void LoadPngToRaw(RawImage image,string ModName)
        {
            for(int i = 0; i < ModPaths.Count; i++)
            {
                if (ModPaths[i].Name == ModName)
                {
                    string P = ModsFilePath + "/" + ModPaths[i].Path + "/" +  ModPaths[i].FileName;
                    
                    StartCoroutine(DownloadImageToRaw(P, image));
                    
                    Debug.Log(ModsFilePath + "/" + ModPaths[i].Path + "/" + ModPaths[i].FileName);
                }
                
            }
        }
        
        public void LoadPngToMat(Material mat,string ModName)
        {
            for(int i = 0; i < ModPaths.Count; i++)
            {
                if (ModPaths[i].Name == ModName)
                {
                    string P = ModsFilePath + "/" + ModPaths[i].Path + "/" +  ModPaths[i].FileName;
                    
                    StartCoroutine(DownloadImageToMat(P, mat));
                    
                    Debug.Log(ModsFilePath + "/" + ModPaths[i].Path +  "/" + ModPaths[i].FileName);
                }
                
            }
        }

        
        IEnumerator DownloadImageToRaw(string MediaUrl, RawImage I)
        {   
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) 
                Debug.Log(request.error);
            else
                I.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        } 
        
        IEnumerator DownloadImageToMat(string MediaUrl, Material I)
        {   
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) 
                Debug.Log(request.error);
            else
                I.mainTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        }
        
        //AssetBundle Loaders 
        
        IEnumerator DownloadAssetBundle(string BundleUrl, AssetBundle A)
        {   
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(BundleUrl);
            yield return request.SendWebRequest();
            if(request.isNetworkError || request.isHttpError) 
                Debug.Log(request.error);
            else
                A = ((DownloadHandlerAssetBundle) request.downloadHandler).assetBundle;
        }
    }
}