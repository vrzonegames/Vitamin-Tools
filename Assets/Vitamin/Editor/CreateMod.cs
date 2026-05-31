#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.SceneManagement;
using System.Linq;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class CreateMod : EditorWindow
{
    string ModName = "";
    string basePath = "";
    string exportPath = "";
    string modPath = "";
    bool showMapSettings = true;
    bool showSceneSettings = true;
    bool showExportSettings = true;
    bool showConfiguredGamemodes = true;
    bool showPlatforms = true;
    bool openAfterExport;

    string GroupName;


    List<string> configuredGamemodes = new List<string>();
    List<string> configuredPlatforms = new List<string> { "Windows", "Android" };
    
    SceneMod profile;
    
    AddressableAssetGroup Settings;

    
    [MenuItem("Vitamin Tools/Create Mods")]
    private static void AssetBundleWindow()
    {
        GetWindow<CreateMod>("Create Mods");
    }
    
    void OnEnable()
    {
        
    }
    
    private void Awake() {
        exportPath = "";
        basePath = FormatPath(UnityEngine.Application.persistentDataPath + "/Export");
        openAfterExport = EditorPrefs.GetBool("OpenAfterExport", false);
    }
    
    void OnGUI ()
    {
        //Window Code
        GUILayout.Space(20);
        
        Texture banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/Vitamin/PNG/Vitamin_Tools-Logo.png", typeof(Texture));
        GUILayout.Box(banner, GUILayout.Width(position.width), GUILayout.Height(98));
        
        GUILayout.Space(20);
        
        profile = EditorGUILayout.ObjectField("Mod Profile", profile, typeof(SceneMod), false) as SceneMod;
        

        if (profile)
        {
            ModName = profile.ModName;
            modPath = profile.ScenePath;
            GUILayout.Label(ModName);
        }
        
        GUILayout.Space(20);
        
        Settings = EditorGUILayout.ObjectField("Settings Profile", Settings, typeof(AddressableAssetGroup), false) as AddressableAssetGroup;

        if (Settings)
        {
            GroupName = Settings.Name;
            GUILayout.Label(GroupName);
        }
            
        GUILayout.Space(20);
        GUILayout.Space(20);

        Build();
    }
    
    

    public void Build()
    {
        if (GUILayout.Button("Build Mod"))
        {
            Export();
        }
    }
    
    
    void Export()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.StandaloneWindows;
        BuildAddressables();
    }
    
    private void DeleteFolder(string path) {
        if (!Directory.Exists(path))
            return;
        FileUtil.DeleteFileOrDirectory(path);
    }

    private void CreateFolder(string path) {
        Directory.CreateDirectory(path);
    }

    private string FormatPath(string path) {
        return path.Replace(" ", "").Replace(@"\", "/");
    }

    private void BuildAddressables(object obj = null)
    {
        if (Directory.Exists(Application.persistentDataPath + "/Export/" + FormatPath(ModName) + "/" + EditorUserBuildSettings.selectedStandaloneTarget))
            DeleteFolder(Application.persistentDataPath + "/Export/" + FormatPath(ModName) + "/" + EditorUserBuildSettings.selectedStandaloneTarget);
        var group = AddressableAssetSettingsDefaultObject.Settings.FindGroup(GroupName);
        var guid = AssetDatabase.AssetPathToGUID(modPath);
        if (group == null || guid == null)
        {
            return;
        }

        foreach (AddressableAssetEntry entry in group.entries.ToList())
        {
            group.RemoveAssetEntry(entry);
        }
        
        var e = AddressableAssetSettingsDefaultObject.Settings.CreateOrMoveEntry(guid, group, false, false);
        var entriesAdded = new List<AddressableAssetEntry> { e };
        e.SetLabel("Map", true, true, false);

        
        
        group.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
        AddressableAssetSettingsDefaultObject.Settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
        
        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId,
            "Local.LoadPath",
            "{UnityEngine.Application.persistentDataPath}/Mods/{LOCAL_FILE_NAME}/" + EditorUserBuildSettings.selectedStandaloneTarget
        );
        
        AddressableAssetSettingsDefaultObject.Settings.profileSettings.SetValue(
            AddressableAssetSettingsDefaultObject.Settings.activeProfileId,
            "Local.BuildPath",
            Application.persistentDataPath + "/Export/" + FormatPath(ModName) + "/" + EditorUserBuildSettings.selectedStandaloneTarget
        );
        AddressableAssetSettings.CleanPlayerContent(AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder);
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
    }
    
    public static void DrawUILine(Color color, int thickness = 2, int padding = 10) {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
    
}

#endif