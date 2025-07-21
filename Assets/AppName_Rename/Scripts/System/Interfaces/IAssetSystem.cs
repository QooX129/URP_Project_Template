using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace AppName_Rename.Core
{
    public interface IAssetSystem : ISystem
    {
        UniTask InitializeAsync();
        bool ValidateAsset<T>(string key) where T : Object;

        UniTask<GameObject> InstantiateInstanceAsync(AssetReferenceGameObject reference, Transform parent = null,
            bool instantiateInWorldSpace = false);

        UniTask<GameObject> InstantiateInstanceAsync(string key, Transform parent = null,
            bool instantiateInWorldSpace = false);

        GameObject InstantiateInstance(AssetReferenceGameObject reference, Transform parent = null,
            bool instantiateInWorldSpace = false);

        GameObject InstantiateInstance(string key, Transform parent = null, bool instantiateInWorldSpace = false);
        UniTask<T> GetAssetAsync<T>(string key) where T : Object;
        UniTask<T> GetAssetAsync<T>(AssetReferenceT<T> reference) where T : Object;
        T GetAsset<T>(string key) where T : Object;
        T GetAsset<T>(AssetReferenceT<T> reference) where T : Object;
        UniTask<SceneInstance> LoadSceneAsync(AssetReference reference, LoadSceneMode mode);
        UniTask<SceneInstance> LoadSceneAsync(string key, LoadSceneMode mode);
        SceneInstance LoadScene(AssetReference reference, LoadSceneMode mode);
        SceneInstance LoadScene(string key, LoadSceneMode mode);
        UniTask<IEnumerable<T>> GetAssetsAsync<T>(IList<string> keys) where T : Object;
        IEnumerable<T> GetAssets<T>(IList<string> keys) where T : Object;
        UniTask<IEnumerable<T>> GetAssetsByLabelAsync<T>(IList<string> keys) where T : Object;
        IEnumerable<T> GetAssetsByLabel<T>(IList<string> keys) where T : Object;
        bool TryGetScene(AssetReference reference, out SceneInstance scene);
        bool TryGetScene(string key, out SceneInstance scene);
        void ReleaseInstance(AssetReferenceGameObject reference, GameObject instance);
        void ReleaseInstance(string key, GameObject instance);
        void ReleaseAsset<T>(string key) where T : Object;
        void ReleaseAsset<T>(AssetReferenceT<T> reference) where T : Object;
        UniTask<SceneInstance> UnloadSceneAsync(AssetReference reference);
        UniTask<SceneInstance> UnloadSceneAsync(string key);
    }
}