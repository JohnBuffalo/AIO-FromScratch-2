using GameFramework;
using YooAsset;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace AIOFramework.Runtime
{
    public partial class ResourceComponent : GameFrameworkComponent, IAssetManager
    {
        private ResourcePackage m_ResourcePackage;

        private ResourcePackage ResourcePackage
        {
            get
            {
                if (m_ResourcePackage == null)
                {
                    m_ResourcePackage = YooAssets.GetPackage(PackageName);
                }

                return m_ResourcePackage;
            }
        }

        public void Initialize()
        {
            Log.Info($"Initialize PlayMode:{PlayMode} , PackageName:{PackageName}");
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(TimeSlice);
        }

        /// <summary>
        /// 创建资源包
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        public ResourcePackage CreateAssetsPackage(string packageName)
        {
            return YooAssets.CreatePackage(packageName);
        }

        /// <summary>
        /// 设置默认资源包
        /// </summary>
        /// <param name="resourcePackage">资源信息</param>
        /// <returns></returns>
        public void SetDefaultAssetsPackage(ResourcePackage resourcePackage)
        {
            YooAssets.SetDefaultPackage(resourcePackage);
        }

        /// <summary>
        /// 获取资源包
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <returns></returns>
        public ResourcePackage GetAssetsPackage(string packageName)
        {
            return YooAssets.TryGetPackage(packageName);
        }
        
        /// <summary>
        /// 是否需要下载
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <returns></returns>
        public bool IsNeedDownload(AssetInfo assetInfo)
        {
            return YooAssets.IsNeedDownloadFromRemote(assetInfo);
        }

        /// <summary>
        /// 是否需要下载
        /// </summary>
        /// <param name="path">资源地址</param>
        /// <returns></returns>
        public bool IsNeedDownload(string path)
        {
            return YooAssets.IsNeedDownloadFromRemote(path);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="assetTags">资源标签列表</param>
        /// <returns></returns>
        public AssetInfo[] GetAssetInfos(string[] assetTags)
        {
            return YooAssets.GetAssetInfos(assetTags);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        /// <param name="assetTag">资源标签</param>
        /// <returns></returns>
        public AssetInfo[] GetAssetInfos(string assetTag)
        {
            return YooAssets.GetAssetInfos(assetTag);
        }

        /// <summary>
        /// 获取资源信息
        /// </summary>
        public AssetInfo GetAssetInfo(string path)
        {
            return YooAssets.GetAssetInfo(path);
        }

        /// <summary>
        /// 检查指定的资源路径是否有效。
        /// </summary>
        /// <param name="path">要检查的资源路径。</param>
        /// <returns>如果资源路径有效，则返回 true；否则返回 false。</returns>
        public bool HasAssetPath(string path)
        {
            return YooAssets.CheckLocationValid(path);
        }

        #region 资源卸载

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        public void UnloadAsset(string assetPath)
        {
            ResourcePackage.TryUnloadUnusedAsset(assetPath);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        /// <param name="assetPath">资源路径</param>
        public void UnloadAsset(string packageName, string assetPath)
        {
            var package = YooAssets.GetPackage(packageName);
            package.TryUnloadUnusedAsset(assetPath);
        }

        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        public void UnloadAllAssetsAsync(string packageName)
        {
            var package = YooAssets.GetPackage(packageName);
            package.UnloadAllAssetsAsync();
        }

        /// <summary>
        /// 强制回收所有资源
        /// </summary>
        public void UnloadAllAssetsAsync()
        {
            ResourcePackage.UnloadAllAssetsAsync();
        }

        /// <summary>
        /// 卸载无用资源, 尽量不用
        /// </summary>
        /// <param name="packageName">资源包名称</param>
        public void UnloadUnusedAssetsAsync(string packageName)
        {
            var package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssetsAsync();
        }

        /// <summary>
        /// 卸载无用资源, 尽量不用
        /// </summary>
        public void UnloadUnusedAssetsAsync()
        {
            ResourcePackage.UnloadUnusedAssetsAsync();
        }

        /// <summary>
        /// 销毁实例对象
        /// </summary>
        /// <param name="obj"></param>
        public void DestroyInstance(UnityEngine.Object obj)
        {
            Object.Destroy(obj);
        }

        #endregion

        #region 异步加载资源

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="assetInfo">资源信息</param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(AssetInfo assetInfo, uint priority = 0) where T : UnityEngine.Object
        {
            AssetHandle handle = ResourcePackage.LoadAssetAsync<T>(assetInfo.AssetPath, priority);
            await handle.ToUniTask();

            if (handle.Status == EOperationStatus.Succeed)
            {
                var asset = handle.AssetObject as T;
                handle.Release();
                return asset;
            }
            else
            {
                Log.Error("Failed to LoadAssetAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public async UniTask<T> LoadAssetAsync<T>(string path, uint priority = 0) where T : UnityEngine.Object
        {
            AssetHandle handle = ResourcePackage.LoadAssetAsync<T>(path, priority);
            await handle.ToUniTask();

            if (handle.Status == EOperationStatus.Succeed)
            {
                var asset = handle.AssetObject as T;
                handle.Release();
                return asset;
            }
            else
            {
                Log.Error("Failed to LoadAssetAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载bundle中的全部资源
        /// </summary>
        /// <param name="path"></param>
        /// <param name="priority"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<IReadOnlyList<T>> LoadAllAssetsAsync<T>(string path, uint priority = 0)
            where T : UnityEngine.Object
        {
            AllAssetsHandle handle = ResourcePackage.LoadAllAssetsAsync<T>(path, priority);
            await handle.ToUniTask();

            if (handle.Status == EOperationStatus.Succeed)
            {
                var assets = handle.AllAssetObjects as IReadOnlyList<T>;
                handle.Release();
                return assets;
            }
            else
            {
                Log.Error("Failed to LoadAllAssetsAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载bundle中的全部资源
        /// </summary>
        /// <param name="assetInfo"></param>
        /// <param name="priority"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async UniTask<IReadOnlyList<T>> LoadAllAssetsAsync<T>(AssetInfo assetInfo, uint priority = 0)
            where T : UnityEngine.Object
        {
            AllAssetsHandle handle = ResourcePackage.LoadAllAssetsAsync<T>(assetInfo.AssetPath, priority);
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed)
            {
                var asset = handle.AllAssetObjects as IReadOnlyList<T>;
                handle.Release();
                return asset;
            }
            else
            {
                Log.Error("Failed to LoadAllAssetsAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载子资源对象
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="assetInfo">资源信息</param>
        /// <param name="priority">加载优先级，默认为0</param>
        /// <returns>加载成功返回子资源对象列表，失败返回null</returns>
        public async UniTask<IReadOnlyList<T>> LoadSubAssetsAsync<T>(AssetInfo assetInfo, uint priority = 0)
            where T : UnityEngine.Object
        {
            // 如果m_Handles字典中不存在指定资源路径的句柄，则调用m_ResourcePackage的LoadSubAssetsAsync方法加载子资源
            SubAssetsHandle handle = ResourcePackage.LoadSubAssetsAsync<T>(assetInfo.AssetPath, priority);
            // 等待加载完成
            await handle.ToUniTask();
            // 检查加载状态
            if (handle.Status == EOperationStatus.Succeed)
            {
                var asset = handle.SubAssetObjects as IReadOnlyList<T>;
                handle.Release();
                return asset;
            }
            else
            {
                // 如果加载失败，记录错误日志并返回null
                Log.Error("Failed to LoadSubAssetsAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载原生文件数据
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <param name="priority">加载优先级，默认为0</param>
        /// <returns>加载成功返回原始文件数据的字节数组，失败返回null</returns>
        public async UniTask<byte[]> LoadRawFileAsync(string path, uint priority = 0)
        {
            RawFileHandle handle = ResourcePackage.LoadRawFileAsync(path, priority);
            await handle.ToUniTask();
            if (handle.Status == EOperationStatus.Succeed)
            {
                var asset = handle.GetRawFileData();
                handle.Release();
                return asset;
            }
            else
            {
                Log.Error("Failed to LoadRawFileAsync: " + handle.LastError);
                return null;
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="path">场景路径</param>
        /// <param name="mode">加载场景的模式，默认为Single</param>
        /// <param name="activateOnLoad">加载完成后是否激活场景，默认为true</param>
        /// <param name="priority">加载优先级，默认为0</param>
        /// <returns>加载成功返回加载的场景，失败返回null</returns>
        public async UniTask<Scene> LoadSceneAsync(string path, LoadSceneMode mode = LoadSceneMode.Single,
            bool activateOnLoad = true, uint priority = 0)
        {
            SceneHandle handle =
                ResourcePackage.LoadSceneAsync(path, mode, LocalPhysicsMode.None, activateOnLoad == false, priority);
            await handle.ToUniTask();

            if (handle.Status == EOperationStatus.Succeed)
            {
                var scene = handle.SceneObject;
                handle.Release();
                return scene;
            }
            else
            {
                Log.Error("Failed to LoadSceneAsync: " + handle.LastError);
                return default;
            }
        }


        /// <summary>
        /// 异步实例化一个Unity对象
        /// </summary>
        /// <typeparam name="T">要实例化的对象类型，必须是UnityEngine.Object的子类</typeparam>
        /// <param name="location">资源的路径或名称</param>
        /// <param name="parent">父级Transform，如果为null，则实例化的对象将没有父级</param>
        /// <param name="position">实例化对象的初始位置，如果未指定，则使用默认值</param>
        /// <param name="rotation">实例化对象的初始旋转，如果未指定，则使用默认值</param>
        /// <returns>实例化后的对象</returns>
        public async UniTask<T> InstantiateAsync<T>(string location, Transform parent = null,
            Vector3 position = default, Quaternion rotation = default) where T : UnityEngine.Object
        {
            T prefab = await LoadAssetAsync<T>(location);
            T instance = Object.Instantiate(prefab, position, rotation, parent);

            return instance;
        }
        

        #endregion
    }
}