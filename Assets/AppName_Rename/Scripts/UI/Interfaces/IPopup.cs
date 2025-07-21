using UnityEngine;

namespace AppName_Rename.UI
{
    public enum PopupState
    {
        Loading,
        Showing,
        Hide,
        Closed
    }

    public interface IPopup : IUIBase
    {
        CanvasGroup CanvasGroup { get; }
        PopupInfo Info { get; set; }
        PopupState State { get; set; }
    }
}