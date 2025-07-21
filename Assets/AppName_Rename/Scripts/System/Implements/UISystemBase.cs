using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AppName_Rename.UI
{
    public abstract class UISystemBase<T, U> : AbstractSystem, IUISystem<T, U>, IUnRegisterList
        where T : IUIBase where U : AbstractUISearchKeys
    {
        public List<IUnRegister> UnregisterList { get; } = new();

        protected IAssetSystem AssetSystem;
        protected Transform GamePlayManagerRoot;
        protected Transform PersistentControllerRoot;

        protected override void OnInit()
        {
            AssetSystem = this.GetSystem<IAssetSystem>();
        }

        public abstract UniTask OpenUIAsync(U searchKeys, Action<T> onLoaded, bool isDeferPush = false);
        public abstract T OpenUI(U uiKeys, bool isDeferPush = false);
        public abstract void ShowUI(U searchKeys);
        public abstract void HideUI(U searchKeys);
        public abstract void HideAllUI();
        public abstract void CloseUI(U searchKeys);
        public abstract void CloseAllUI(bool isFullClear = false);
        public abstract void RemoveFromCache(U searchKeys);
        public abstract UniTask CreateUIAsync(U searchKeys, Action<T> onCreated);
        public abstract T CreateUI(U searchKeys);

        public virtual void Back<Tui>() where Tui : IUIBase
        {
            var searchKeys = AllocateSearchKeys();
            searchKeys.UIType = typeof(Tui);

            HideUI(searchKeys);
            RecycleSearchKeys(searchKeys);
        }

        public virtual void Back(IUIBase ui)
        {
            if (ui == null)
                return;

            var searchKeys = AllocateSearchKeys();
            searchKeys.AssetReference = ui.AssetReference;
            searchKeys.UIType = ui.GetType();
            searchKeys.GameObjName = ui.Transform.name;

            HideUI(searchKeys);
            RecycleSearchKeys(searchKeys);
        }

        public virtual void Back(string panelName)
        {
            if (string.IsNullOrEmpty(panelName))
                return;

            var searchKeys = AllocateSearchKeys();
            searchKeys.GameObjName = panelName;

            HideUI(searchKeys);
            RecycleSearchKeys(searchKeys);
        }

        protected abstract U AllocateSearchKeys();
        protected abstract void RecycleSearchKeys(U searchKeys);
        protected abstract UniTask DeferPushUntilUIShown(T ui, T currentUI);

        protected async UniTask<GameObject> LoadUIAsync(U searchKeys, Transform parent)
        {
            var prefab = searchKeys.AssetReference switch
            {
                string path => await AssetSystem.InstantiateInstanceAsync(path, parent),
                AssetReferenceGameObject assetReference => await AssetSystem.InstantiateInstanceAsync(assetReference,
                    parent),
                _ => throw new InvalidOperationException($"SearchKeys {searchKeys} has invalid AssetReference!")
            };

            return prefab;
        }

        protected GameObject LoadUI(U searchKeys, Transform parent)
        {
            var prefab = searchKeys.AssetReference switch
            {
                string path => AssetSystem.InstantiateInstance(path, parent),
                AssetReferenceGameObject assetReference => AssetSystem.InstantiateInstance(assetReference, parent),
                _ => throw new InvalidOperationException($"SearchKeys {searchKeys} has invalid AssetReference!")
            };

            return prefab;
        }

        protected void ReleaseUI(U searchKeys, GameObject ui)
        {
            if (ui is null)
                return;

            switch (searchKeys.AssetReference)
            {
                case string path:
                    AssetSystem.ReleaseInstance(path, ui);
                    break;
                case AssetReferenceGameObject assetReference:
                    AssetSystem.ReleaseInstance(assetReference, ui);
                    break;
                default:
                    throw new InvalidOperationException($"SearchKeys {searchKeys} has invalid AssetReference!");
            }
        }
    }
}