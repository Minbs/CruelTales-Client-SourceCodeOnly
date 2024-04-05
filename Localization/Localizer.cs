using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CT.Logger;
using CT.Common.Tools.Collections;
using CT.Common.Tools.Data;
using UnityEngine;

namespace CTC.Globalizations
{
	public static class Localizer
	{
		private static readonly ILog _log = LogManager.GetLogger(typeof(Localizer));

		private static readonly BidirectionalMap<LanguageType, string> mLanguageCodeTable = new()
		{
			//{ LanguageType.None,					"ERROR"		},		//	Error
			//{ LanguageType.Arabic,					"arabic"	},		//	ar
			//{ LanguageType.Bulgarian,				"bulgarian"	},		//	bg
			//{ LanguageType.Chinese_Simplified,		"schinese"	},		//	zh-CN
			//{ LanguageType.Chinese_Traditional,		"tchinese"	},		//	zh-TW
			//{ LanguageType.Czech,					"czech"		},		//	cs
			//{ LanguageType.Danish,					"danish"	},		//	da
			//{ LanguageType.Dutch,					"dutch"		},		//	nl
			{ LanguageType.English,                 "english"   },		//	en
			//{ LanguageType.Finnish,					"finnish"	},		//	fi
			//{ LanguageType.French,					"french"	},		//	fr
			//{ LanguageType.German,					"german"	},		//	de
			//{ LanguageType.Greek,					"greek"		},		//	el
			//{ LanguageType.Hungarian,				"hungarian"	},		//	hu
			//{ LanguageType.Italian,					"italian"	},		//	it
			//{ LanguageType.Japanese,				"japanese"	},		//	ja
			{ LanguageType.Korean,                  "koreana"   },		//	ko
			//{ LanguageType.Norwegian,				"norwegian"	},		//	no
			//{ LanguageType.Polish,					"polish"	},		//	pl
			//{ LanguageType.Portuguese,				"portuguese"},		//	pt
			//{ LanguageType.Portuguese_Brazil,		"brazilian"	},		//	pt-BR
			//{ LanguageType.Romanian,				"romanian"	},		//	ro
			//{ LanguageType.Russian,					"russian"	},		//	ru
			//{ LanguageType.Spanish_Spain,			"spanish"	},		//	es
			//{ LanguageType.Spanish_Latin_America,	"latam"		},		//	es-419
			//{ LanguageType.Swedish,					"swedish"	},		//	sv
			//{ LanguageType.Thai,					"thai"		},		//	th
			//{ LanguageType.Turkish,					"turkish"	},		//	tr
			//{ LanguageType.Ukrainian,				"ukrainian"	},		//	uk
			//{ LanguageType.Vietnamese,				"vietnamese"},		//	vn
		};

		private static Dictionary<string, string> mTextTable = new();

		private static readonly string ErrorText = "ERROR_";

		/// <summary>언어가 변경되면 호출됩니다.</summary>
		public static event Action OnLanguageChanged;

		private static LanguageType mCurrentLanguage = LanguageType.Korean;

		/// <summary>스팀 API가 제공하는 언어 코드로 언어를 설정합니다.</summary>
		/// <param name="steamworksApiLanguageCode">스팀 API의 언어 코드 문자열</param>
		public static void SetLanguage(string steamworksApiLanguageCode)
		{
			mCurrentLanguage = mLanguageCodeTable
				.TryGetValue(steamworksApiLanguageCode, out var code) ? code : LanguageType.English;
			mTextTable = loadLocalizationData(mCurrentLanguage);
			OnLanguageChanged?.Invoke();
		}

		/// <summary>언어 타입으로 언어를 설정합니다.</summary>
		/// <param name="languageType">언어 타입</param>
		public static void SetLanguage(LanguageType languageType)
		{
			mCurrentLanguage = (languageType == LanguageType.None) ? LanguageType.English : languageType;
			mTextTable = loadLocalizationData(mCurrentLanguage);
			OnLanguageChanged?.Invoke();
		}

		/// <summary>현재 설정된 언어를 가져옵니다.</summary>
		/// <returns>언어 타입입니다.</returns>
		public static LanguageType GetCurrentLanguage()
		{
			if (!mLanguageCodeTable.Contains(mCurrentLanguage))
			{
				mCurrentLanguage = LanguageType.Korean;
			}

			return mCurrentLanguage;
		}

		public static string GetText(string textType)
		{
			return mTextTable.TryGetValue(textType, out var result)
				? result : ErrorText + textType;
		}

		public static string GetText(TextKey textType)
		{
			return GetText(textType.ToString());
		}

		private static Dictionary<string, string> loadLocalizationData(LanguageType languageType)
		{
			var textAsset = Resources.Load(Global.Path.Localization) as TextAsset;
			var localizationTable = DataHandler.ReadAsTable(textAsset.text, DataHandler.TSV_PARSE_OPTION);

			string textKey = Global.Data.LocalizationKey;
			string languageKey = languageType.ToString();

			Dictionary<string, string> textTable = new();

			foreach (var line in localizationTable)
			{
				if (line.TryGetValue(textKey, out var keyData) &&
					line.TryGetValue(languageKey, out var valueData))
				{
					textTable.Add((string)keyData, (string)valueData);
				}
			}

			_log.Info($"Localization data load success! Count : {localizationTable.Count}");

			return textTable;
		}

		public static Dictionary<string, string> mDevelopLanguage = new();

		/// <summary>개발자의 언어입니다. 개발자의 언어로 에디터상에서 모든 Text가 설정됩니다.</summary>
		public static LanguageType DevelopLanguage => LanguageType.Korean;

		public static string GetTextAsDevelop(string textType)
		{
			if (mDevelopLanguage.Count == 0)
			{
				mDevelopLanguage = loadLocalizationData(DevelopLanguage);
			}

			return mDevelopLanguage.TryGetValue(textType, out var result)
				? result : ErrorText + textType;
		}

		public static string GetTextAsDevelop(TextKey textKey)
		{
			return GetTextAsDevelop(textKey.ToString());
		}

		public static void OnUpdateDevelopLocalizationData()
		{
			mDevelopLanguage = loadLocalizationData(DevelopLanguage);
		}

		public static string GetThisCodeFilePath([CallerFilePath] string path = null)
		{
			return path;
		}

		public static string GetThisCodeFileDirectory()
		{
			return Path.GetDirectoryName(GetThisCodeFilePath());
		}

		public static string GetThisCodeFileNameWithoutExtension()
		{
			return Path.GetFileNameWithoutExtension(GetThisCodeFilePath());
		}
	}
}
