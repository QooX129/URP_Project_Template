using System;
using Cysharp.Threading.Tasks;
using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public enum ParentTransform
    {
        Gameplay,
        Persistent,
    }

    public interface IUISystem<out T, in U> : ISystem where T : IUIBase where U : AbstractUISearchKeys
    {
        UniTask OpenUIAsync(U searchKeys, Action<T> onLoaded, bool isDeferPush = false);
        T OpenUI(U searchKeys, bool isDeferPush = false);
        void ShowUI(U searchKeys);
        void HideUI(U searchKeys);
        void HideAllUI();
        void CloseUI(U searchKeys);
        void CloseAllUI(bool isFullClear);
        void RemoveFromCache(U searchKeys);
        UniTask CreateUIAsync(U searchKeys, Action<T> onCreated);
        T CreateUI(U searchKeys);
        void Back<Tui>() where Tui : IUIBase;
        void Back(IUIBase ui);
        void Back(string uiName);
    }
}
