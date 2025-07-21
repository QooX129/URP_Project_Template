using System;

namespace AppName_Rename.UI
{
    public abstract class UIConfigBase<TInitData, TShowData> : IUIConfig<TInitData, TShowData>
    {
        public bool ForceLoad { get; protected set; }
        public TInitData InitData { get; protected set; }
        public TShowData ShowData { get; protected set; }

        public Action OnShowing { get; protected set; }
        public Action OnShowed { get; protected set; }
        public Action OnUpdated { get; protected set; }
        public Action OnCancel { get; protected set; }
        public Action<object> OnInited { get; protected set; }

        object IUIConfig.InitData => InitData;
        object IUIConfig.ShowData => ShowData;

        public abstract class BuilderBase<TBuilder> where TBuilder : BuilderBase<TBuilder>
        {
            protected readonly UIConfigBase<TInitData, TShowData> Config;

            protected BuilderBase(UIConfigBase<TInitData, TShowData> config)
            {
                Config = config;
            }

            public TBuilder SetForceLoad(bool forceLoad)
            {
                Config.ForceLoad = forceLoad;
                return (TBuilder)this;
            }

            public TBuilder SetInitData(TInitData initData)
            {
                Config.InitData = initData;
                return (TBuilder)this;
            }

            public TBuilder SetShowData(TShowData showData)
            {
                Config.ShowData = showData;
                return (TBuilder)this;
            }

            public TBuilder SetOnShowing(Action onShowing)
            {
                Config.OnShowing = onShowing;
                return (TBuilder)this;
            }

            public TBuilder SetOnShowed(Action onShowed)
            {
                Config.OnShowed = onShowed;
                return (TBuilder)this;
            }

            public TBuilder SetOnUpdated(Action onUpdated)
            {
                Config.OnUpdated = onUpdated;
                return (TBuilder)this;
            }

            public TBuilder SetOnCancel(Action onCancel)
            {
                Config.OnCancel = onCancel;
                return (TBuilder)this;
            }

            public TBuilder SetOnInited(Action<object> loadedData)
            {
                Config.OnInited = loadedData;
                return (TBuilder)this;
            }

            public UIConfigBase<TInitData, TShowData> Build()
            {
                return Config;
            }
        }
    }
}