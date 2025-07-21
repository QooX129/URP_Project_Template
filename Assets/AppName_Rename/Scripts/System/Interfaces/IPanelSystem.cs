using System;
using Cysharp.Threading.Tasks;
using QFramework;

namespace AppName_Rename.UI
{
    public interface IPanelSystem : IUISystem<IPanel, PanelUISearchKeys>
    {
        EasyEvent<bool> LoadingScreenActiveEvent { get; }
        PanelUIStack PanelUIStack { get; }

        T GetPanel<T>() where T : class, IPanel;
        AbstractPanel GetPanel(PanelUISearchKeys uiUISearchKeys);
        void ToggleLoadingScreen(LoadingScreenType type, bool isOn, bool needOverlay, Action clientCallback);
        void ToggleLoadingScreen(LoadingScreenType type, bool isOn, bool needOverlay,
            Func<UniTask> clientCallbackAsync);

#if UNITY_EDITOR
        void InitTableForTest(PanelUISearchKeys uiSearchKeys);
#endif
    }
}