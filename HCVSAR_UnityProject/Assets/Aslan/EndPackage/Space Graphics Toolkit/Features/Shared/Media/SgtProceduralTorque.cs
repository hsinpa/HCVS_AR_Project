﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceGraphicsToolkit
{
	/// <summary>This component rotates the current GameObject along a random axis, with a random speed.</summary>
	[RequireComponent(typeof(Rigidbody))]
	[HelpURL(SgtHelper.HelpUrlPrefix + "SgtProceduralTorque")]
	[AddComponentMenu(SgtHelper.ComponentMenuPrefix + "Procedural Torque")]
	public class SgtProceduralTorque : SgtProcedural
	{
		/// <summary>Minimum degrees per second.</summary>
		public float SpeedMin;

		/// <summary>Maximum degrees per second.</summary>
		public float SpeedMax = 10.0f;

		protected override void DoGenerate()
		{
			var axis  = Random.onUnitSphere;
			var speed = Random.Range(SpeedMin, SpeedMax);

			transform.localRotation = Random.rotation;

			GetComponent<Rigidbody>().angularVelocity = axis * speed * Mathf.Deg2Rad;
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SgtProceduralTorque))]
	public class SgtProceduralTorque_Editor : SgtProcedural_Editor<SgtProceduralTorque>
	{
		protected override void OnInspector()
		{
			base.OnInspector();

			Draw("SpeedMin", "Minimum degrees per second.");
			Draw("SpeedMax", "Maximum degrees per second.");
		}
	}
}
#endif