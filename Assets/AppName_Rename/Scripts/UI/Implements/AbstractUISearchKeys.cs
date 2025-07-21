using System;
using QFramework;

namespace AppName_Rename.UI
{
    public abstract class AbstractUISearchKeys : IPoolType, IPoolable
    {
        public Type UIType;
        public object AssetReference;
        public string GameObjName;
        public IUIData UIData;

        public bool IsRecycled { get; set; }

        public abstract void OnRecycled();
        public abstract void Recycle2Cache();

    }
}
