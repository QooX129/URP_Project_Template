using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class HidePopupCommand : AbstractCommand
    {
        private readonly UI.IPopup _popup;

        public HidePopupCommand(UI.IPopup popup)
        {
            _popup = popup;
            AudioKit.PlaySound(AssetAddress.SfxBack);
        }

        protected override void OnExecute()
        {
            var searchKeys = PopupUISearchKeys.Allocate();
            searchKeys.UIType = _popup.GetType();

            this.GetSystem<IPopupSystem>().HideUI(searchKeys);

            searchKeys.Recycle2Cache();
        }
    }
}