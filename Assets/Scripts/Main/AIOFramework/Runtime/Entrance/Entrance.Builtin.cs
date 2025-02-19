using UnityEngine;

namespace AIOFramework.Runtime
{
    public partial class Entrance : MonoBehaviour
    {
        public static BaseComponent Base { get; private set; }
        public static ResourceComponent Resource { get; private set; }
        public static EventComponent Event { get; private set; }
        public static ProcedureComponent Procedure { get; private set; }

        private void InitBuiltinComponents()
        {
            Base = GameEntry.GetComponent<BaseComponent>();
            Resource = GameEntry.GetComponent<ResourceComponent>();
            Event = GameEntry.GetComponent<EventComponent>();
            Procedure = GameEntry.GetComponent<ProcedureComponent>();
        }
    }
}