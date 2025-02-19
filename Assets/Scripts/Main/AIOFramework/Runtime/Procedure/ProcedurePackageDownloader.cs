using Cysharp.Threading.Tasks;
using GameFramework.Event;
using GameFramework.Procedure;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    public class ProcedurePackageDownloader : ProcedureBase
    {
        private ProcedureOwner procedureOwner;
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            AddListeners();
            this.procedureOwner = procedureOwner;
            Entrance.Event.Fire(this, PatchStateChangeEventArgs.Create("CreatePackageDownloader"));
            CreateDownloader(procedureOwner).Forget();
        }
        
        protected internal override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            RemoveListeners();
        }
        
        private void AddListeners()
        {
            Entrance.Event.Subscribe(BeginDownloadUpdateFilesEventArgs.EventId, OnBeginDownloadUpdateFiles);
        }
        
        private void RemoveListeners()
        {
            Entrance.Event.Unsubscribe(BeginDownloadUpdateFilesEventArgs.EventId, OnBeginDownloadUpdateFiles);
        }
        
        private void OnBeginDownloadUpdateFiles(object sender, GameEventArgs e)
        {
            ChangeState<ProcedureDownloadPackageFiles>(procedureOwner);
        }

        private async UniTask CreateDownloader(ProcedureOwner procedureOwner)
        {
            await UniTask.WaitForSeconds(0.5f);

            var packageName = this.procedureOwner.GetData<VarString>("PackageName");
            var package = Entrance.Resource.GetAssetsPackage(packageName);
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            procedureOwner.SetData<VarDownloader>("Downloader", downloader);
            
            if (downloader.TotalDownloadCount == 0)
            {
                Log.Info("No file need download, procedure done");
                ChangeState<ProcedureUpdateDone>(procedureOwner);
            }
            else
            {
                int totalDownloadCount = downloader.TotalDownloadCount;
                long totalDownloadBytes = downloader.TotalDownloadBytes;
                Log.Info($"Need Download File Count {totalDownloadCount}, total Bytes {totalDownloadBytes}");
                Entrance.Event.Fire(this, FindUpdateFilesEventArgs.Create(totalDownloadCount, totalDownloadBytes));
                // CheckDiskSpace(totalDownloadBytes); 
            }
        }
    }
}