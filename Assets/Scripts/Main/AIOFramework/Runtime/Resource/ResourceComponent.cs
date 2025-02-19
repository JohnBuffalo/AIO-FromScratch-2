using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using YooAsset;

namespace AIOFramework.Runtime
{
    [DisallowMultipleComponent]
    [AddComponentMenu("AIOFramework/Resource")]
    public partial class ResourceComponent : GameFrameworkComponent
    {
        [SerializeField] 
        private string m_PackageName = "DefaultPackage";
        public string PackageName => m_PackageName;
        
        [SerializeField] 
        private EPlayMode m_PlayMode = EPlayMode.EditorSimulateMode;
        public EPlayMode PlayMode
        {
            get
            {
#if UNITY_EDITOR
                return m_PlayMode;
#elif UNITY_WEBGL
                return EPlayMode.WebPlayMode;
#else
                return EPlayMode.HostPlayMode;
#endif
            }
        }

        [SerializeField]
        [Tooltip("异步操作每帧最大时间切片(毫秒)")]
        private long m_TimeSlice = 1000L;
        public long TimeSlice => m_TimeSlice;
        
        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }
    }
}