using System.Collections.Generic;
using DG.Tweening;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public abstract class UIElement : MonoBehaviour, IController
    {
        public string AssetPath { get; internal set; }
        public bool IsLoading { get; internal set; }
        public bool IsLoaded { get; internal set; }
        public bool IsInited { get; internal set; }
        public bool IsShowed { get; internal set; }
        public bool IsShowing => gameObject.activeSelf;
        public bool NeedToShow { get; internal set; }
        public bool NeedToLoad { get; internal set; }
        public object InitData { get; set; }
        public object ShowData { get; set; }

        protected virtual float FadeDuration => 0.1f;
        protected Tween Tween;

        public List<GameObject> references;

        protected virtual void OnInit(IUIConfig config)
        {
            IsLoading = true;

            //AssetPath ??= GUID.Generate().ToString();
            InitData = config?.InitData;
            config?.OnInited?.Invoke(config.InitData);

            IsInited = true;
            IsLoading = false;
            IsLoaded = true;
        }

        protected virtual void OnShow(IUIConfig config)
        {
            if (IsShowing || !IsLoaded)
                return;

            gameObject.SetActive(true);
            IsShowed = true;

            ShowData = config?.ShowData;
            config?.OnShowed?.Invoke();
        }

        protected virtual void OnUpdate(IUIConfig config)
        {
            ShowData = config?.ShowData;
        }

        protected virtual void OnPause(bool isPaused)
        {

        }

        protected virtual void OnHide()
        {
            if (!IsShowing)
                return;

            gameObject.SetActive(false);
            IsShowed = false;
        }

        protected virtual void ForceHide()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void OnHideDelay(float delay)
        {
            if (delay <= 0)
                OnHide();
            else
                Invoke(nameof(OnHide), delay);
        }

        protected virtual void OnUIDestroy(bool destroyResources)
        {
            Destroy(gameObject);
        }

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture_Rename.Interface;
        }
    }
}