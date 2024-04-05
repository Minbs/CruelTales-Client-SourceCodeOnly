using System.Collections;
using Sirenix.OdinInspector;

namespace CTC.Data.Spine
{
	public static class DokzaAnimationVDLExtension
	{
		public static IEnumerable DokzaAnimationVDL = new ValueDropdownList<string>()
		{
			{ "None", "none" },
			{ "Action_Push", "Action_Push" },
			{ "Action_Pushed", "Action_Pushed" },
			{ "Action_wolf_attack_side", "Action_wolf_attack_side" },
			{ "Action_wolf_attack_up", "Action_wolf_attack_up" },
			{ "test/back_attach", "test/back_attach" },
			{ "Redhood_mission/Bird", "Redhood_mission/Bird" },
			{ "Redhood_mission/Clean1", "Redhood_mission/Clean1" },
			{ "Redhood_mission/Clean2", "Redhood_mission/Clean2" },
			{ "Costume_wolftail", "Costume_wolftail" },
			{ "Redhood_mission/Drink", "Redhood_mission/Drink" },
			{ "Redhood_mission/Fireplace", "Redhood_mission/Fireplace" },
			{ "Redhood_mission/Flower", "Redhood_mission/Flower" },
			{ "Redhood_mission/Food", "Redhood_mission/Food" },
			{ "Redhood_mission/Herb", "Redhood_mission/Herb" },
			{ "Idle_Back", "Idle_Back" },
			{ "Idle_Back_sleep", "Idle_Back_sleep" },
			{ "Idle_Front", "Idle_Front" },
			{ "Idle_Front_sleep", "Idle_Front_sleep" },
			{ "Redhood_mission/Letter", "Redhood_mission/Letter" },
			{ "Redhood_mission/Pack", "Redhood_mission/Pack" },
			{ "test/R", "test/R" },
			{ "Run_Back", "Run_Back" },
			{ "Run_Front", "Run_Front" },
			{ "Redhood_mission/Stump", "Redhood_mission/Stump" },
			{ "test/test", "test/test" },
			{ "Walk_Back", "Walk_Back" },
			{ "Walk_Front", "Walk_Front" },
			{ "Redhood_mission/Wanted", "Redhood_mission/Wanted" },
		};
	}
}
