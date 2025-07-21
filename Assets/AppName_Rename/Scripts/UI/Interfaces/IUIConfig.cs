using System;

namespace AppName_Rename.UI
{
    public interface IUIData
    {

    }

    public interface IUIConfig
    {
        bool ForceLoad { get; }
        object InitData { get; }
        object ShowData { get; }

        Action OnShowing { get; }
        Action OnShowed { get; }
        Action OnUpdated { get; }
        Action OnCancel { get; }
        Action<object> OnInited { get; }
    }

    public interface IUIConfig<out TInitData, out TShowData> : IUIConfig
    {
        new TInitData InitData { get; }
        new TShowData ShowData { get; }
    }
}
