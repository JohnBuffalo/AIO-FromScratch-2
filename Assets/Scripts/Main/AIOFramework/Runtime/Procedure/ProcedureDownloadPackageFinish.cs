using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    public class ProcedureDownloadPackageFinish : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            ClearCache(procedureOwner).Forget();
        }

        private async UniTask ClearCache(ProcedureOwner procedureOwner)
        {
            await UniTask.WaitForSeconds(0.5f);

            var packageName = procedureOwner.GetData<VarString>("PackageName");
            var package = Entrance.Resource.GetAssetsPackage(packageName);
            var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
            Log.Info($"ProcedureDownloadPackageFinish ClearCacheFilesAsync : {EFileClearMode.ClearUnusedBundleFiles}");

            await operation.ToUniTask();

            if (operation.Status != EOperationStatus.Succeed)
            {
                Log.Error($"ProcedureDownloadPackageFinish ClearCacheFilesAsync Failed : {operation.Error}");
            }
            else
            {
                ChangeState<ProcedureUpdateDone>(procedureOwner);
            }
            
        }
    }
}


