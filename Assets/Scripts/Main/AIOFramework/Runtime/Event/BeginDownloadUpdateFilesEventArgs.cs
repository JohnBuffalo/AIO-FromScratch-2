using GameFramework;
using GameFramework.Event;

namespace AIOFramework.Runtime
{
    public class BeginDownloadUpdateFilesEventArgs : GameEventArgs
    {
        public override void Clear()
        {
        }

        public static readonly int EventId = typeof(BeginDownloadUpdateFilesEventArgs).GetHashCode();
        public override int Id => EventId;
        
        public static BeginDownloadUpdateFilesEventArgs Create()
        {
            BeginDownloadUpdateFilesEventArgs args = ReferencePool.Acquire<BeginDownloadUpdateFilesEventArgs>();
            return args;
        }
    }
}