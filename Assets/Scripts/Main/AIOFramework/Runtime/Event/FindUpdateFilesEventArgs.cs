using GameFramework;
using GameFramework.Event;
using UnityEngine;

namespace AIOFramework
{
    public class FindUpdateFilesEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(FindUpdateFilesEventArgs).GetHashCode();

        public int TotalCount { get; private set; }
        public long TotalSizeBytes { get; private set; }
        public override int Id => EventId;

        public static FindUpdateFilesEventArgs Create(int totalCount, long totalSizeBytes)
        {
            var findUpdateFiles = ReferencePool.Acquire<FindUpdateFilesEventArgs>();
            findUpdateFiles.TotalCount = totalCount;
            findUpdateFiles.TotalSizeBytes = totalSizeBytes;
            return findUpdateFiles;
        }
        
        public override void Clear()
        {
            TotalCount = 0;
            TotalSizeBytes = 0;
        }

    }
}