using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace AppName_Rename.Core
{
    public class AssetSystem : AbstractSystem, IAssetSystem
    {
        private static readonly Dictionary<string, List<string>> LabelKeys = new();

        protected override void OnInit()
        {

        }

        public async UniTask InitializeAsync()
        {
            await AddressablesManager.InitializeAsync();
            AddressablesManager.SuppressWarningLogs = true;
        }

        public bool ValidateAsset<T>(string key) where T : Object
        {
            return AddressablesManager.ContainsAsset(key);
        }

        public async UniTask<GameObject> InstantiateInstanceAsync(AssetReferenceGameObject reference,
            Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            var result = await AddressablesManager.InstantiateAsync(reference, parent, instantiateInWorldSpace);
            return result.Value;
        }

        public async UniTask<GameObject> InstantiateInstanceAsync(string key, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            var result = await AddressablesManager.InstantiateAsync(key, parent, instantiateInWorldSpace);
            return result.Value;
        }

        public GameObject InstantiateInstance(AssetReferenceGameObject reference, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            var result = AddressablesManager.InstantiateSync(reference, parent, instantiateInWorldSpace);
            return result;
        }

        public GameObject InstantiateInstance(string key, Transform parent = null, bool instantiateInWorldSpace = false)
        {
            var result = AddressablesManager.InstantiateSync(key, parent, instantiateInWorldSpace);
            return result;
        }

        public async UniTask<T> GetAssetAsync<T>(AssetReferenceT<T> reference) where T : Object
        {
            if (AddressablesManager.TryGetAsset<T>(reference, out var asset))
                return asset;

            return await AddressablesManager.LoadAssetAsync(reference);
        }

        public async UniTask<T> GetAssetAsync<T>(string key) where T : Object
        {
            if (AddressablesManager.TryGetAsset<T>(key, out var asset))
                return asset;

            return await AddressablesManager.LoadAssetAsync<T>(key);
        }

        public T GetAsset<T>(string key) where T : Object
        {
            return AddressablesManager.TryGetAsset<T>(key, out var asset)
                ? asset
                : AddressablesManager.LoadAssetSync<T>(key);
        }

        public T GetAsset<T>(AssetReferenceT<T> reference) where T : Object
        {
            return AddressablesManager.TryGetAsset<T>(reference, out var asset)
                ? asset
                : AddressablesManager.LoadAssetSync(reference);
        }

        public async UniTask<SceneInstance> LoadSceneAsync(AssetReference reference, LoadSceneMode mode)
        {
            if (AddressablesManager.TryGetScene(reference, out var scene))
                return scene;

            var result = await AddressablesManager.LoadSceneAsync(reference, mode);
            return result.Value;
        }

        public async UniTask<SceneInstance> LoadSceneAsync(string key, LoadSceneMode mode)
        {
            if (AddressablesManager.TryGetScene(key, out var scene))
                return scene;

            var result = await AddressablesManager.LoadSceneAsync(key, mode);
            return result.Value;
        }

        public SceneInstance LoadScene(AssetReference reference, LoadSceneMode mode)
        {
            return AddressablesManager.TryGetScene(reference, out var scene)
                ? scene
                : AddressablesManager.LoadSceneSync(reference);
        }

        public SceneInstance LoadScene(string key, LoadSceneMode mode)
        {
            return AddressablesManager.TryGetScene(key, out var scene)
                ? scene
                : AddressablesManager.LoadSceneSync(key);
        }

        public async UniTask<IEnumerable<T>> GetAssetsAsync<T>(IList<string> keys) where T : Object
        {
            var result = new List<T>();
            var tasks = new List<UniTask<T>>();

            foreach (var key in keys)
            {
                if (AddressablesManager.TryGetAsset<T>(key, out var asset))
                    result.Add(asset);
                else
                    tasks.Add(AddressablesManager.LoadAssetAsync<T>(key).ContinueWith(t => t.Value));
            }

            if (tasks.Count <= 0)
                return result;

            var missingAssets = await UniTask.WhenAll(tasks);
            result.AddRange(missingAssets);
            return result;
        }

        public IEnumerable<T> GetAssets<T>(IList<string> keys) where T : Object
        {
            var result = new List<T>();
            foreach (var key in keys)
            {
                if (AddressablesManager.TryGetAsset<T>(key, out var asset))
                {
                    result.Add(asset);
                }
                else
                {
                    var missingAsset = AddressablesManager.LoadAssetSync<T>(key);
                    if (missingAsset is not null)
                        result.Add(missingAsset);
                }
            }

            return result;
        }

        public async UniTask<IEnumerable<T>> GetAssetsByLabelAsync<T>(IList<string> keys) where T : Object
        {
            var result = new List<T>();
            var tasks = new List<UniTask<T>>();

            foreach (var key in keys)
            {
                if (LabelKeys.ContainsKey(key))
                    ProcessExistingKey(key, result);
                else
                    await ProcessMissingKeyAsync(key, tasks);
            }

            if (tasks.Count <= 0)
                return result;

            var missingAssets = await UniTask.WhenAll(tasks);
            result.AddRange(missingAssets);
            return result;
        }

        public IEnumerable<T> GetAssetsByLabel<T>(IList<string> keys) where T : Object
        {
            var result = new List<T>();
            foreach (var key in keys)
            {
                if (LabelKeys.ContainsKey(key))
                    ProcessExistingKey(key, result);
                else
                    ProcessMissingKey(key, result);
            }

            return result;
        }

        public bool TryGetScene(AssetReference reference, out SceneInstance scene)
        {
            return AddressablesManager.TryGetScene(reference, out scene);
        }

        public bool TryGetScene(string key, out SceneInstance scene)
        {
            return AddressablesManager.TryGetScene(key, out scene);
        }

        public void ReleaseInstance(AssetReferenceGameObject reference, GameObject instance)
        {
            AddressablesManager.ReleaseInstance(reference, instance);
        }

        public void ReleaseInstance(string key, GameObject instance)
        {
            AddressablesManager.ReleaseInstance(key, instance);
        }

        public void ReleaseAsset<T>(string key) where T : Object
        {
            AddressablesManager.ReleaseAsset(key);
        }

        public void ReleaseAsset<T>(AssetReferenceT<T> reference) where T : Object
        {
            AddressablesManager.ReleaseAsset(reference);
        }

        public async UniTask<SceneInstance> UnloadSceneAsync(AssetReference reference)
        {
            if (!AddressablesManager.TryGetScene(reference, out _))
            {
                Debug.LogWarning($"[{nameof(AssetSystem)}]: Scene with reference {reference} is not loaded");
                return default;
            }

            var result = await AddressablesManager.UnloadSceneAsync(reference);
            return result.Value;
        }

        public async UniTask<SceneInstance> UnloadSceneAsync(string key)
        {
            var result = await AddressablesManager.UnloadSceneAsync(key);

            return result.Value;
        }

        private static void ProcessExistingKey<T>(string key, List<T> result) where T : Object
        {
            if (!LabelKeys.TryGetValue(key, out var paths))
                return;

            foreach (var path in paths)
            {
                if (AddressablesManager.TryGetAsset<T>(path, out var asset))
                    result.Add(asset);
            }
        }

        private static async UniTask ProcessMissingKeyAsync<T>(string key, List<UniTask<T>> tasks) where T : Object
        {
            var loc = await AddressablesManager.LoadLocationsAsync(key);
            LabelKeys[key] = new List<string>();
            foreach (var r in loc.Value)
            {
                LabelKeys[key].Add(r.PrimaryKey);
                tasks.Add(AddressablesManager.LoadAssetAsync<T>(r.PrimaryKey).ContinueWith(t => t.Value));
            }
        }

        private static void ProcessMissingKey<T>(string key, List<T> result) where T : Object
        {
            var loc = AddressablesManager.LoadLocationsSync(key);
            LabelKeys[key] = new List<string>();
            foreach (var r in loc)
            {
                LabelKeys[key].Add(r.PrimaryKey);
                var asset = AddressablesManager.LoadAssetSync<T>(r.PrimaryKey);
                if (asset is not null)
                    result.Add(asset);
            }
        }
    }
}