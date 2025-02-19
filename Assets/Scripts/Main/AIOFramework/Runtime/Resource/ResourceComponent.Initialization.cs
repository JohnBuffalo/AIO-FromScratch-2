using YooAsset;
using Cysharp.Threading.Tasks;

namespace AIOFramework.Runtime
{
    public partial class ResourceComponent
    {
        /// <summary>
        /// 异步初始化资源包
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="hostServerURL"></param>
        /// <param name="fallbackHostServerURL"></param>
        /// <param name="isDefaultPackage"></param>
        /// <returns></returns>
        public async UniTask<bool> InitPackageAsync(string packageName, string hostServerURL,
            string fallbackHostServerURL, bool isDefaultPackage = false)
        {
            m_ResourcePackage = GetAssetsPackage(packageName) ?? CreateAssetsPackage(packageName);
            if (isDefaultPackage) SetDefaultAssetsPackage(m_ResourcePackage);

            InitializationOperation initOperation =
                CreateInitializationOperationHandler(m_ResourcePackage, hostServerURL, fallbackHostServerURL);

            await initOperation.ToUniTask();

            if (initOperation.Status != EOperationStatus.Succeed)
            {
                Log.Error($"{initOperation.Error}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 根据运行模式创建初始化操作数据
        /// </summary>
        /// <returns></returns>
        private InitializationOperation CreateInitializationOperationHandler(ResourcePackage resourcePackage,
            string hostServerURL, string fallbackHostServerURL)
        {
            switch (PlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    // 编辑器下的模拟模式
                    return InitializeYooAssetEditorSimulateMode(resourcePackage);
                }
                case EPlayMode.OfflinePlayMode:
                {
                    // 单机运行模式
                    return InitializeYooAssetOfflinePlayMode(resourcePackage);
                }
                case EPlayMode.HostPlayMode:
                {
                    // 联机运行模式
                    return InitializeYooAssetHostPlayMode(resourcePackage, hostServerURL, fallbackHostServerURL);
                }
                case EPlayMode.WebPlayMode:
                {
                    // WebGL运行模式
                    return InitializeYooAssetWebPlayMode(resourcePackage, hostServerURL, fallbackHostServerURL);
                }
                default:
                {
                    return null;
                }
            }
        }

        private InitializationOperation InitializeYooAssetEditorSimulateMode(ResourcePackage resourcePackage)
        {
            var buildResult = EditorSimulateModeHelper.SimulateBuild(PackageName);
            var packageRoot = buildResult.PackageRootDirectory;
            var createParameters = new EditorSimulateModeParameters();
            createParameters.EditorFileSystemParameters =
                FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            return resourcePackage.InitializeAsync(createParameters);
        }

        private InitializationOperation InitializeYooAssetOfflinePlayMode(ResourcePackage resourcePackage)
        {
            var createParameters = new OfflinePlayModeParameters();
            createParameters.BuildinFileSystemParameters =
                FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            return resourcePackage.InitializeAsync(createParameters);
        }

        private InitializationOperation InitializeYooAssetWebPlayMode(ResourcePackage resourcePackage,
            string hostServerURL, string fallbackHostServerURL)
        {
            var initParameters = new WebPlayModeParameters();
            FileSystemParameters webFileSystem = null;
#if UNITY_WEBGL
#if ENABLE_DOUYIN_MINI_GAME
            // 创建字节小游戏文件系统
            if (hostServerURL.IsNullOrWhiteSpace())
            {
                webFileSystem = ByteGameFileSystemCreater.CreateByteGameFileSystemParameters();
            }
            else
            {
                webFileSystem = ByteGameFileSystemCreater.CreateByteGameFileSystemParameters(hostServerURL);
            }
#elif ENABLE_WECHAT_MINI_GAME
            WeChatWASM.WXBase.PreloadConcurrent(10);
            // 创建微信小游戏文件系统
            if (hostServerURL.IsNullOrWhiteSpace())
            {
                webFileSystem = WechatFileSystemCreater.CreateWechatFileSystemParameters();
            }
            else
            {
                webFileSystem = WechatFileSystemCreater.CreateWechatPathFileSystemParameters(hostServerURL);
            }
#else
            // 创建默认WebGL文件系统
            webFileSystem = FileSystemParameters.CreateDefaultWebFileSystemParameters();
#endif
#else
            webFileSystem = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
#endif
            initParameters.WebServerFileSystemParameters = webFileSystem;
            return resourcePackage.InitializeAsync(initParameters);
        }

        private InitializationOperation InitializeYooAssetHostPlayMode(ResourcePackage resourcePackage,
            string hostServerURL, string fallbackHostServerURL)
        {
            IRemoteServices remoteServices = new RemoteServices(hostServerURL, fallbackHostServerURL);
            var createParameters = new HostPlayModeParameters();
            createParameters.BuildinFileSystemParameters =
                FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
            createParameters.CacheFileSystemParameters =
                FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
            return resourcePackage.InitializeAsync(createParameters);
        }
    }
}