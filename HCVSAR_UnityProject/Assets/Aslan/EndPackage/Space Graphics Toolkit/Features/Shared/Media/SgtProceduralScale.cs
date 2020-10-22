﻿using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceGraphicsToolkit
{
	/// <summary>This component rotates the current GameObject along a random axis, with a random speed.</summary>
	[HelpURL(SgtHelper.HelpUrlPrefix + "SgtProceduralScale")]
	[AddComponentMenu(SgtHelper.ComponentMenuPrefix + "Procedural Scale")]
	public class SgtProceduralScale : SgtProcedural
	{
		/// <summary>The default scale of your object.</summary>
		public Vector3 BaseScale = Vector3.one;

		/// <summary>The minimum multiplication of the BaseScale.</summary>
		public float ScaleMultiplierMin = 1.0f;

		/// <summary>The maximum multiplication of the BaseScale.</summary>
		public float ScaleMultiplierMax = 2.0f;

		protected override void DoGenerate()
		{
			transform.localScale = BaseScale * Mathf.Lerp(ScaleMultiplierMin, ScaleMultiplierMax, Random.value);
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SgtProceduralScale))]
	public class SgtProceduralScale_Editor : SgtProcedural_Editor<SgtProceduralScale>
	{
		protected override void OnInspector()
		{
			base.OnInspector();

			BeginError(Any(t => t.BaseScale == Vector3.zero));
				Draw("BaseScale", "The default scale of your object.");
			EndError();
			Draw("ScaleMultiplierMin", "The minimum multiplication of the BaseScale.");
			Draw("ScaleMultiplierMax", "The maximum multiplication of the BaseScale.");
		}
	}
}
#endif