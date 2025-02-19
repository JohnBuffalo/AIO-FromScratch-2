using Cysharp.Threading.Tasks;
using GameFramework.Procedure;
using YooAsset;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    public class ProcedureUpdatePackageVersion : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Entrance.Event.Fire(this,PatchStateChangeEventArgs.Create("UpdatePackageVersion"));
            UpdatePackageVersion(procedureOwner).Forget();
        }

        private async UniTask UpdatePackageVersion(ProcedureOwner procedureOwner)
        {
            await UniTask.WaitForSeconds(0.5f);
            var packageName = Entrance.Resource.PackageName;
            var package = Entrance.Resource.GetAssetsPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            Log.Info($"UpdatePackageVersion for package: {packageName}");

            await operation.ToUniTask();

            if (operation.Status != EOperationStatus.Succeed)
            {
                Log.Error($"UpdatePackageVersion for package: {packageName} failed, error message: {operation.Error}");
                Entrance.Event.Fire(this,InitPackageFailedEventArgs.Create());
            }
            else
            {
                Log.Info($"Request package version : {operation.PackageVersion}");
                procedureOwner.SetData<VarString>("PackageVersion", operation.PackageVersion);
                Entrance.Event.Fire(this, PackageVersionEventArgs.Create(operation.PackageVersion));
                ChangeState<ProcedureUpdatePackageManifest>(procedureOwner);
            }
        }
    }
}