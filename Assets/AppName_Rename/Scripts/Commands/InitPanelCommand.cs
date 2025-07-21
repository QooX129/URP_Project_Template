using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class InitPanelCommand<T> : AbstractCommand where T : IPanel
    {
        private readonly PanelUISearchKeys _panelUISearchKeys;

        public InitPanelCommand(object assetReference, string prefabName = null, IUIData uiData = null,
            ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            _panelUISearchKeys = PanelUISearchKeys.Allocate();

            _panelUISearchKeys.AssetReference = assetReference;
            _panelUISearchKeys.GameObjName = prefabName;
            _panelUISearchKeys.UIData = uiData;
            _panelUISearchKeys.UIType = typeof(T);
            _panelUISearchKeys.ParentTransform = parentTransform;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IPanelSystem>().CreateUI(_panelUISearchKeys);

            _panelUISearchKeys.Recycle2Cache();
        }
    }
}