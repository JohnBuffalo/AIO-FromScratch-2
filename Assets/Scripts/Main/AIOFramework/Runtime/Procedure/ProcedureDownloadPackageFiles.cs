using Cysharp.Threading.Tasks;
using GameFramework.Procedure;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    public class ProcedureDownloadPackageFiles : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Entrance.Event.Fire(this, PatchStateChangeEventArgs.Create("Begin Download Files"));
            BeginDownloadFiles(procedureOwner).Forget();
        }

        private async UniTask BeginDownloadFiles(ProcedureOwner procedureOwner)
        {
            await UniTask.WaitForSeconds(0.5f);
            
            ResourceDownloaderOperation downloader = procedureOwner.GetData<VarDownloader>("Downloader");
            downloader.DownloadErrorCallback += OnDownloadError;
            downloader.DownloadUpdateCallback += OnDownloadProgress;
            downloader.BeginDownload();
            await downloader.ToUniTask();
            
            if (downloader.Status != EOperationStatus.Succeed)
            {
                Log.Error("Download package files failed.");
            }

            ChangeState<ProcedureDownloadPackageFinish>(procedureOwner);
        }
        
        private void OnDownloadError(DownloadErrorData data)
        {
            Log.Error("Download error: Package:{0}, File:{1}, Error:{2}", data.PackageName, data.FileName, data.ErrorInfo);
            Entrance.Event.Fire(this, DownloadFilesFailedEventArgs.Create(data.PackageName,data.FileName, data.ErrorInfo));
        }
        
        private void OnDownloadProgress(DownloadUpdateData data)
        {
            var currentDownloadCount = data.CurrentDownloadCount;
            var totalDownloadCount = data.TotalDownloadCount;
            var currentDownloadBytes = data.CurrentDownloadBytes;
            var totalDownloadBytes = data.TotalDownloadBytes;
            Log.Info("Download progress: {0}/{1}, {2}/{3}", currentDownloadCount, totalDownloadCount,
                currentDownloadBytes, totalDownloadBytes);
            var args = DownloadProgressEventArgs.Create(totalDownloadCount, currentDownloadCount, totalDownloadBytes,
                currentDownloadBytes);
            Entrance.Event.Fire(this, args);
        }
    }
}