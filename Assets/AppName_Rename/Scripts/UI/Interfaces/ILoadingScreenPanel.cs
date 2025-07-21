using System;
using Cysharp.Threading.Tasks;

namespace AppName_Rename.UI
{
    public interface ILoadingScreenPanel : IPanel
    {
        // public void OnLoadingScreenEvent(ILoadingScreenEvent loadingScreenEvent);
        public UniTaskVoid DoFade(bool isOn, Action onCompleted, float duration = 0f);
    }
}