using UnityEngine;
using UnityEditor;

namespace Kino
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Fog))]
    public class FogEditor : Editor
    {
        SerializedProperty _startDistance;
        SerializedProperty _useRadialDistance;
        SerializedProperty _fadeToSkybox;

        void OnEnable()
        {
            _startDistance = serializedObject.FindProperty("_startDistance");
            _useRadialDistance = serializedObject.FindProperty("_useRadialDistance");
            _fadeToSkybox = serializedObject.FindProperty("_fadeToSkybox");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_startDistance);
            EditorGUILayout.PropertyField(_useRadialDistance);
            EditorGUILayout.PropertyField(_fadeToSkybox);

            serializedObject.ApplyModifiedProperties();
        }
    }
}