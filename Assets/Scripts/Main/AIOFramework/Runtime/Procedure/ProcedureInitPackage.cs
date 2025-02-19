using AIOFramework.Setting;
using GameFramework.Procedure;
using YooAsset;
using Cysharp.Threading.Tasks;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    /// <summary>
    /// 初始化ResourcePackage
    /// </summary>
    public class ProcedureInitPackage : ProcedureBase
    {
        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Enter ProcedureInitPackage");
            Entrance.Event.Fire(this, PatchStateChangeEventArgs.Create("InitPackage"));
            InitPackage(procedureOwner).Forget();
        }

        private async UniTask InitPackage(ProcedureOwner procedureOwner)
        {
            var playMode = Entrance.Resource.PlayMode;
            var packageName = Entrance.Resource.PackageName;

            procedureOwner.SetData<VarInt32>("PlayMode", (int)playMode);
            procedureOwner.SetData<VarString>("PackageName", packageName);

            Log.Info($"InitPackage , playMode : {playMode}, packageName : {packageName}");

            var initSuccess =
                await Entrance.Resource.InitPackageAsync(packageName, GetHostServerURL(), GetDefaultServerURL(), true);

            if (initSuccess)
            {
                ChangeState<ProcedureUpdatePackageVersion>(procedureOwner);
            }
            else
            {
                Entrance.Event.Fire(this, InitPackageFailedEventArgs.Create());
            }
        }

        private string GetHostServerURL()
        {
            var serverType = SettingUtility.GlobalSettings.GameSetting.ServerType;
            string url;
            string platform = SettingUtility.PlatformName();
            switch (serverType)
            {
                case ServerTypeEnum.Intranet:
                    url = SettingUtility.GlobalSettings.GameSetting.InnerResourceSourceUrl;
                    break;
                case ServerTypeEnum.Extranet:
                    url = SettingUtility.GlobalSettings.GameSetting.ExtraResourceSourceUrl;
                    break;
                case ServerTypeEnum.Formal:
                    url = SettingUtility.GlobalSettings.GameSetting.FormalResourceSourceUrl;
                    break;
                default:
                    url = string.Empty;
                    break;
            }

            return $"{url}/{platform}/";
        }

        private string GetDefaultServerURL()
        {
            string platform = SettingUtility.PlatformName();
            string url = "http://127.0.0.1";
            return $"{url}/{platform}/";
        }
    }
}