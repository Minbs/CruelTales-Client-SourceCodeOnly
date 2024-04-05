using System;
using CT.Common.Gameplay;
using CT.Common.Gameplay.Infos;
using CTC.DebugTools;
using UnityEditor;
using UnityEngine;

namespace CTC.Locators
{
	[RequireComponent(typeof(BoxGizmo))]
	public class InteractorGizmo : MonoBehaviour
	{
		public InteractorColliderShapeType ColliderShapeType;
		public InteractorType InteractorType;
		public InteractionBehaviourType BehaviourType;
		public float ProgressTime = 3.0f;
		public float Cooltime;

		public float DonutOuter = 1.0f;
		public float DonutInner = 0.5f;

		public Color FillColor = Color.white;
		public Color LineColor = Color.white;

#if UNITY_EDITOR
		public void OnValidate()
		{
			Reset();
		}

		public virtual void OnDrawGizmos()
		{
			Handles.Label(transform.position, name);
		}

		public virtual void Reset()
		{
			BoxGizmo gizmo = GetComponent<BoxGizmo>();
			gizmo.GizmoFillColor = FillColor;
			gizmo.GizmoLineColor = LineColor;
		}
#endif

		public virtual InteractorInfo CreateInfo()
		{
			var info = new InteractorInfo()
			{
				InteractorType = InteractorType,
				BehaviourType = BehaviourType,
				Position = transform.position.ToNativeVector2(),
				ProgressTime = ProgressTime,
				Cooltime = Cooltime,
			};

			Vector2 size = transform.localScale;

			info.Size.ShapeType = ColliderShapeType;
			switch (ColliderShapeType)
			{
				case InteractorColliderShapeType.Box:
					info.Size.Width = size.x;
					info.Size.Height = size.y;
					break;

				case InteractorColliderShapeType.Circle:
					info.Size.Radius = size.x;
					break;

				case InteractorColliderShapeType.Donut:
					info.Size.RadiusOuter = DonutOuter;
					info.Size.RadiusInner = DonutInner;
					break;

				default:
					throw new ArgumentException($"There is no shape type in {name}");
			}
			return info;
		}
	}
}
