using GameFramework;
using GameFramework.Event;

namespace AIOFramework.Runtime
{
    public class InitPackageFailedEventArgs : GameEventArgs
    {
        public override void Clear()
        {
        }

        public static readonly int EventId = typeof(InitPackageFailedEventArgs).GetHashCode();
        public override int Id => EventId;

        public static InitPackageFailedEventArgs Create()
        {
            var args = ReferencePool.Acquire<InitPackageFailedEventArgs>();
            return args;
        }
    }
}