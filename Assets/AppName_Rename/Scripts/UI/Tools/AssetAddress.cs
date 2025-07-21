using System;
using System.Collections.Generic;
using AppName_Rename.UI;

namespace AppName_Rename
{
    public static class AssetAddress
    {
        public const string FLoadingScreen = "Assets/MindverseApp/UI/Prefabs/P_LoadingScreenPanel_Full.prefab";
        public const string SLoadingScreen = "Assets/MindverseApp/UI/Prefabs/P_LoadingScreenPanel_Simple.prefab";
        public const string Overlay = "Assets/MindverseApp/UI/Prefabs/P_Overlay.prefab";

        #region Panels
        //Template: public const string MainMenuPanel = "Assets/MindverseApp/UI/Prefabs/P_MainMenuPanel.prefab";
        #endregion

        #region Popups
        //Template: public const string PurchasePopup = "Assets/MindverseApp/UI/Prefabs/P_PurchasePopup.prefab";
        #endregion

        #region Audio Assets
        //Template: public const string Bgm1 = "Assets/MindverseApp/Audio/bgm/Bgm_1.mp3";
        public const string SfxBack = "Assets/MindverseApp/Audio/sfx/(filename).mp3";
        #endregion
    }

    public static class AssetHelper
    {
        private static readonly Dictionary<Type, string> AssetAddressMap = new()
        {
            //Template: {typeof(MainMenuPanel), AssetAddress.MainMenuPanel},

            //Template: {typeof(RegistrationPopup), AssetAddress.RegistrationPopup},
        };

        public static string GetAssetAddress<T>()
        {
            if (AssetAddressMap.TryGetValue(typeof(T), out var address))
            {
                return address;
            }

            throw new ArgumentException($"No asset address found for type {typeof(T)}");
        }

        public static string GetAssetAddress(Type type)
        {
            if (AssetAddressMap.TryGetValue(type, out var address))
            {
                return address;
            }

            throw new ArgumentException($"No asset address found for type {type}");
        }

        public static void RegisterAssetAddress<T>(string address)
        {
            AssetAddressMap[typeof(T)] = address;
        }
    }
}