using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class ClosePanelCommand : AbstractCommand
    {
        private readonly IPanel _panel;

        public ClosePanelCommand(IPanel panel)
        {
            _panel = panel;
        }

        protected override void OnExecute()
        {
            var searchKeys = PanelUISearchKeys.Allocate();
            searchKeys.UIType = _panel.GetType();

            this.GetSystem<IPanelSystem>().CloseUI(searchKeys);

            searchKeys.Recycle2Cache();
        }
    }

    public class HidePanelCommand<T> : AbstractCommand where T : AbstractPanel
    {
        protected override void OnExecute()
        {
            var searchKeys = PanelUISearchKeys.Allocate();
            searchKeys.UIType = typeof(T);

            this.GetSystem<IPanelSystem>().HideUI(searchKeys);

            searchKeys.Recycle2Cache();
        }
    }
}