using System;
using UnityEngine;

public enum ServerTypeEnum
{
    /// <summary>
    /// 无
    /// </summary>
    None = 0,

    /// <summary>
    /// 内网
    /// </summary>
    Intranet = 1,

    /// <summary>
    /// 外网
    /// </summary>
    Extranet = 2,

    /// <summary>
    /// 正式服
    /// </summary>
    Formal = 3
}

namespace AIOFramework.Setting
{
    [Serializable]
    public class GameSetting
    {
        [SerializeField] private ServerTypeEnum m_ServerType = ServerTypeEnum.Intranet;

        public ServerTypeEnum ServerType => m_ServerType;

        [SerializeField] private string m_Version = "0.0.0";
        public string Version => m_Version;

        [Tooltip("是否在构建资源的时候清理上传到服务端目录的老资源")] [SerializeField]
        private bool m_CleanCommitPathRes = true;

        public bool CleanCommitPathRes => m_CleanCommitPathRes;


        [Tooltip("Dev内网资源地址")] [SerializeField]
        private string m_InnerResourceSourceUrl = "http://127.0.0.1";

        public string InnerResourceSourceUrl => m_InnerResourceSourceUrl;

        [Tooltip("Dev外网资源地址")] [SerializeField]
        private string m_ExtraResourceSourceUrl = "http://127.0.0.1";

        public string ExtraResourceSourceUrl => m_ExtraResourceSourceUrl;


        [Tooltip("Master线上资源地址")] [SerializeField]
        private string m_FormalResourceSourceUrl = "http://127.0.0.1";

        public string FormalResourceSourceUrl => m_FormalResourceSourceUrl;
    }
}