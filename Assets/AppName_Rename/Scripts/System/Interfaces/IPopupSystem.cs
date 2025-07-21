namespace AppName_Rename.UI
{
    /// <summary>
    /// Defines the interface for a popup system that manages the display and removal of popup windows
    /// </summary>
    public interface IPopupSystem : IUISystem<IPopup, PopupUISearchKeys>
    {
        //PopupQueue

        T GetPopup<T>() where T : class, IPopup;
        AbstractPopup GetPopup(PopupUISearchKeys searchKeys);

    }
}