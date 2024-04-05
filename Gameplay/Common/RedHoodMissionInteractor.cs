#nullable enable
#pragma warning disable CS0649

using CT.Common.Gameplay.RedHood;
using CTC.Globalizations;

namespace CTC.Networks.SyncObjects.SyncObjects
{
	public partial class RedHoodMissionInteractor : Interactor
	{
		public override void OnCreated()
		{
			base.OnCreated();
			if (Mission != RedHoodMission.None)
			{
				InfoTextKey = TextKey.MG_RedHood_Mission + (int)Mission;
			}
		}
	}
}
