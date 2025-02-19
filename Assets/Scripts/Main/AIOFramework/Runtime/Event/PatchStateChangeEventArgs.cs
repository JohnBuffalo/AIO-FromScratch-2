using GameFramework.Event;
using GameFramework;

namespace AIOFramework.Runtime
{
    public class PatchStateChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PatchStateChangeEventArgs).GetHashCode();
        public override int Id => EventId;
        public string Tips { get; private set; }
        public override void Clear()
        {
            Tips = null;
        }

        public static PatchStateChangeEventArgs Create(string tips)
        {
            var args = ReferencePool.Acquire<PatchStateChangeEventArgs>();
            args.Tips = tips;
            return args;
        }
    }
}