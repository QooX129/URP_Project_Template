using System;
using System.Collections.Generic;
// using AillieoUtils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public abstract class AbstractPanel : MonoBehaviour, IPanel, IController
    {
        public Transform Transform => transform;
        public object AssetReference { get; set; }
        public CanvasGroup CanvasGroup { get; protected set; }
        public PanelInfo Info { get; set; }
        public PanelState State { get; set; }

        public List<GameObject> references;

        protected virtual float FadeDuration => 0.2f;
        protected IUIData UIData;
        protected Tween Tween;
        // protected ContextUITable ScrollViewContextTable;

        private Action _onClose;

        private void OnDestroy()
        {
            Tween?.Kill();
            Tween = null;
        }

        public void Init(IUIData uiData = null)
        {
            CanvasGroup = GetComponent<CanvasGroup>();
            if (!CanvasGroup)
                Debug.LogError($"[{nameof(GetType)}]: CanvasGroup is null!");

            UIData = uiData;
            OnPanelInit(uiData);
        }

        public void UpdateAndShow(IUIData uiData = null)
        {
            OnPanelShow(uiData).Forget();
        }

        public void Show()
        {
            //gameObject.SetActive(true);
            OnPanelShow().Forget();
        }

        public void Hide()
        {
            State = PanelState.Hide;
            OnPanelHide().Forget();
            //gameObject.SetActive(false);
        }

        void IUIBase.Close(bool destroy)
        {
            Info.UIData = UIData;
            _onClose?.Invoke();
            _onClose = null;

            Hide();
            State = PanelState.Closed;
            OnPanelClose();

            if (destroy)
                Destroy(gameObject);

            UIData = null;
        }

        // public virtual void JumpTo(PanelPath path, IUIData uiData) { }

        // public virtual void SetScrollViewFunctions<TContainer, TData>(TData[] dataSet, ScrollView scrollView)
        //     where TContainer : UIContext where TData : IUIData
        // {
        //     ScrollViewContextTable ??= new ContextUITable();

        //     scrollView.ResetAllDelegates();

        //     scrollView.SetItemCountFunc(() => dataSet.Length);

        //     scrollView.SetUpdateFunc((index, rectTransform) =>
        //     {
        //         var searchKeys = GetSearchKeys<TContainer>(rectTransform, dataSet[index]);

        //         var container =
        //             ScrollViewContextTable.GetContextsBySearchKeys(searchKeys).FirstOrDefault() as TContainer;

        //         if (!container)
        //         {
        //             container = rectTransform.GetComponent<TContainer>();
        //             container.Info = ContextInfo.Allocate(
        //                 rectTransform.transform.gameObject.name,
        //                 this,
        //                 dataSet[index],
        //                 typeof(TContainer),
        //                 null,
        //                 scrollView.transform,
        //                 rectTransform
        //             );

        //             ScrollViewContextTable.Add(container);
        //         }

        //         if (!container.IsInitialized)
        //             container.Init(dataSet[index]);

        //         container.UpdateAndShow(dataSet[index]);

        //         searchKeys.Recycle2Cache();
        //     });

        //     scrollView.SetItemGetAndRecycleFunc(null, rectTransform =>
        //     {
        //         var searchKeys = GetSearchKeys<TContainer>(rectTransform, null);

        //         var container =
        //             ScrollViewContextTable.GetContextsBySearchKeys(searchKeys).FirstOrDefault() as TContainer;

        //         if (!container)
        //             throw new NullReferenceException(
        //                 $"Container of type {typeof(TContainer)} not found in the context table");

        //         container.Hide();

        //         searchKeys.Recycle2Cache();
        //     });
        // }

        protected virtual void CloseSelf()
        {
            this.SendCommand(new ClosePanelCommand(this));
        }

        protected virtual void BackToPreviousPanel()
        {
            AudioKit.PlaySound(AssetAddress.SfxBack);
            this.SendCommand(new BackToPreviousPanelCommand(this));
        }

        protected virtual async UniTask FadeIn(float duration)
        {
            if (!CanvasGroup)
            {
                Debug.LogWarning("CanvasGroup is null!");
                return;
            }

            CanvasGroup.blocksRaycasts = false;
            Tween = CanvasGroup.DOFade(1f, duration)
                .SetEase(Ease.OutSine)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            await Tween;

            CanvasGroup.blocksRaycasts = true;
        }

        protected virtual async UniTask FadeOut(float duration)
        {
            if (!CanvasGroup)
            {
                Debug.LogWarning("CanvasGroup is null!");
                return;
            }

            CanvasGroup.blocksRaycasts = false;
            Tween = CanvasGroup.DOFade(0f, duration)
                .SetEase(Ease.OutSine)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            await Tween;

            gameObject.SetActive(false);
        }

        protected abstract void OnPanelInit(IUIData uiData = null);

        protected virtual async UniTask OnPanelShow(IUIData uiData = null)
        {
            if (State != PanelState.Showing)
            {
                CanvasGroup.alpha = 0f;
                gameObject.SetActive(true);

                await FadeIn(FadeDuration);
            }

            gameObject.SetActive(true);
            State = PanelState.Showing;
        }

        protected virtual async UniTask OnPanelHide()
        {
            await FadeOut(FadeDuration);
            State = PanelState.Hide;
        }

        protected abstract void OnPanelClose();

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

        protected static ContextUISearchKeys GetSearchKeys<TContainer>(RectTransform rectTransform, IUIData data)
        {
            var searchKeys = ContextUISearchKeys.Allocate();
            searchKeys.UIType = typeof(TContainer);
            searchKeys.UIData = data;
            searchKeys.RectTransform = rectTransform;

            return searchKeys;
        }

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture_Rename.Interface;
        }
    }
}