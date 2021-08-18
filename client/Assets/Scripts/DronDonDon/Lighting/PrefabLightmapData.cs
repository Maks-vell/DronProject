using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// ReSharper disable IdentifierTypo
namespace DronDonDon.PrefabLightmap
{
    [ExecuteAlways]
    [ExecuteInEditMode]
    public class PrefabLightmapData : MonoBehaviour
    {
        [SerializeField]
        private RendererInfo[] _rendererInfo;
        [SerializeField]
        private Texture2D[] _lightmaps;
        [SerializeField]
        private Texture2D[] _lightmapsDir;
        [SerializeField]
        private Texture2D[] _shadowMasks;
        [SerializeField]
        private LightInfo[] _lightInfo;
        
        private void Awake()
        {
            PrepareApplyRendererInfo();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            PrepareApplyRendererInfo();
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void PrepareApplyRendererInfo()
        {
            if (_rendererInfo == null || _rendererInfo.Length == 0) {
                return;
            }

            LightmapData[] lightmaps = LightmapSettings.lightmaps;
            int[] offsetsindexes = new int[_lightmaps.Length];
            int counttotal = lightmaps.Length;
            List<LightmapData> combinedLightmaps = new List<LightmapData>();

            for (int i = 0; i < _lightmaps.Length; i++) {
                bool exists = false;
                for (int j = 0; j < lightmaps.Length; j++) {
                    if (_lightmaps[i] != lightmaps[j].lightmapColor) {
                        continue;
                    }
                    exists = true;
                    offsetsindexes[i] = j;

                }
                if (exists) {
                    continue;
                }
                offsetsindexes[i] = counttotal;
                LightmapData newlightmapdata = new LightmapData {
                        lightmapColor = _lightmaps[i],
                        lightmapDir = _lightmapsDir.Length == _lightmaps.Length ? _lightmapsDir[i] : default(Texture2D),
                        shadowMask = _shadowMasks.Length == _lightmaps.Length ? _shadowMasks[i] : default(Texture2D),
                };

                combinedLightmaps.Add(newlightmapdata);

                counttotal += 1;

            }

            LightmapData[] combinedLightmaps2 = new LightmapData[counttotal];

            lightmaps.CopyTo(combinedLightmaps2, 0);
            combinedLightmaps.ToArray().CopyTo(combinedLightmaps2, lightmaps.Length);

            bool directional = true;

            foreach (Texture2D t in _lightmapsDir) {
                if (t != null) {
                    continue;
                }
                directional = false;
                break;
            }

            LightmapSettings.lightmapsMode = (_lightmapsDir.Length == _lightmaps.Length && directional)
                                                     ? LightmapsMode.CombinedDirectional
                                                     : LightmapsMode.NonDirectional;
            ApplyRendererInfo(_rendererInfo, offsetsindexes, _lightInfo);
            LightmapSettings.lightmaps = combinedLightmaps2;
        }

        private static void ApplyRendererInfo(RendererInfo[] infos, int[] lightmapOffsetIndex, LightInfo[] lightsInfo)
        {
            for (int i = 0; i < infos.Length; i++) {
                RendererInfo info = infos[i];

                info.Renderer.lightmapIndex = lightmapOffsetIndex[info.LightmapIndex];
                info.Renderer.lightmapScaleOffset = info.LightmapOffsetScale;
                
                
                Material[] mat = info.Renderer.sharedMaterials;
                for (int j = 0; j < mat.Length; j++) {
                    if (mat[j] != null && Shader.Find(mat[j].shader.name) != null) {
                        mat[j].shader = Shader.Find(mat[j].shader.name);
                    }
                }

            }

            for (int i = 0; i < lightsInfo.Length; i++) {
                LightBakingOutput bakingOutput = new LightBakingOutput();
                bakingOutput.isBaked = true;
                bakingOutput.lightmapBakeType = (LightmapBakeType) lightsInfo[i].LightmapBaketype;
                bakingOutput.mixedLightingMode = (MixedLightingMode) lightsInfo[i].MixedLightingMode;

                lightsInfo[i].Light.bakingOutput = bakingOutput;

            }
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Bake Prefab Lightmaps")]
        public static void GenerateLightmapInfo()
        {
            if (Lightmapping.giWorkflowMode != Lightmapping.GIWorkflowMode.OnDemand) {
                Debug.LogError("ExtractLightmapData requires that you have baked you lightmaps and Auto mode is disabled.");
                return;
            }
            Lightmapping.Bake();
        
            PrefabLightmapData[] prefabs = FindObjectsOfType<PrefabLightmapData>();
        
            foreach (PrefabLightmapData instance in prefabs) {
                GameObject gameObject = instance.gameObject;
                List<RendererInfo> rendererInfos = new List<RendererInfo>();
                List<Texture2D> lightmaps = new List<Texture2D>();
                List<Texture2D> lightmapsDir = new List<Texture2D>();
                List<Texture2D> shadowMasks = new List<Texture2D>();
                List<LightInfo> lightsInfos = new List<LightInfo>();
        
                GenerateLightmapInfo(gameObject, rendererInfos, lightmaps, lightmapsDir, shadowMasks, lightsInfos);
        
                instance._rendererInfo = rendererInfos.ToArray();
                instance._lightmaps = lightmaps.ToArray();
                instance._lightmapsDir = lightmapsDir.ToArray();
                instance._lightInfo = lightsInfos.ToArray();
                instance._shadowMasks = shadowMasks.ToArray();
                SaveInPrefab(instance.gameObject);
            }
        
        }

        protected static void SaveInPrefab(GameObject instance)
        {
#if UNITY_2018_3_OR_NEWER
            GameObject targetPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(instance) as GameObject;
            if (targetPrefab == null) {
                return;
            }
            GameObject root = PrefabUtility.GetOutermostPrefabInstanceRoot(instance); 
            if (root != null) {
                       
                GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(instance);
                string rootPath = AssetDatabase.GetAssetPath(rootPrefab);
                      
                PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, PrefabUnpackMode.OutermostRoot);
                try {
                            
                    PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
                } catch {
                } finally {
                            
                    PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);
                }
            } else {
                PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
            }
#else
                    GameObject targetPrefab = PrefabUtility.GetPrefabParent(instance) as GameObject;
                    if (targetPrefab == null) {
                        return;
                    }
                    //UnityEditor.Prefab
                    PrefabUtility.ReplacePrefab(instance, targetPrefab);
#endif
        }
        

        private static void GenerateLightmapInfo(GameObject root,
                                                 List<RendererInfo> rendererInfos,
                                                 List<Texture2D> lightmaps,
                                                 List<Texture2D> lightmapsDir,
                                                 List<Texture2D> shadowMasks,
                                                 List<LightInfo> lightsInfo)
        {
            MeshRenderer[] renderers = root.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers) {
                if (renderer.lightmapIndex == -1) {
                    continue;
                }
                RendererInfo info = new RendererInfo();
                info.Renderer = renderer;

                if (renderer.lightmapScaleOffset == Vector4.zero) {
                    continue;
                }
                info.LightmapOffsetScale = renderer.lightmapScaleOffset;

                int lightmapIndex = renderer.lightmapIndex;
                Texture2D lightmap = LightmapSettings.lightmaps[lightmapIndex].lightmapColor;
                Texture2D lightmapDir = LightmapSettings.lightmaps[lightmapIndex].lightmapDir;
                Texture2D shadowMask = LightmapSettings.lightmaps[lightmapIndex].shadowMask;
        
                info.LightmapIndex = lightmaps.IndexOf(lightmap);
                if (info.LightmapIndex == -1) {
                    info.LightmapIndex = lightmaps.Count;
                    lightmaps.Add(lightmap);
                    lightmapsDir.Add(lightmapDir);
                    shadowMasks.Add(shadowMask);
                }
        
                rendererInfos.Add(info);
            }
        
            Light[] lights = root.GetComponentsInChildren<Light>(true);
        
            foreach (Light l in lights) {
                LightInfo lightInfo = new LightInfo();
                lightInfo.Light = l;
                lightInfo.LightmapBaketype = (int) l.lightmapBakeType;
#if UNITY_2020_1_OR_NEWER
                lightInfo.MixedLightingMode = (int)Lightmapping.lightingSettings.mixedBakeMode;
#elif UNITY_2018_1_OR_NEWER
                lightInfo.MixedLightingMode = (int) UnityEditor.LightmapEditorSettings.mixedBakeMode;
#else
                    lightInfo.mixedLightingMode = (int)l.bakingOutput.lightmapBakeType;
#endif
                lightsInfo.Add(lightInfo);
            }
        }
#endif
        [Serializable]
        private struct RendererInfo
        {
            public Renderer Renderer;
            public int LightmapIndex;
            public Vector4 LightmapOffsetScale;
        }

        [Serializable]
        private struct LightInfo
        {
            public Light Light;
            public int LightmapBaketype;
            public int MixedLightingMode;
        }
    }
}
