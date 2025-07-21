using System;
using System.Collections.Generic;
using System.Linq;
using AppName_Rename.UI;
using QFramework;
using IPopup = AppName_Rename.UI.IPopup;
namespace AppName_Rename.Utility
{
    public class PopupUITable : Table<IPopup>, IUtility
    {
        public TableIndex<ParentTransform, IPopup> TransformIndex = new(popup => popup.Info.ParentTransform);
        public TableIndex<Type, IPopup> TypeIndex = new(popup => popup.Info.PopupType);

        protected override void OnAdd(IPopup item)
        {
            TransformIndex.Add(item);
            TypeIndex.Add(item);
        }

        protected override void OnRemove(IPopup item)
        {
            TransformIndex.Remove(item);
            TypeIndex.Remove(item);
        }

        protected override void OnClear()
        {
            TransformIndex.Clear();
            TypeIndex.Clear();
        }

        public override IEnumerator<IPopup> GetEnumerator()
        {
            return TransformIndex.Dictionary.SelectMany(key => key.Value).GetEnumerator();
        }

        protected override void OnDispose()
        {
            TransformIndex.Dispose();
            TypeIndex.Dispose();

            TransformIndex = null;
            TypeIndex = null;
        }

        public IEnumerable<IPopup> GetPopupsByPopupUIKeys(PopupUISearchKeys popupUISearchKeys)
        {
            if (popupUISearchKeys == null)
                throw new ArgumentException(nameof(popupUISearchKeys));

            IEnumerable<IPopup> typeResults = null;
            if (popupUISearchKeys.UIType is not null)
                typeResults = TypeIndex.Get(popupUISearchKeys.UIType);

            var transformResults = TransformIndex.Get(popupUISearchKeys.ParentTransform);

            if (typeResults is not null && transformResults is not null)
                return typeResults.Intersect(transformResults);

            return typeResults ?? transformResults;
        }
    }
}
