using OPMedia.Core.Logging;
using OPMedia.Runtime.ProTONE.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OPMedia.Runtime.ProTONE.OnlineMediaContent
{
    public abstract class OnlineContentSearcher
    {
        public static bool IsSearchConfigValid(OnlineMediaSource source)
        {
            try
            {
                OnlineContentSearcher searcher = GetSearcher(source);
                if (searcher != null)
                    return searcher.HasValidConfig;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return false;
        }

        public static List<IOnlineMediaItem> Search(OnlineMediaSource source, string search, ManualResetEvent abortEvent)
        {
            try
            {
                if (search == null)
                    search = string.Empty;

                OnlineContentSearcher searcher = GetSearcher(source);
                if (searcher != null)
                    return searcher.Search(search.ToLowerInvariant(), abortEvent);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return null;
        }

        private static OnlineContentSearcher GetSearcher(OnlineMediaSource source)
        {
            OnlineContentSearcher searcher = null;

            switch (source)
            {
                case OnlineMediaSource.Deezer:
                    searcher = new DeezerTrackSearcher();
                    break;

                case OnlineMediaSource.ShoutCast:
                    searcher = new ShoutcastDirSearcher();
                    break;

                case OnlineMediaSource.Internal:
                default:
                    searcher = new LocalDatabaseSearcher();
                    break;
            }

            return searcher;
        }

        protected abstract List<IOnlineMediaItem> Search(string search, ManualResetEvent abortEvent);

        protected abstract bool HasValidConfig { get; }
    }
}
