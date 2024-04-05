using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using UnityEngine;

namespace CTC.Locators
{
	public class TeleporterGizmo : InteractorGizmo
	{
		public Transform Destination;
		public TeleporterShapeType ShapeType;
		public byte SectionTo;

		public override InteractorInfo CreateInfo()
		{
			InteractorInfo info = base.CreateInfo();

			info.Destination = Destination.position.ToNativeVector2();
			info.TeleporterShape = ShapeType;
			info.SectionTo = SectionTo;

			return info;
		}
	}
}