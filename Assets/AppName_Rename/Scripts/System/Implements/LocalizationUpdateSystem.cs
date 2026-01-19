using System.Text;
using I2.Loc;
using Markdig.Helpers;
using QFramework;
using UnityEngine;

namespace AppName_Rename.Core
{
    public class LocalizationUpdateSystem : AbstractSystem, ILocalizationUpdateSystem
    {
        protected override void OnInit()
        {
            ReadCSVFrom_Resources();
        }
        void ReadCSVFrom_Resources()
        {
            //Big 5 encoding for Traditional Chinese
            string csvPath = "Assets/Resources/i2LanguageTerm.csv";
            var asset = LocalizationReader.ReadCSVfile(csvPath, Encoding.GetEncoding(950));

            // Check for errors (file not found)
            if (asset == null)
            {
                Debug.LogWarning("Unable to load Localization data");
                return;
            }
            UseLocalizationCSV(asset);
        }

        void UseLocalizationCSV(string CSVfile)
        {
            // Source[0] is the I2Languages.asset
            I2.Loc.LocalizationManager.Sources[0].Import_CSV(string.Empty, CSVfile, eSpreadsheetUpdateMode.Replace, ',');

            LocalizationManager.LocalizeAll();    // Force localing all enabled labels/sprites with the new data
            LocalizationManager.UpdateSources();
        }
    }
}