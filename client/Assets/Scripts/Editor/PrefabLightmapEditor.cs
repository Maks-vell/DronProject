using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable IdentifierTypo
namespace DronDonDon.PrefabLightmap
{
    [CustomEditor(typeof(BakingPrefabLightmap))]
    public class PrefabLightmapEditor : Editor
    {
        private const string BAKE_BUTTON_NAME = "Bake";
        private const string CHANGE_SCALE_IN_LIGHTMAP_BUTTON_NAME = "Change scale";
        private const string CHANGE_SCALE_IN_LIGHTMAP_LABEL_NAME = "Scale in Lightmap:";
        private const float DEFAULT_SCALE_IN_LIGHTMAP = 0.5f;
        
        private readonly List<string> _serializedPropertiesName = new List<string>() {
                "_rendererInfo",
                "_lightmaps",
                "_lightmapsDir",
                "_shadowMasks",
                "_lightInfo",
        };
        private List<SerializedProperty> _serializedProperties;
        private BakingPrefabLightmap _bakingPrefabLightmap;
        private float _scaleInLightmap;
        private void OnEnable()
        {
            _serializedProperties = new List<SerializedProperty>();
            _serializedPropertiesName.ForEach(s => { _serializedProperties.Add(serializedObject.FindProperty(s)); });
            _scaleInLightmap = DEFAULT_SCALE_IN_LIGHTMAP;
        }
        public override void OnInspectorGUI()
        {
            _bakingPrefabLightmap = ((BakingPrefabLightmap) target);
            _serializedProperties.ForEach(s => { EditorGUILayout.PropertyField(s); });
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button(BAKE_BUTTON_NAME)) {
                _bakingPrefabLightmap.GenerateLightmapInPrefab();
            }
            
            _scaleInLightmap = EditorGUILayout.FloatField(CHANGE_SCALE_IN_LIGHTMAP_LABEL_NAME, _scaleInLightmap);
            
            if (GUILayout.Button(CHANGE_SCALE_IN_LIGHTMAP_BUTTON_NAME)) {
                _bakingPrefabLightmap.ChangeScaleInLightmap(_scaleInLightmap);
            }
        }
    }
}
