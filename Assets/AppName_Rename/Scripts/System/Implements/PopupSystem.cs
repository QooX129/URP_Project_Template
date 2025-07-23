using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using AppName_Rename.Utility;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public class PopupSystem : UISystemBase<IPopup, PopupUISearchKeys>, IPopupSystem
    {
        private IPopup _currentPopup;
        private PopupUITable _table;

        protected override void OnInit()
        {
            base.OnInit();
            _table = new PopupUITable();

            this.RegisterEvent<IOnUIRootSetEvent>(arg =>
                {
                        PersistentControllerRoot = arg.PopupRootTransform;
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

        public override async UniTask OpenUIAsync(PopupUISearchKeys searchKeys, Action<IPopup> onLoaded,
            bool isDeferPush = false)
        {
            var popup = _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault();
            if (popup == null)
            {
                await CreateUIAsync(searchKeys, p =>
                {
                    popup = p;
                    popup.UpdateAndShow(searchKeys.UIData);

                    onLoaded?.Invoke(popup);
                });
            }
            else
            {
                popup.UpdateAndShow(searchKeys.UIData);

                onLoaded?.Invoke(popup);
            }
            _currentPopup = popup;
        }

        public override IPopup OpenUI(PopupUISearchKeys searchKeys, bool isDeferPush = false)
        {
            var popup = _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault();
            if (popup == null)
            {
                popup = CreateUI(searchKeys);
                popup.UpdateAndShow(searchKeys.UIData);
            }
            else
            {
                popup.UpdateAndShow(searchKeys.UIData);
            }
            _currentPopup = popup;
            return popup;
        }

        public override void ShowUI(PopupUISearchKeys searchKeys)
        {
            var popup = _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault();

            popup?.Show();
            _currentPopup = popup;
        }

        public override void HideUI(PopupUISearchKeys searchKeys)
        {
            var popup = _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault();

            popup?.Hide();
            _currentPopup = null;
        }

        public override void HideAllUI()
        {
            foreach (var popup in _table)
            {
                popup.Hide();
            }

            _currentPopup = null;
        }

        public override void CloseUI(PopupUISearchKeys searchKeys)
        {
            var popup = _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault();
            if (popup == null)
                return;

            if (!(popup as AbstractPopup))
                return;

            _currentPopup = null;
            popup.Close(true);
            _table.Remove(popup);
            popup.Info.Recycle2Cache();
            popup.Info = null;
        }

        public override void CloseAllUI(bool isFullClear = false)
        {
            _currentPopup = null;
            foreach (var panel in _table)
            {
                RecyclePopup(panel, true);
            }

            _table.Clear();
        }

        public override void RemoveFromCache(PopupUISearchKeys uiSearchKeys)
        {
            var popup = _table.GetPopupsByPopupUIKeys(uiSearchKeys).FirstOrDefault();
            if (popup == null)
                return;

            _currentPopup = null;
            _table.Remove(popup);
        }

        public override async UniTask CreateUIAsync(PopupUISearchKeys searchKeys, Action<IPopup> onCreated)
        {
            await LoadPopupAsync(searchKeys, p =>
            {
                p.Transform.gameObject.name = searchKeys.GameObjName ?? searchKeys.UIType.Name;
                p.AssetReference = searchKeys.AssetReference;
                p.Info = PopupInfo.Allocate(searchKeys.GameObjName, searchKeys.UIData, searchKeys.UIType,
                    searchKeys.AssetReference, searchKeys.ParentTransform);

                _table.Add(p);
                p.Init(searchKeys.UIData);

                onCreated(p);
            });
        }

        public override IPopup CreateUI(PopupUISearchKeys searchKeys)
        {
            var popup = LoadPopup(searchKeys);
            popup.Transform.gameObject.name = searchKeys.GameObjName ?? searchKeys.UIType.Name;
            popup.AssetReference = searchKeys.AssetReference;
            popup.Info = PopupInfo.Allocate(searchKeys.GameObjName, searchKeys.UIData, searchKeys.UIType,
                searchKeys.AssetReference, searchKeys.ParentTransform);

            _table.Add(popup);
            popup.Init(searchKeys.UIData);

            return popup;
        }

        public override void Back<Tui>()
        {
            base.Back<Tui>();

            // Queue

            _currentPopup = null;
        }

        public override void Back(IUIBase ui)
        {
            base.Back(ui);

            // Queue

            _currentPopup = null;
        }

        public override void Back(string panelName)
        {
            base.Back(panelName);

            // Queue

            _currentPopup = null;
        }

        protected override PopupUISearchKeys AllocateSearchKeys()
        {
            return PopupUISearchKeys.Allocate();
        }

        protected override void RecycleSearchKeys(PopupUISearchKeys searchKeys)
        {
            searchKeys.Recycle2Cache();
        }

        protected override async UniTask DeferPushUntilUIShown(IPopup ui, IPopup currentUI)
        {
            await UniTask.Yield();
        }

        public T GetPopup<T>() where T : class, IPopup
        {
            var panelUISearchKeys = PopupUISearchKeys.Allocate();
            panelUISearchKeys.UIType = typeof(T);

            var popup = _table.GetPopupsByPopupUIKeys(panelUISearchKeys).FirstOrDefault();

            panelUISearchKeys.Recycle2Cache();

            return popup as T;
        }

        public AbstractPopup GetPopup(PopupUISearchKeys searchKeys)
        {
            return _table.GetPopupsByPopupUIKeys(searchKeys).FirstOrDefault() as AbstractPopup;
        }

        private async UniTask<IPopup> LoadPopupAsync(PopupUISearchKeys uiSearchKeys, Action<IPopup> onPopupLoaded)
        {
            var rootTrans = uiSearchKeys.ParentTransform == ParentTransform.Gameplay
                ? GamePlayManagerRoot
                : PersistentControllerRoot;

            var instance = await LoadUIAsync(uiSearchKeys, rootTrans);
            instance.SetActive(false);

            var popup = instance.GetComponent<IPopup>();
            if (popup != null)
            {
                onPopupLoaded?.Invoke(popup);
                return popup;
            }

            Debug.LogError($"Failed to get popup component in {uiSearchKeys.Popup.Info.GameObjName}");
            ReleaseUI(uiSearchKeys, instance);
            return default;
        }

        private IPopup LoadPopup(PopupUISearchKeys uiSearchKeys)
        {
            var rootTrans = uiSearchKeys.ParentTransform == ParentTransform.Gameplay
                ? GamePlayManagerRoot
                : PersistentControllerRoot;

            var instance = LoadUI(uiSearchKeys, rootTrans);

            var popup = instance.GetComponent<IPopup>();
            if (popup != null)
            {
                instance.SetActive(false);
                return popup;
            }

            Debug.LogError($"Failed to get popup component in {uiSearchKeys.Popup.Info.GameObjName}");
            ReleaseUI(uiSearchKeys, instance);
            return default;
        }

        private static void RecyclePopup(IPopup popup, bool isDestroy = false)
        {
            popup.Close(isDestroy);
            popup.Info.Recycle2Cache();
            popup.Info = null;
        }
    }
}