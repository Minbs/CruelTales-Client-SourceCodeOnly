using System.Collections;
using Sirenix.OdinInspector;

namespace CTC.Data.Spine
{
	public static class DokzaSkinVDLExtension
	{
		public static IEnumerable DokzaSkinVDL = new ValueDropdownList<string>()
		{
			{ "None", "none" },
			{ "default", "default" },
			{ "Test", "Test" },
			{ "wolf", "wolf" },
		};
	}
}
