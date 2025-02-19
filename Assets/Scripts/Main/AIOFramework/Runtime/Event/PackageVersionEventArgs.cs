using GameFramework;
using GameFramework.Event;

namespace AIOFramework.Runtime
{
    public class PackageVersionEventArgs : GameEventArgs
    {
        public override void Clear()
        {
            PackageVersion = null;
        }
        public static readonly int EventId = typeof(PackageVersionEventArgs).GetHashCode();
        public override int Id => EventId;
        public string PackageVersion { get; private set; }
        public static PackageVersionEventArgs Create(string version)
        {
            var args = ReferencePool.Acquire<PackageVersionEventArgs>();
            args.PackageVersion = version;
            return args;
        }
    }
}