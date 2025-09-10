using AppName_Rename.Core;
using QFramework;
using UnityEngine;

namespace AppName_Rename
{
    public class InitializationLoader : MonoBehaviour, IController
    {
        [SerializeField] SceneLoader _sceneLoader;

        void Start()
        {
            AudioController.Instance.OnSingletonInit();

            _sceneLoader.OnInit();
            DestroyImmediate(gameObject);
        }
        public IArchitecture GetArchitecture()
        {
            return AppArchitecture.Interface;
        }
    }
}