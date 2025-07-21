using System;
using QFramework;
using UnityEngine;

namespace AppName_Rename.UI
{
    public class ContextUISearchKeys : AbstractUISearchKeys
    {
        public IUIBase ParentUI;
        public Transform RootTransform;
        public RectTransform RectTransform;

        public override void OnRecycled()
        {
            UIType = null;
            AssetReference = null;
            GameObjName = null;
            ParentUI = null;
            UIData = null;
            RootTransform = null;
            RectTransform = null;
        }

        public override void Recycle2Cache()
        {
            SafeObjectPool<ContextUISearchKeys>.Instance.Recycle(this);
        }

        public static ContextUISearchKeys Allocate()
        {
            return SafeObjectPool<ContextUISearchKeys>.Instance.Allocate();
        }
    }
}