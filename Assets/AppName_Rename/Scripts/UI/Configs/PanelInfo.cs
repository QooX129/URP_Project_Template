using System;
using QFramework;

namespace AppName_Rename.UI
{
    public class PanelInfo : IPoolType, IPoolable
    {
        public bool IsRecycled { get; set; }

        public IUIData UIData;
        public object AssetReference;
        public string GameObjName;
        public Type PanelType;
        public ParentTransform ParentTransform;

        public void OnRecycled()
        {
            UIData = null;
            AssetReference = null;
            GameObjName = null;
            PanelType = null;
            ParentTransform = ParentTransform.Gameplay;
        }

        public void Recycle2Cache()
        {
            SafeObjectPool<PanelInfo>.Instance.Recycle(this);
        }

        public static PanelInfo Allocate(string gameObjName, IUIData uiData, Type panelType, object assetRef,
            ParentTransform parentTransform = ParentTransform.Gameplay)
        {
            var panelInfo = SafeObjectPool<PanelInfo>.Instance.Allocate();

            panelInfo.GameObjName = gameObjName;
            panelInfo.UIData = uiData;
            panelInfo.PanelType = panelType;
            panelInfo.AssetReference = assetRef;
            panelInfo.ParentTransform = parentTransform;

            return panelInfo;
        }
    }
}
