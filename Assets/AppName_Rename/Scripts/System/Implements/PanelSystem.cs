using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using AppName_Rename.Utility;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public class PanelSystem : UISystemBase<IPanel, PanelUISearchKeys>, IPanelSystem
    {
        public EasyEvent<bool> LoadingScreenActiveEvent { get; } = new();
        public PanelUIStack PanelUIStack { get; } = new();

        private PanelUITable _table;
        private IPanel _currentPanel;

        protected override void OnInit()
        {
            base.OnInit();

            _table = new PanelUITable();

            this.RegisterEvent<IOnUIRootSetEvent>(arg =>
                {
                    PersistentControllerRoot = arg.PanelRootTransform;
                })
                .AddToUnregisterList(this);

            // this.RegisterEvent<ILoadSceneEvent>(args =>
            // {
            //     var needFullClear = args.Scene.sceneType is GameSceneSO.SceneType.Level or GameSceneSO.SceneType.Game;
            //     CloseAllUI(needFullClear);
            // }).AddToUnregisterList(this);
        }

        protected override void OnDeinit()
        {
            this.UnRegisterAll();

            _table.Clear();
        }

        public override async UniTask OpenUIAsync(PanelUISearchKeys searchKeys, Action<IPanel> onLoaded,
            bool isDeferPush = false)
        {
            var panel = _table.GetPanelsBySearchKeys(searchKeys).FirstOrDefault();
            if (panel == null)
            {
                await CreateUIAsync(searchKeys, p => { panel = p; });
            }

            panel.UpdateAndShow(searchKeys.UIData);

            while (panel.State != PanelState.Showing)
            {
                await UniTask.Yield();
            }

            if (_currentPanel != panel && isDeferPush)
                await DeferPushUntilUIShown(panel, _currentPanel);

            PanelUIStack.Push(_currentPanel);
            _currentPanel = panel;

            onLoaded?.Invoke(panel);
        }

        public override IPanel OpenUI(PanelUISearchKeys searchKeys, bool isDeferPush = false)
        {
            var panel = _table.GetPanelsBySearchKeys(searchKeys).FirstOrDefault() ?? CreateUI(searchKeys);
            if (_currentPanel != panel)
            {
                panel.UpdateAndShow(searchKeys.UIData);
                if (isDeferPush)
                    DeferPushUntilUIShown(panel, _currentPanel).Forget();
                else
                    PanelUIStack.Push(_currentPanel);
            }
            _currentPanel = panel;

            return panel;
        }

        public override void ShowUI(PanelUISearchKeys searchKeys)
        {
            var panel = _table.GetPanelsBySearchKeys(searchKeys).FirstOrDefault();

            panel?.UpdateAndShow(searchKeys.UIData);
            _currentPanel = panel;
        }

        public override void HideUI(PanelUISearchKeys searchKeys)
        {
            var panel = _table.GetPanelsBySearchKeys(searchKeys).FirstOrDefault();

            panel?.Hide();
            _currentPanel = null;
        }

        public override void HideAllUI()
        {
            foreach (var panel in _table)
            {
                panel.Hide();
            }

            _currentPanel = null;
        }

        public override void CloseUI(PanelUISearchKeys searchKeys)
        {
            var panel = _table.GetPanelsBySearchKeys(searchKeys).FirstOrDefault();
            if (panel == null)
                return;

            if (!(panel as AbstractPanel))
                return;

            _currentPanel = null;
            panel.Close(true);
            _table.Remove(panel);
            panel.Info.Recycle2Cache();
            panel.Info = null;
        }

        public override void CloseAllUI(bool isFullClear = false)
        {
            _currentPanel = null;
            if (isFullClear)
            {
                foreach (var panel in _table.ToList())
                {
                    if (panel.Info.PanelType == typeof(LoadingScreenPanel))
                        continue;

                    _table.Remove(panel);
                    RecyclePanel(panel, true);
                }
                return;
            }

            var searchKeys = PanelUISearchKeys.Allocate();
            searchKeys.UIType = typeof(LoadingScreenPanel);

            var panels = _table.GetPanelsBySearchKeys(searchKeys);
            foreach (var panel in panels)
            {
                if (panel is LoadingScreenPanel)
                    continue;

                RecyclePanel(panel);
            }
        }

        public override void RemoveFromCache(PanelUISearchKeys uiUISearchKeys)
        {
            var panel = _table.GetPanelsBySearchKeys(uiUISearchKeys).FirstOrDefault();
            if (panel == null)
                return;

            _currentPanel = null;
            _table.Remove(panel);
        }

        public override async UniTask CreateUIAsync(PanelUISearchKeys uiSearchKeys, Action<IPanel> onCreated)
        {
            await LoadPanelAsync(uiSearchKeys, p =>
            {
                //p.Transform.gameObject.name = uiUISearchKeys.GameObjName ?? uiUISearchKeys.UIType.Name;
                p.AssetReference = uiSearchKeys.AssetReference;
                p.Info = PanelInfo.Allocate(uiSearchKeys.GameObjName, uiSearchKeys.UIData, uiSearchKeys.UIType,
                    uiSearchKeys.AssetReference);

                _table.Add(p);
                p.Init(uiSearchKeys.UIData);

                onCreated(p);
            });
        }

        public override IPanel CreateUI(PanelUISearchKeys uiUISearchKeys)
        {
            var panel = LoadPanel(uiUISearchKeys);
            panel.Transform.gameObject.name = uiUISearchKeys.GameObjName ?? uiUISearchKeys.UIType.Name;
            panel.AssetReference = uiUISearchKeys.AssetReference;
            panel.Info = PanelInfo.Allocate(uiUISearchKeys.GameObjName, uiUISearchKeys.UIData, uiUISearchKeys.UIType,
                uiUISearchKeys.AssetReference);

            _table.Add(panel);
            panel.Init(uiUISearchKeys.UIData);

            return panel;
        }

        public override void Back<Tui>()
        {
            base.Back<Tui>();

            PanelUIStack.Pop();
        }

        public override void Back(IUIBase ui)
        {
            base.Back(ui);

            PanelUIStack.Pop();
        }

        public override void Back(string panelName)
        {
            base.Back(panelName);

            PanelUIStack.Pop();
        }

        protected override PanelUISearchKeys AllocateSearchKeys()
        {
            return PanelUISearchKeys.Allocate();
        }

        protected override void RecycleSearchKeys(PanelUISearchKeys searchKeys)
        {
            searchKeys.Recycle2Cache();
        }

        protected override async UniTask DeferPushUntilUIShown(IPanel panel, IPanel previousPanel)
        {
            while (panel.State != PanelState.Showing)
            {
                await UniTask.Yield();
            }

            PanelUIStack.Push(previousPanel);
        }

        public T GetPanel<T>() where T : class, IPanel
        {
            var panelKey = PanelUISearchKeys.Allocate();
            panelKey.UIType = typeof(T);

            var panel = _table.GetPanelsBySearchKeys(panelKey).FirstOrDefault();

            panelKey.Recycle2Cache();

            return panel as T;
        }

        public AbstractPanel GetPanel(PanelUISearchKeys uiUISearchKeys)
        {
            return _table.GetPanelsBySearchKeys(uiUISearchKeys).FirstOrDefault() as AbstractPanel;
        }

        public void ToggleLoadingScreen(LoadingScreenType type, bool isOn, bool needOverlay, Action clientCallback)
        {
            this.SendEvent<ILoadingScreenEvent>(new LoadingScreenEvent(type, isOn, needOverlay, () =>
            {
                LoadingScreenActiveEvent.Trigger(false);
                clientCallback?.Invoke();
            }));

            LoadingScreenActiveEvent.Trigger(true);
        }

        public void ToggleLoadingScreen(LoadingScreenType type, bool isOn, bool needOverlay,
            Func<UniTask> clientCallbackAsync)
        {
            this.SendEvent<ILoadingScreenEvent>(new LoadingScreenEvent(type, isOn, needOverlay, async () =>
            {
                LoadingScreenActiveEvent.Trigger(false);
                await clientCallbackAsync();
            }));

            LoadingScreenActiveEvent.Trigger(true);
        }

#if UNITY_EDITOR
        public void InitTableForTest(PanelUISearchKeys uiSearchKeys)
        {
            uiSearchKeys.Panel.Info = PanelInfo.Allocate(uiSearchKeys.GameObjName, uiSearchKeys.UIData,
                uiSearchKeys.UIType,
                uiSearchKeys.AssetReference);

            _table.Add(uiSearchKeys.Panel);

            uiSearchKeys.Panel.Init(uiSearchKeys.UIData);
            uiSearchKeys.Panel.UpdateAndShow(uiSearchKeys.UIData);
            _currentPanel = uiSearchKeys.Panel;
        }
#endif

        private async UniTask<IPanel> LoadPanelAsync(PanelUISearchKeys uiSearchKeys, Action<IPanel> onPanelLoaded)
        {
            var rootTrans = uiSearchKeys.ParentTransform == ParentTransform.Gameplay
                ? GamePlayManagerRoot
                : PersistentControllerRoot;

            var instance = await LoadUIAsync(uiSearchKeys, rootTrans);
            instance.SetActive(false);

            var panel = instance.GetComponent<IPanel>();
            if (panel != null)
            {
                onPanelLoaded?.Invoke(panel);
                return panel;
            }

            Debug.LogError($"Failed to get panel component in {uiSearchKeys.Panel.Info.GameObjName}");
            ReleaseUI(uiSearchKeys, instance);
            return default;
        }

        private IPanel LoadPanel(PanelUISearchKeys uiSearchKeys)
        {
            var rootTrans = uiSearchKeys.ParentTransform == ParentTransform.Gameplay
                ? GamePlayManagerRoot
                : PersistentControllerRoot;

            var instance = LoadUI(uiSearchKeys, rootTrans);

            var panel = instance.GetComponent<IPanel>();
            if (panel != null)
            {
                instance.SetActive(false);
                return panel;
            }

            Debug.LogError($"Failed to get panel component in {uiSearchKeys.Panel.Info.GameObjName}");
            ReleaseUI(uiSearchKeys, instance);
            return default;
        }

        private static void RecyclePanel(IPanel panel, bool isDestroy = false)
        {
            panel.Close(isDestroy);
            panel.Info.Recycle2Cache();
            panel.Info = null;
        }
    }

    public static class LoadingScreenExtensions
    {
        public static void ExecuteWithLoadingScreen(this IBelongToArchitecture sender, LoadingScreenType type,
            Action onComplete, bool needOverlay = false)
        {
            sender.GetArchitecture().SendCommand(new ToggleLoadingScreenCommand(type, true, () =>
            {
                onComplete?.Invoke();
                sender.GetArchitecture().SendCommand(new ToggleLoadingScreenCommand(type, false, null, needOverlay));
            }, needOverlay));
        }

        public static void ExecuteWithLoadingScreen(this IBelongToArchitecture sender, LoadingScreenType type,
            Func<UniTask> onComplete, bool needOverlay = false)
        {
            sender.GetArchitecture().SendCommand(new ToggleLoadingScreenCommand(type, true, async () =>
            {
                await onComplete();
                sender.GetArchitecture().SendCommand(new ToggleLoadingScreenCommand(type, false, null, needOverlay));
            }, needOverlay));
        }
    }
}