// using AillieoUtils;
using UnityEngine;

namespace AppName_Rename.UI
{
    public enum PanelState
    {
        Loading,
        Showing,
        Hide,
        Closed
    }

    public interface IPanel : IUIBase
    {
        CanvasGroup CanvasGroup { get; }
        PanelInfo Info { get; set; }
        PanelState State { get; set; }

        // void JumpTo(PanelPath path, IUIData uiData = null);

        // void SetScrollViewFunctions<TContainer, TData>(TData[] dataSet, ScrollView scrollView)
        //     where TContainer : UIContext where TData : IUIData;
    }
}