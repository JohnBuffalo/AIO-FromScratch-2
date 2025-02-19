using Cysharp.Threading.Tasks;
using GameFramework.Procedure;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    public class ProcedureUpdatePackageManifest : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Entrance.Event.Fire(this, PatchStateChangeEventArgs.Create("UpdateManifest"));
            UpdateManifest(procedureOwner).Forget();
        }

        private async UniTask UpdateManifest(ProcedureOwner procedureOwner)
        {
            await UniTask.WaitForSeconds(0.5f);
            
            var packageName = procedureOwner.GetData<VarString>("PackageName");
            var packageVersion = procedureOwner.GetData<VarString>("PackageVersion");
            var package = Entrance.Resource.GetAssetsPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            await operation.ToUniTask();

            if (operation.Status != EOperationStatus.Succeed)
            {
                Log.Error($"UpdatePackageManifest for package: {packageName} failed, error message: {operation.Error}");
                Entrance.Event.Fire(this, InitPackageFailedEventArgs.Create());
            }
            else
            {
                Log.Info("UpdatePackageManifest succeed");
                ChangeState<ProcedurePackageDownloader>(procedureOwner);
            }
            
        }
    }
}