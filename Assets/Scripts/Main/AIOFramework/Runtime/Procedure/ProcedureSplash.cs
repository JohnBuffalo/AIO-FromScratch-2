using GameFramework.Procedure;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace AIOFramework.Runtime
{
    /// <summary>
    /// 公司Logo,免责声明的展示等
    /// </summary>
    public class ProcedureSplash : ProcedureBase
    {
        private bool m_SplashFinished = false;

        protected internal override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Enter ProcedureSplash");
            Splash();
        }

        protected internal override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_SplashFinished) return;

            ChangeState<ProcedureInitPackage>(procedureOwner);
        }

        private void Splash()
        {
            m_SplashFinished = true;
        }
    }
}