using UnityEditor;
using TMPro.EditorUtilities;
using UnityEngine;


namespace Editor {
    [CustomEditor(typeof(ShadowedText))]
    public class ShadowedTextEditor : TMP_EditorPanelUI {
        
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            ShadowedText shadowedText = (ShadowedText) target;

            shadowedText.shadowMaterial = (Material) EditorGUILayout.ObjectField("Shadow Material", shadowedText.shadowMaterial, typeof(Material), true);
            shadowedText.shadowColor = (Color) EditorGUILayout.ColorField("Shadow Color", shadowedText.shadowColor);
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 30.0f;
            EditorGUILayout.LabelField("Shadow Offset");
            EditorGUILayout.Space(-20f);
            EditorGUIUtility.labelWidth = 10.0f;
            shadowedText.shadowOffsetX = EditorGUILayout.FloatField("X", shadowedText.shadowOffsetX);
            shadowedText.shadowOffsetY = EditorGUILayout.FloatField("Y", shadowedText.shadowOffsetY);
            EditorGUILayout.EndHorizontal();
        }
    }
}