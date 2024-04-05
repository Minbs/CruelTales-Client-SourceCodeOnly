using CT.Common.Tools.Data;
using UnityEngine;

namespace CTC.SystemCore
{
	public class ConfigManager : IManager
	{
		public string AnnouncementUrl { get; private set; }

		public void Initialize()
		{
			var textAsset = Resources.Load(Global.Path.ClientConfig) as TextAsset;
			var configTable = DataHandler.ReadAsPairs(textAsset.text, DataHandler.TSV_PARSE_OPTION);

			AnnouncementUrl = configTable[nameof(AnnouncementUrl)];
		}

		public void Release()
		{
		}
	}
}