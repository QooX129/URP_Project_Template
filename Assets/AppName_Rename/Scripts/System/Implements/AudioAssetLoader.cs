using System;
using Cysharp.Threading.Tasks;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;

namespace AppName_Rename
{
    public class AudioAssetLoaderPool : AbstractAudioLoaderPool, IController
    {
        protected override IAudioLoader CreateLoader()
        {
            return new AudioAssetLoader(this.GetSystem<IAssetSystem>());
        }

        public IArchitecture GetArchitecture()
        {
            return AppArchitecture.Interface;
        }
    }

    public class AudioAssetLoader : IAudioLoader
    {
        public AudioClip Clip => _clip;

        private readonly IAssetSystem _assetSystem;
        private AudioClip _clip;
        private string _assetAddress;

        public AudioAssetLoader(IAssetSystem assetSystem)
        {
            _assetSystem = assetSystem;
        }

        public AudioClip LoadClip(AudioSearchKeys audioSearchKeys)
        {
            _assetAddress = audioSearchKeys.AssetName;
            _clip = _assetSystem.GetAsset<AudioClip>(_assetAddress);
            return _clip;
        }

        public void LoadClipAsync(AudioSearchKeys audioSearchKeys, Action<bool, AudioClip> onLoad)
        {
            _assetAddress = audioSearchKeys.AssetName;
            LoadClipTask(audioSearchKeys, onLoad).Forget();
        }

        public void Unload()
        {
            _assetSystem.ReleaseAsset<AudioClip>(_assetAddress);

            _clip = null;
            _assetAddress = null;
        }

        private async UniTaskVoid LoadClipTask(AudioSearchKeys audioSearchKeys, Action<bool, AudioClip> onLoad)
        {
            _clip = await _assetSystem.GetAssetAsync<AudioClip>(audioSearchKeys.AssetName);
            onLoad?.Invoke(true, _clip);
        }
    }
}