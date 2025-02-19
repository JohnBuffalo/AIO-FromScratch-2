using UnityEngine;

namespace AIOFramework.Setting
{
    [CreateAssetMenu(fileName = "GlobalSettings", menuName = "AIOFramework/GlobalSettings")]
    public class GlobalSettings : ScriptableObject
    {
        [Header("GameSetting")][SerializeField] private GameSetting m_GameSetting;
        public GameSetting GameSetting => m_GameSetting;
    }
}