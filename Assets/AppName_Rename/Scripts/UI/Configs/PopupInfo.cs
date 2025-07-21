using System;
using AppName_Rename.Core;
using QFramework;
namespace AppName_Rename.UI
{
    public class PopupInfo : IPoolType, IPoolable
    {
        public bool IsRecycled { get; set; }

        public IUIData UIData;
        public object AssetReference;
        public string GameObjName;
        public Type PopupType;
        public ParentTransform ParentTransform;

        public void OnRecycled()
        {
            UIData = null;
            AssetReference = null;
            GameObjName = null;
            PopupType = null;
            ParentTransform = ParentTransform.Gameplay;
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<PopupInfo>.Instance.Recycle(this);
        }

        public static PopupInfo Allocate(string gameObjName, IUIData uiData, Type panelType, object assetRef,
            ParentTransform parentTransform)
        {
            var popupInfo = SafeObjectPool<PopupInfo>.Instance.Allocate();

            popupInfo.GameObjName = gameObjName;
            popupInfo.UIData = uiData;
            popupInfo.PopupType = panelType;
            popupInfo.AssetReference = assetRef;
            popupInfo.ParentTransform = parentTransform;

            return popupInfo;
        }
    }
}
