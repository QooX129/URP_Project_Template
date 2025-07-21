using UnityEngine;

namespace AppName_Rename.UI
{
    public enum ContextState
    {
        Loading,
        Showing,
        Hide,
        Closed
    }

    public interface IContext : IUIBase
    {
        bool IsInitialized { get; }

        ContextInfo Info { get; set; }
        ContextState State { get; set; }
    }

    public interface ISpriteClickable
    {
        void OnSpriteClicked();
        void OnSpriteReleased();
    }

    public interface IColorProvider
    {
        Color GetLabelColor();
    }
}