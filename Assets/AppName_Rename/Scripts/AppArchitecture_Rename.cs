using AppName_Rename.UI;
using AppName_Rename.Utilities;
using AppName_Rename.Utility;
using QFramework;

namespace AppName_Rename.Core
{
    public class AppArchitecture_Rename : Architecture<AppArchitecture_Rename>
    {
        protected override void Init()
        {
            //Model
            RegisterModel<IPlayerModel>(new PlayerModel());

            //System
            RegisterSystem<IAssetSystem>(new AssetSystem());
            RegisterSystem<IPanelSystem>(new PanelSystem());
            RegisterSystem<IPopupSystem>(new PopupSystem());

            //Utility
        }
    }
}
