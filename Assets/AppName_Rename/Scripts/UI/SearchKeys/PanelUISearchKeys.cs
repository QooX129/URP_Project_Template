using System;
using QFramework;

namespace AppName_Rename.UI
{
    public class PanelUISearchKeys : AbstractUISearchKeys
    {
        public ParentTransform ParentTransform;
        public IPanel Panel;

        public override void OnRecycled()
        {
            UIType = null;
            AssetReference = null;
            GameObjName = null;
            UIData = null;
            Panel = null;
            ParentTransform = ParentTransform.Gameplay;
        }

        public override void Recycle2Cache()
        {
            SafeObjectPool<PanelUISearchKeys>.Instance.Recycle(this);
        }

        public static PanelUISearchKeys Allocate()
        {
            return SafeObjectPool<PanelUISearchKeys>.Instance.Allocate();
        }
    }
}
