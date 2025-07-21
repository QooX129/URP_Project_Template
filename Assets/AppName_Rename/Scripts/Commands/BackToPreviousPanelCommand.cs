using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class BackToPreviousPanelCommand : AbstractCommand
    {
        private readonly IPanel _panel;

        public BackToPreviousPanelCommand(IPanel panel)
        {
            _panel = panel;
        }

        protected override void OnExecute()
        {
            this.GetSystem<IPanelSystem>().Back(_panel);
        }
    }
}