using UnityEngine;

namespace I2.Loc
{
	public static class ScriptLocalization
	{

		public static string text_testKey 		{ get{ return LocalizationManager.GetTranslation ("text_testKey"); } }
	}

    public static class ScriptTerms
	{

		public const string text_testKey = "text_testKey";
	}
}