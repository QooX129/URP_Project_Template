using AppName_Rename.Core;
using QFramework;
using UnityEngine;

public class InitializationLoader : MonoBehaviour, IController
{
    [SerializeField] SceneLoader _sceneLoader;

    void Start()
    {
        _sceneLoader.OnInit();
        DestroyImmediate(gameObject);
    }
    public IArchitecture GetArchitecture()
    {
        return AppArchitecture_Rename.Interface;
    }
}