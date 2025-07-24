using System;
using UnityEngine;

namespace AppName_Rename
{
    public interface IOnUIRootSetEvent
    {
        Transform PanelRootTransform { get; }
        Transform PopupRootTransform { get; }
    }

    public record OnUIRootSetEvent : IOnUIRootSetEvent
    {
        public Transform PanelRootTransform { get; }
        public Transform PopupRootTransform { get; }
        public OnUIRootSetEvent(Transform panelRootTransform, Transform popupRootTransform)
        {
            PanelRootTransform = panelRootTransform;
            PopupRootTransform = popupRootTransform;
        }
    }
}