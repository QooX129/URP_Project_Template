using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class ClosePopupCommand : AbstractCommand
    {
        private readonly UI.IPopup _popup;

        public ClosePopupCommand(UI.IPopup popup)
        {
            _popup = popup;
        }

        protected override void OnExecute()
        {
            var searchKeys = PopupUISearchKeys.Allocate();
            searchKeys.UIType = _popup.GetType();

            this.GetSystem<IPopupSystem>().CloseUI(searchKeys);

            searchKeys.Recycle2Cache();
        }
    }
}