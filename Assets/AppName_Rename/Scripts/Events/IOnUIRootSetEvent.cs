using System;
using UnityEngine;

namespace AppName_Rename
{
    public interface IOnUIRootSetEvent
    {
        Transform PanelRootTransform { get; }
        Transform PopupRootTransform { get; }
    }

    public record OnUIRootSetEvent(Transform panelRootTransform, Transform popupRootTransform) : IOnUIRootSetEvent
    {
        public Transform PanelRootTransform { get; }
        public Transform PopupRootTransform { get; }
    }
}