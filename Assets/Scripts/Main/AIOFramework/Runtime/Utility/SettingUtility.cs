
using System;
using AIOFramework.Runtime;
using UnityEngine;

namespace AIOFramework.Setting
{
    public static class SettingUtility
    {
        private static readonly string GlobalSettingsPath = $"Settings/GlobalSettings";
        private static GlobalSettings m_GlobalSettings;
        
        public static GlobalSettings GlobalSettings
        {
            get
            {
                if (m_GlobalSettings == null)
                {
                    m_GlobalSettings = GetSingletonAssetsByResources<GlobalSettings>(GlobalSettingsPath);
                }
                return m_GlobalSettings;
            }
        }
        
        private static T GetSingletonAssetsByResources<T>(string assetsPath) where T : ScriptableObject, new()
        {
            string assetType = typeof(T).Name;
#if UNITY_EDITOR
            string[] globalAssetPaths = UnityEditor.AssetDatabase.FindAssets($"t:{assetType}");
            if (globalAssetPaths.Length > 1)
            {
                foreach (var assetPath in globalAssetPaths)
                {
                    Log.Error($"Could not had Multiple {assetType}. Repeated Path: {UnityEditor.AssetDatabase.GUIDToAssetPath(assetPath)}");
                }

                throw new Exception($"Could not had Multiple {assetType}");
            }
#endif
            T customGlobalSettings = Resources.Load<T>(assetsPath);
            if (customGlobalSettings == null)
            {
                Log.Error($"Could not found {assetType} asset，so auto create:{assetsPath}.");
                return null;
            }

            return customGlobalSettings;
        }

        public static string PlatformName()
        {
            var platFormName = "Android";
#if UNITY_EDITOR
            switch (UnityEditor.EditorUserBuildSettings.activeBuildTarget)
            {
                case UnityEditor.BuildTarget.Android:
                    platFormName = "Android";
                    break;
                case UnityEditor.BuildTarget.iOS:
                    platFormName = "IOS";
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    platFormName = "PC";
                    break;
                case UnityEditor.BuildTarget.StandaloneOSX:
                    platFormName = "MacOS";
                    break;
                case UnityEditor.BuildTarget.StandaloneWindows:
                    platFormName = "PC";
                    break;
            }
#else
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    platFormName = "Android";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platFormName = "IOS";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    platFormName = "PC";
                    break;
            }
#endif
            return platFormName;
        }
    }
}