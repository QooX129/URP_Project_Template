using QFramework;
using AppName_Rename.UI;
using UnityEngine.UI;
using UnityEngine;

namespace AppName_Rename
{
    public class PanelTemplate : AbstractPanel, IPanelTemplate
    {
        private Button _startBtn;
        protected override void OnPanelInit(IUIData uiData = null)
        {
            // _startBtn = GetReference<Button>("Btn_Start");

            // this.SendCommand(new OpenPanelCommand<SettingPanel>(AssetAddress.SettingPanel, nameof(SettingPanel), null, true));
        }
        protected override void OnPanelClose()
        {
        }
    }
}
