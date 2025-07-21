using UnityEngine;

namespace AppName_Rename.UI
{
    /// <summary>
    /// Interface representing the base functionality for UI elements.
    /// </summary>
    public interface IUIBase
    {
        Transform Transform { get; }
        object AssetReference { get; set; }

        void Init(IUIData uiData);
        void UpdateAndShow(IUIData uiData);
        void Show();
        void Hide();
        //void HideDelay(float delay);
        void Close(bool destroy = false);
    }
}