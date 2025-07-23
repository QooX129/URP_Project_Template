using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DG.Tweening;
using AppName_Rename.Core;
using I2.Loc;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AppName_Rename.UI
{
    public abstract class AbstractPopup : MonoBehaviour, IPopup, IController
    {
        public CanvasGroup CanvasGroup { get; protected set; }
        public Transform Transform => transform;
        public object AssetReference { get; set; }
        public PopupInfo Info { get; set; }
        public PopupState State { get; set; }

        protected virtual float FadeDuration => 0.1f;

        [SerializeField] protected List<GameObject> references;

        protected IUIData UIData;
        protected Tween Tween;

        protected IAssetSystem AssetSystem;
        protected Localize SubjectLabel;
        protected Button PrimaryButton;
        protected Button SecondaryButton;

        private Action _onClose;

        private void OnDisable()
        {
            Tween?.Kill();
            Tween = null;
        }

        public void Init(IUIData uiData = null)
        {
            AssetSystem = this.GetSystem<IAssetSystem>();

            CanvasGroup = GetComponent<CanvasGroup>();
            if (!CanvasGroup)
                Debug.LogError($"[{nameof(GetType)}]: CanvasGroup is null!");

            UIData = uiData;
            OnPopupInit(uiData);
        }

        public void UpdateAndShow(IUIData uiData = null)
        {
            if (uiData is PopupUIData data)
            {
                SetSubject(data.Subject);
                SetPrimaryButton(data.PrimaryButtonText, data.PrimaryButtonAction);
                SetSecondaryButton(data.SecondaryButtonText, data.SecondaryButtonAction);
            }
            // else
            // {
            //     Debug.LogError(
            //         $"[{GetType()}]: UI Data is not of type {nameof(PopupUIData)}");
            // }

            State = PopupState.Showing;
            gameObject.SetActive(true);
            OnPopupShow(uiData);
        }

        public void Show()
        {
            State = PopupState.Showing;
            OnPopupShow();
        }

        public void Hide()
        {
            if (PrimaryButton)
                PrimaryButton.onClick.RemoveAllListeners();
            if (SecondaryButton)
                SecondaryButton.onClick.RemoveAllListeners();

            State = PopupState.Hide;
            OnPopupHide();
            gameObject.SetActive(false);
        }

        void IUIBase.Close(bool destroy)
        {
            Info.UIData = UIData;
            _onClose?.Invoke();
            _onClose = null;

            Hide();
            State = PopupState.Closed;
            OnPopupClose();

            if (destroy)
                Destroy(gameObject);

            UIData = null;
        }

        protected virtual void CloseSelf()
        {
            this.SendCommand(new ClosePopupCommand(this));
            AudioKit.PlaySound(AssetAddress.SfxBack);
        }

        protected virtual void SetSubject(string subject)
        {
            if (!SubjectLabel)
                return;

            // if (!SubjectLabel)
            //     throw new NullReferenceException($"[{GetType()}]: Subject is not set");

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException($"[{GetType()}]: String is empty: {nameof(subject)}");

            SubjectLabel.SetTerm(subject);
            var text = SubjectLabel.GetComponent<TextMeshProUGUI>();
            text.text = Regex.Unescape(text.text);
        }

        protected virtual void SetPrimaryButton(string buttonText, Action onClick = null)
        {
            if (!PrimaryButton)
                return;
            // throw new NullReferenceException($"[{GetType()}] PrimaryButton is not set");

            PrimaryButton.GetComponentInChildren<Localize>().SetTerm(string.IsNullOrEmpty(buttonText)
                ? "OK"
                : buttonText);

            PrimaryButton.onClick.RemoveAllListeners();
            PrimaryButton.onClick.AddListener(() =>
            {
                onClick?.Invoke();
            });
        }

        protected virtual void SetSecondaryButton(string buttonText, Action onClick = null)
        {
            if (!SecondaryButton)
                return;
            //throw new NullReferenceException($"[{GetType()}] SecondaryButton is not set");

            if (string.IsNullOrEmpty(buttonText))
            {
                SecondaryButton.gameObject.SetActive(false);
                return;
            }

            SecondaryButton.GetComponentInChildren<Localize>().SetTerm(buttonText);
            SecondaryButton.onClick.RemoveAllListeners();
            SecondaryButton.onClick.AddListener(onClick == null ? CloseSelf : new UnityAction(onClick));
        }

        protected abstract void OnPopupInit(IUIData uiData = null);
        protected abstract void OnPopupShow(IUIData uiData = null);
        protected abstract void OnPopupHide();
        protected abstract void OnPopupClose();

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

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture_Rename.Interface;
        }
    }
}