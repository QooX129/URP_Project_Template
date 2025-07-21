using System.Collections.Generic;
using AppName_Rename.Core;
using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class PanelUIStack : IController
    {
        private readonly Stack<PanelInfo> _panelStacks = new();

        public void Push<T>() where T : class, IPanel
        {
            Push(this.GetSystem<IPanelSystem>().GetPanel<T>());
        }

        public void Push(IPanel panel)
        {
            if (panel == null)
                return;

            _panelStacks.Push(panel.Info);
            panel.Hide();

            // panel.Close();
            //     
            // var panelSearchKeys = PanelUISearchKeys.Allocate();
            // panelSearchKeys.GameObjName = panel.Transform.name;
            // this.GetSystem<IPanelSystem>().RemoveFromCache(panelSearchKeys);
            // panelSearchKeys.Recycle2Cache();
        }

        public void Pop()
        {
            var previousInfo = _panelStacks.Pop();

            var panelSearchKeys = PanelUISearchKeys.Allocate();
            panelSearchKeys.GameObjName = previousInfo.GameObjName;
            panelSearchKeys.UIType = previousInfo.PanelType;
            panelSearchKeys.UIData = previousInfo.UIData;
            panelSearchKeys.AssetReference = previousInfo.AssetReference;
            panelSearchKeys.ParentTransform = previousInfo.ParentTransform;

            this.GetSystem<IPanelSystem>().ShowUI(panelSearchKeys);

            panelSearchKeys.Recycle2Cache();
        }

        public void Clear()
        {
            _panelStacks.Clear();
        }

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture_Rename.Interface;
        }
    }
}