using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public abstract class UIContext : MonoBehaviour, IContext, IController
    {
        public Transform Transform => transform;
        public object AssetReference { get; set; }

        public bool IsInitialized { get; protected set; }
        public ContextInfo Info { get; set; }
        public ContextState State { get; set; }

        protected IUIData UIData;
        protected Tween Tween;

        private readonly Dictionary<RectTransform, UIContext> _containerCaches = new();

        public List<GameObject> references;

        public void Init(IUIData uiData = null)
        {
            OnContextInit(uiData);
        }

        public void UpdateAndShow(IUIData uiData = null)
        {
            State = ContextState.Showing;
            gameObject.SetActive(true);
            OnContextShowAsync(uiData);
        }

        public void Show()
        {
            State = ContextState.Showing;
            gameObject.SetActive(true);
            OnContextShowAsync();
        }

        public void Hide()
        {
            State = ContextState.Hide;
            gameObject.SetActive(false);
            OnContextHide();
        }

        void IUIBase.Close(bool destroy)
        {

        }

        protected abstract void OnContextInit(IUIData uiData = null);
        protected abstract UniTaskVoid OnContextShowAsync(IUIData uiData = null);
        protected abstract void OnContextHide();

        protected GameObject GetReference(string objName)
        {
            var go = references.Find(r => r.name == objName);
            if (go)
                return go;

            Debug.LogError($"[{nameof(GetType)}]: Reference {objName} is null!");
            return null;
        }

        protected T GetReference<T>(string objName) where T : Component
        {
            var component = GetReference(objName).GetComponent<T>();
            if (component)
                return component;

            Debug.LogError($"[{nameof(GetType)}]: Component {typeof(T).Name} is null!");
            return null;
        }

        // protected void SetupScrollView<TContainer, TData>(TData[] dataSet, ScrollView scrollView)
        //     where TContainer : UIContext where TData : IUIData
        // {
        //     scrollView.ResetAllDelegates();

        //     scrollView.SetItemCountFunc(() => dataSet.Length);

        //     scrollView.SetUpdateFunc((index, rectTransform) =>
        //     {
        //         if (!_containerCaches.TryGetValue(rectTransform, out var container))
        //         {
        //             container = rectTransform.GetComponent<TContainer>();
        //             _containerCaches[rectTransform] = container;
        //         }

        //         if (!container.IsInitialized)
        //             container.Init(dataSet[index]);

        //         container.UpdateAndShow(dataSet[index]);
        //     });

        //     scrollView.SetItemGetAndRecycleFunc(null, rectTransform =>
        //     {
        //         if (!_containerCaches.TryGetValue(rectTransform, out var container))
        //             throw new NullReferenceException(
        //                 $"Container of type {typeof(TContainer)} not found in the context table");

        //         container.Hide();
        //     });

        //     scrollView.UpdateData(false);
        // }

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture_Rename.Interface;
        }
    }
}
