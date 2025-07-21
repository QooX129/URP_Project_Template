using System;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public class ContextInfo : IPoolType, IPoolable
    {
        public bool IsRecycled { get; set; }

        public object AssetReference;
        public string GameObjName;
        public IUIBase ParentUI;
        public IUIData UIData;
        public Type UIType;
        public Transform RootTransform;
        public RectTransform RectTransform;

        public void OnRecycled()
        {
            AssetReference = null;
            GameObjName = null;
            ParentUI = null;
            UIData = null;
            UIType = null;
            RootTransform = null;
            RectTransform = null;
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<ContextInfo>.Instance.Recycle(this);
        }

        public static ContextInfo Allocate(string gameObjName, IUIBase parentUI, IUIData uiData, Type uiType,
            object assetReference, Transform rootTransform, RectTransform rectTransform)
        {
            var contextInfo = SafeObjectPool<ContextInfo>.Instance.Allocate();

            contextInfo.GameObjName = gameObjName;
            contextInfo.ParentUI = parentUI;
            contextInfo.UIData = uiData;
            contextInfo.UIType = uiType;
            contextInfo.AssetReference = assetReference;
            contextInfo.RootTransform = rootTransform;
            contextInfo.RectTransform = rectTransform;

            return contextInfo;
        }
    }
}