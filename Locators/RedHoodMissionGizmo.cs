using CT.Common.Gameplay.Infos;
using CT.Common.Gameplay.RedHood;

namespace CTC.Locators
{
	public class RedHoodMissionGizmo : InteractorGizmo
	{
		public RedHoodMission MissionType;

		public override InteractorInfo CreateInfo()
		{
			InteractorInfo info = base.CreateInfo();
			info.RedHoodMission = MissionType;

			return info;
		}
	}
}