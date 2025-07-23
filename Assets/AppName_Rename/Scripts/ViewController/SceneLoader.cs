using Cysharp.Threading.Tasks;
using AppName_Rename.Core;
using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IController
{
    [SerializeField] AssetReference _gameplayScene;

    public void OnInit()
    {
        LoadGameplayScene().Forget();
    }
    private async UniTaskVoid LoadGameplayScene()
    {
        var scene = await this.GetSystem<IAssetSystem>().LoadSceneAsync(_gameplayScene, LoadSceneMode.Additive);
        if (scene.Scene == null)
        {
            Debug.LogError("Failed to load gameplay scene.");
            return;
        }
        while (scene.Scene.isLoaded == false)
        {
            await UniTask.Yield();
        }
        Debug.Log("Gameplay scene loaded successfully.");
    }
    public IArchitecture GetArchitecture()
    {
        return AppArchitecture_Rename.Interface;
    }
}
