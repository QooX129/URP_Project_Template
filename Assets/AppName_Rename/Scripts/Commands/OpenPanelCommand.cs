using System;
using Cysharp.Threading.Tasks;
using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class OpenPanelCommand<T> : AbstractCommand<T> where T : class, IPanel
    {
        private readonly PanelUISearchKeys _panelUISearchKeys;
        private readonly bool _isDeferPush;

        public OpenPanelCommand(object assetReference = null, string prefabName = null, IUIData uiData = null,
            bool isDeferPush = false,
            ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            _panelUISearchKeys = PanelUISearchKeys.Allocate();

            _panelUISearchKeys.AssetReference = assetReference;
            _panelUISearchKeys.UIType = typeof(T);
            _panelUISearchKeys.GameObjName = prefabName;
            _panelUISearchKeys.UIData = uiData;
            _panelUISearchKeys.ParentTransform = parentTransform;

            _isDeferPush = isDeferPush;
        }

        protected override T OnExecute()
        {
            var panel = this.GetSystem<IPanelSystem>().OpenUI(_panelUISearchKeys, _isDeferPush) as T;

            _panelUISearchKeys.Recycle2Cache();

            return panel;
        }
    }

    public class OpenPanelAsyncCommand<T> : AbstractCommand where T : IPanel
    {
        private readonly PanelUISearchKeys _panelUISearchKeys;
        private readonly Action<IPanel> _onCompleted;
        private readonly bool _isDeferPush;

        public OpenPanelAsyncCommand(Action<IPanel> onCompleted = null, object assetReference = null,
            string prefabName = null, IUIData uiData = null, bool isDeferPush = false,
            ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            _panelUISearchKeys = PanelUISearchKeys.Allocate();

            _panelUISearchKeys.AssetReference = assetReference;
            _panelUISearchKeys.UIType = typeof(T);
            _panelUISearchKeys.GameObjName = prefabName;
            _panelUISearchKeys.UIData = uiData;
            _panelUISearchKeys.ParentTransform = parentTransform;

            _onCompleted = onCompleted;
            _isDeferPush = isDeferPush;
        }

        protected override void OnExecute()
        {
            var task = UniTask.Create(async () =>
            {
                await this.GetSystem<IPanelSystem>().OpenUIAsync(_panelUISearchKeys, p =>
                {
                    _onCompleted?.Invoke(p);
                    _panelUISearchKeys.Recycle2Cache();
                }, _isDeferPush);
            });

            task.Forget();
        }
    }
}