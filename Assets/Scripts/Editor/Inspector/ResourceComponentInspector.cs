using System;
using AIOFramework.Runtime;
using UnityEditor;
using GameFramework.Editor;

namespace AIOFramework.Editor
{
    [CustomEditor(typeof(ResourceComponent))]
    public class ResourceComponentInspector : GameFrameworkInspector
    {
        private static readonly string[] ResourceModeNames = new string[] { 
            "EditorSimulateMode-编辑器下的模拟模式", 
            "OfflinePlayMode-离线运行模式", 
            "HostPlayMode-联机运行模式",
            "WebPlayMode-WebGL运行模式" 
        };

        private SerializedProperty m_PlayMode = null;
        private SerializedProperty m_PackageName = null;
        private SerializedProperty m_TimeSlice = null;
        private int m_PlayModeIndex = 0;

        private void OnEnable()
        {
            m_PackageName = serializedObject.FindProperty("m_PackageName");
            m_PlayMode = serializedObject.FindProperty("m_PlayMode");
            m_TimeSlice = serializedObject.FindProperty("m_TimeSlice");
            
            RefreshModes();
            RefreshTypeNames();
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            ResourceComponent t = (ResourceComponent)target;
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_PackageName.stringValue = EditorGUILayout.TextField("Package Name", m_PackageName.stringValue);
                int selectedIndex = EditorGUILayout.Popup("Play Mode", m_PlayModeIndex, ResourceModeNames);
                if (selectedIndex != m_PlayModeIndex)
                {
                    m_PlayModeIndex = selectedIndex;
                    m_PlayMode.enumValueIndex = selectedIndex;
                }
            }
            EditorGUI.EndDisabledGroup();
            
            m_TimeSlice.longValue = (long)EditorGUILayout.Slider("Max TimeSlice",m_TimeSlice.longValue, 30f, 1000f);

            serializedObject.ApplyModifiedProperties();
        }
        
        private void RefreshModes()
        {
            m_PlayModeIndex = m_PlayMode.enumValueIndex;
        }
        private void RefreshTypeNames()
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}