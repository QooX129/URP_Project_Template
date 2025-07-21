using QFramework;

namespace AppName_Rename.UI
{
    public class PopupUISearchKeys : AbstractUISearchKeys
    {
        public ParentTransform ParentTransform;
        public IPopup Popup;

        public override void OnRecycled()
        {
            UIType = null;
            AssetReference = null;
            GameObjName = null;
            UIData = null;
            Popup = null;
            ParentTransform = ParentTransform.Gameplay;
        }

        public override void Recycle2Cache()
        {
            SafeObjectPool<PopupUISearchKeys>.Instance.Recycle(this);
        }

        public static PopupUISearchKeys Allocate()
        {
            return SafeObjectPool<PopupUISearchKeys>.Instance.Allocate();
        }
    }
}
