using System;
using Cysharp.Threading.Tasks;

namespace AppName_Rename
{
    public interface ILoadingScreenEvent
    {
        LoadingScreenType Type { get; }
        bool IsOn { get; }
        bool NeedOverlay { get; }
        Action SystemCallback { get; }
        Func<UniTask> SystemCallbackAsync { get; }
    }

    public record LoadingScreenEvent(LoadingScreenType Type, bool IsOn, bool NeedOverlay, Action SystemCallback)
        : ILoadingScreenEvent
    {
        public Func<UniTask> SystemCallbackAsync { get; }

        public LoadingScreenEvent(LoadingScreenType type, bool isOn, bool needOverlay,
            Func<UniTask> systemCallbackAsync) : this(type, isOn, needOverlay, default(Action))
        {
            SystemCallbackAsync = systemCallbackAsync;
        }
    }
}