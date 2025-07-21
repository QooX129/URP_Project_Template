using System;
using Cysharp.Threading.Tasks;

namespace AppName_Rename.UI
{
    public class LoadingScreenPanel : AbstractPanel, ILoadingScreenPanel
    {
        public LoadingScreenType loadingScreenType;

        protected override void OnPanelInit(IUIData uiData = null)
        {
            Hide();
        }

        protected override void OnPanelClose()
        {
        }

        public async UniTaskVoid DoFade(bool isOn, Action onCompleted, float duration = 0f)
        {
            CanvasGroup.alpha = isOn ? 0f : 1f;

            if (State != PanelState.Showing)
                gameObject.SetActive(true);

            if (duration == 0f)
                duration = FadeDuration;

            if (isOn)
                await FadeIn(duration);
            else
                await FadeOut(duration);

            onCompleted?.Invoke();
        }
    }
}