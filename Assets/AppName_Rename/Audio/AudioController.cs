using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AppName_Rename
{
    [MonoSingletonPath("[Audio]/AudioController")]
    public class AudioController : QFramework.MonoSingleton<AudioController>
    {
        //public AssetReferenceT<AudioClip> bgm1;

        public override void OnSingletonInit()
        {
            AudioKit.Config.AudioLoaderPool = new AudioAssetLoaderPool();
        }

        private void Awake()
        {
            //gameObject.AddComponent<AudioListener>();
            // I remove the AudioListener because AudioManager have 1 too.
        }

        protected override void OnDestroy()
        {
            AudioKit.Config.AudioLoaderPool = null;

            base.OnDestroy();
        }
    }
}