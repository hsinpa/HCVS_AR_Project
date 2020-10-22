using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceGraphicsToolkit
{
	/// <summary>This component handles rendering of a quads model.</summary>
	[ExecuteInEditMode]
	[AddComponentMenu("")]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	public class SgtQuadsModel : MonoBehaviour
	{
		public class CameraState : SgtCameraState
		{
			public Vector3 LocalPosition;
		}

		/// <summary>The quads this belongs to. If this is null then this GameObject will automatically be destroyed.</summary>
		public SgtQuads Quads;

		[System.NonSerialized]
		private MeshFilter meshFilter;

		[System.NonSerialized]
		private MeshRenderer meshRenderer;

		[System.NonSerialized]
		private Mesh mesh;

		[System.NonSerialized]
		private Material material;

		[System.NonSerialized]
		private List<CameraState> cameraStates;

		public Mesh Mesh
		{
			get
			{
				return mesh;
			}
		}

		public void PoolMeshNow()
		{
			if (mesh != null)
			{
				if (meshFilter == null) meshFilter = gameObject.GetComponent<MeshFilter>();

				mesh.Clear(false);

				mesh = meshFilter.sharedMesh = SgtObjectPool<Mesh>.Add(mesh);
			}
		}

		public void SetMesh(Mesh newMesh)
		{
			if (newMesh != mesh)
			{
				if (meshFilter == null) meshFilter = gameObject.GetComponent<MeshFilter>();

				mesh = meshFilter.sharedMesh = newMesh;
			}
		}

		public void SetMaterial(Material newMaterial)
		{
			if (newMaterial != material)
			{
				if (meshRenderer == null) meshRenderer = gameObject.GetComponent<MeshRenderer>();

				material = meshRenderer.sharedMaterial = newMaterial;
			}
		}

		public static SgtQuadsModel Create(SgtQuads quads)
		{
			var model = SgtComponentPool<SgtQuadsModel>.Pop(quads.transform, "Quads Model", quads.gameObject.layer);

			model.Quads = quads;

			return model;
		}

		public static void Pool(SgtQuadsModel model)
		{
			if (model != null)
			{
				model.Quads = null;

				model.PoolMeshNow();

				SgtComponentPool<SgtQuadsModel>.Add(model);
			}
		}

		public static void MarkForDestruction(SgtQuadsModel model)
		{
			if (model != null)
			{
				model.Quads = null;

				model.PoolMeshNow();

				model.gameObject.SetActive(true);
			}
		}

		public void Save(Camera camera)
		{
			var cameraState = SgtCameraState.Find(ref cameraStates, camera);

			cameraState.LocalPosition = transform.localPosition;
		}

		public void Restore(Camera camera)
		{
			var cameraState = SgtCameraState.Restore(cameraStates, camera);

			if (cameraState != null)
			{
				transform.localPosition = cameraState.LocalPosition;
			}
		}

		public void Revert()
		{
			transform.localPosition = Vector3.zero;
		}

		protected virtual void OnDestroy()
		{
			PoolMeshNow();
		}

		protected virtual void Update()
		{
			if (Quads == null)
			{
				Pool(this);
			}
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SgtQuadsModel))]
	public class SgtQuadsModel_Editor : SgtEditor<SgtQuadsModel>
	{
		protected override void OnInspector()
		{
			BeginDisabled();
				Draw("Quads", "The quads this belongs to. If this is null then this GameObject will automatically be destroyed.");
			EndDisabled();
		}
	}
}
#endif