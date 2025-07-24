using System;
using Cysharp.Threading.Tasks;
using AppName_Rename.UI;
using QFramework;
using IPopup = AppName_Rename.UI.IPopup;

namespace AppName_Rename
{
    public class OpenPopupCommand<T> : AbstractCommand where T : class, IPopup
    {
        private readonly PopupUISearchKeys _popupUISearchKeys;

        public OpenPopupCommand(object assetReference = null, string prefabName = null, IUIData uiData = null,
            ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            _popupUISearchKeys = PopupUISearchKeys.Allocate();

            _popupUISearchKeys.AssetReference = assetReference;
            _popupUISearchKeys.UIType = typeof(T);
            _popupUISearchKeys.GameObjName = prefabName;
            _popupUISearchKeys.UIData = uiData;
            _popupUISearchKeys.ParentTransform = parentTransform;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IPopupSystem>().OpenUI(_popupUISearchKeys);
            _popupUISearchKeys.Recycle2Cache();
        }
    }

    public class OpenPopupAsyncCommand<T> : AbstractCommand where T : IPopup
    {
        private readonly PopupUISearchKeys _popupUISearchKeys;
        private readonly Action<IPopup> _onCompleted;

        public OpenPopupAsyncCommand(Action<IPopup> onCompleted = null, object assetReference = null,
            string prefabName = null,
            IUIData uiData = null, ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            _popupUISearchKeys = PopupUISearchKeys.Allocate();

            _popupUISearchKeys.AssetReference = assetReference;
            _popupUISearchKeys.UIType = typeof(T);
            _popupUISearchKeys.GameObjName = prefabName;
            _popupUISearchKeys.UIData = uiData;
            _popupUISearchKeys.ParentTransform = parentTransform;

            _onCompleted = onCompleted;
        }

        protected override void OnExecute()
        {
            UniTask.Create(async () =>
            {
                await this.GetSystem<IPopupSystem>().OpenUIAsync(_popupUISearchKeys, p =>
                {
                    _onCompleted?.Invoke(p);
                    _popupUISearchKeys.Recycle2Cache();
                });
            });
        }
    }
}