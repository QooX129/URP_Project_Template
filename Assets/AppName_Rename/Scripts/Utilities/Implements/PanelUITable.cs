using System;
using System.Collections.Generic;
using System.Linq;
using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename.Utility
{
    public class PanelUITable : Table<IPanel>
    {
        public TableIndex<object, IPanel> AssetReferenceIndex = new(panel => panel.Info.AssetReference);
        public TableIndex<Type, IPanel> TypeIndex = new(panel => panel.Info.PanelType);
        public TableIndex<string, IPanel> GameObjNameIndex = new(panel => panel.Transform.name);

        protected override void OnAdd(IPanel item)
        {
            AssetReferenceIndex.Add(item);
            TypeIndex.Add(item);
            GameObjNameIndex.Add(item);
        }

        protected override void OnRemove(IPanel item)
        {
            AssetReferenceIndex.Remove(item);
            TypeIndex.Remove(item);
            GameObjNameIndex.Remove(item);
        }

        protected override void OnClear()
        {
            AssetReferenceIndex.Clear();
            TypeIndex.Clear();
            GameObjNameIndex.Clear();
        }

        public override IEnumerator<IPanel> GetEnumerator()
        {
            return AssetReferenceIndex.Dictionary.SelectMany(key => key.Value).GetEnumerator();
        }

        protected override void OnDispose()
        {
            AssetReferenceIndex.Dispose();
            TypeIndex.Dispose();
            GameObjNameIndex.Dispose();

            AssetReferenceIndex = null;
            TypeIndex = null;
            GameObjNameIndex = null;
        }

        public IEnumerable<IPanel> GetPanelsBySearchKeys(PanelUISearchKeys panelUISearchKeys)
        {
            if (panelUISearchKeys.UIType != null && panelUISearchKeys.AssetReference != null ||
                panelUISearchKeys.Panel != null)
            {
                return TypeIndex.Get(panelUISearchKeys.UIType)
                    .Where(panel =>
                        panel.Info.AssetReference == panelUISearchKeys.AssetReference ||
                        panel == panelUISearchKeys.Panel);
            }

            if (panelUISearchKeys.UIType != null)
            {
                return TypeIndex.Get(panelUISearchKeys.UIType);
            }

            if (panelUISearchKeys.Panel != null)
            {
                return AssetReferenceIndex.Get(panelUISearchKeys.AssetReference).Where(panel => panel == panelUISearchKeys.Panel);
            }

            if (panelUISearchKeys.GameObjName != null)
            {
                return GameObjNameIndex.Get(panelUISearchKeys.GameObjName);
            }

            return panelUISearchKeys.AssetReference != null
                ? AssetReferenceIndex.Get(panelUISearchKeys.AssetReference)
                : Enumerable.Empty<IPanel>();
        }
    }
}
