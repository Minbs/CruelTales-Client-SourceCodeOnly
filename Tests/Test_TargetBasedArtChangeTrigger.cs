using System;
using System.Collections.Generic;
using CTC.DebugTools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

namespace CTC.Tests
{
	public class Test_TargetBasedArtChangeTrigger : MonoBehaviour
	{
		public Transform TargetTransform = null;
		public BoxGizmo TriggerBox;

		public Test_TargetBasedArtChangeObject[] TargetObjects = null;
		public float TargetSpriteRendererAlpha = 0f;
		public Vector3 TargetDirectionalLightAngle = Vector3.zero;
		public float TargetDirectionalLightIntensity = 0f;
		public Color TargetDirectionalLightColor = Color.white;
		
		public bool IsTriggered { get; private set; } = false;


		public void Update()
		{
			if (ReferenceEquals(TargetTransform, null))
				return;

			if (ReferenceEquals(TargetObjects, null))
				return;
			
			if (isInBox() && !IsTriggered)
			{
				foreach (var VARIABLE in TargetObjects)
				{
					VARIABLE.LerpSpriteRenderer(TargetSpriteRendererAlpha);
					VARIABLE.LerpDirectionalLight(TargetDirectionalLightAngle,
						TargetDirectionalLightIntensity, TargetDirectionalLightColor);

					IsTriggered = true;
				}
			}
			else if(!isInBox())
			{
				IsTriggered = false;
			}
		}

		private bool isInBox()
		{
			float targetX = TargetTransform.position.x;
			float targetZ = TargetTransform.position.z;
			float BoxLeft = transform.position.x - (transform.localScale.x / 2f);
			float BoxRight = transform.position.x + (transform.localScale.x / 2f);
			float BoxUp = transform.position.z + (transform.localScale.z / 2f);
			float BoxDown = transform.position.z - (transform.localScale.z / 2f);

			return targetX > BoxLeft && targetX < BoxRight &&
			       targetZ > BoxDown && targetZ < BoxUp;
		}
		
		[Button]
		public void BindTarget(Transform targetTransform)
		{
			if (ReferenceEquals(targetTransform, null))
				return;

			TargetTransform = targetTransform;
		}

		[Button]
		public void ReleaseTarget()
		{
			TargetTransform = null;
		}
	}
}