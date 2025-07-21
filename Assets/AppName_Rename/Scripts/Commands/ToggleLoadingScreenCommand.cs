using System;
using Cysharp.Threading.Tasks;
using AppName_Rename.UI;
using QFramework;

namespace AppName_Rename
{
    public class ToggleLoadingScreenCommand : AbstractCommand
    {
        private readonly LoadingScreenType _type;
        private readonly bool _isOn;
        private readonly bool _needOverlay;
        private readonly Action _onComplete;
        private readonly Func<UniTask> _onCompleteAsync;

        public ToggleLoadingScreenCommand(LoadingScreenType type, bool isOn, Action onComplete,
            bool needOverlay = false)
        {
            _type = type;
            _isOn = isOn;
            _onComplete = onComplete;
            _needOverlay = needOverlay;
        }

        public ToggleLoadingScreenCommand(LoadingScreenType type, bool isOn, Func<UniTask> onComplete,
            bool needOverlay = false)
        {
            _type = type;
            _isOn = isOn;
            _onCompleteAsync = onComplete;
            _needOverlay = needOverlay;
        }

        protected override void OnExecute()
        {
            if (_onCompleteAsync != null)
            {
                this.GetSystem<IPanelSystem>().ToggleLoadingScreen(_type, _isOn, _needOverlay, _onCompleteAsync);
                return;
            }

            this.GetSystem<IPanelSystem>().ToggleLoadingScreen(_type, _isOn, _needOverlay, _onComplete);
        }
    }
}