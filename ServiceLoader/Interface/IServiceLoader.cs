using System;

namespace SCore.Loading
{
    public interface IServiceLoader
    {
        event Action<int, int> OnAsyncStepLoadingEvent;
        event Action OnLoadingCompletedEvent;
        event Action<int, int> OnSyncStepLoadingEvent;
    }
}