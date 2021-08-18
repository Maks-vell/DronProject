using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;

// ReSharper disable IdentifierTypo
#endif

namespace DronDonDon.PrefabLightmap
{
    [ExecuteAlways]
    [ExecuteInEditMode]
    public class BakingPrefabLightmap : PrefabLightmapData
    {
        private const string BAKING_SCENE_PATH = "Assets/Scenes/BakingScene";
        private const string LIGHTING_DATA_PATH = BAKING_SCENE_PATH + "/LightingData.asset";

        private const string UNITY_META_FILE_EXTENSION = ".meta";
        private const string UNITY_FILE_NAME_EXTENSION = ".unity";
        private const string BAKED_FOLDER_PREFIX = "Baked";
        
        private const string COPY_LIGHTING_SETTINGS_MENU_PATH = "Window/Rendering/Copy Lighting Settings";
        private const string PASTE_LIGHTING_SETTINGS_MENU_PATH = "Window/Rendering/Paste Lighting Settings";
        
        private string _bakingFolderPath;
        private string _currentScenePath;
        
#if UNITY_EDITOR
        public void GenerateLightmapInPrefab()
        {
            ConfigurePath();
            PrepareGenerateLightmapInfo();
        
            _currentScenePath = SceneManager.GetActiveScene().path;

            EditorApplication.ExecuteMenuItem(COPY_LIGHTING_SETTINGS_MENU_PATH);

            Scene bakingScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(bakingScene, BAKING_SCENE_PATH + UNITY_FILE_NAME_EXTENSION);
            PrefabUtility.InstantiatePrefab(gameObject);
            
            EditorApplication.ExecuteMenuItem(PASTE_LIGHTING_SETTINGS_MENU_PATH);
        
            GenerateLightmapInfo();
            AfterBakingLightmap();
        }
        public void ChangeScaleInLightmap(float scale)
        {
            MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer render in renderers) {
                render.scaleInLightmap = scale;
            }
            SaveInPrefab(gameObject);
        }
        private void ConfigurePath()
        {
            string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            string pathToPrefab = prefabPath.Substring(0, prefabPath.LastIndexOf('/') + 1);
            string bakingFolderName = name.Replace("pf", "");
            _bakingFolderPath = pathToPrefab + BAKED_FOLDER_PREFIX + bakingFolderName;

        }
        private void PrepareGenerateLightmapInfo()
        {
            if (File.Exists(BAKING_SCENE_PATH + UNITY_FILE_NAME_EXTENSION)) {
                DeleteBakingScene();
            }
            if (Directory.Exists(_bakingFolderPath)) {
                DeleteFileOrDirectory(_bakingFolderPath);
            }
        }
        
        private void AfterBakingLightmap()
        {
            if (File.Exists(LIGHTING_DATA_PATH)) {
                DeleteFileOrDirectory(LIGHTING_DATA_PATH);
            }
            MoveFileOrDirectory(BAKING_SCENE_PATH, _bakingFolderPath);
            EditorSceneManager.OpenScene(_currentScenePath);
            DeleteBakingScene();
        }
        
        private static void DeleteBakingScene()
        {
            DeleteFileOrDirectory(BAKING_SCENE_PATH + UNITY_FILE_NAME_EXTENSION);
        }
        private static void MoveFileOrDirectory(string source, string dest)
        {
            FileUtil.MoveFileOrDirectory(source, dest);
            FileUtil.MoveFileOrDirectory(source + UNITY_META_FILE_EXTENSION, dest + UNITY_META_FILE_EXTENSION);
            AssetDatabase.Refresh();
        }
        private static void DeleteFileOrDirectory(string path)
        {
            FileUtil.DeleteFileOrDirectory(path);
            FileUtil.DeleteFileOrDirectory(path + UNITY_META_FILE_EXTENSION);
            AssetDatabase.Refresh();
        }
#endif
    }
}

