using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceGraphicsToolkit
{
	/// <summary>This component allows you to generate a blurred SgtShadowRing.Texture based on a normal texture.</summary>
	[ExecuteInEditMode]
	[RequireComponent(typeof(SgtShadowRing))]
	[HelpURL(SgtHelper.HelpUrlPrefix + "SgtShadowRingFilter")]
	[AddComponentMenu(SgtHelper.ComponentMenuPrefix + "Shadow Ring Filter")]
	public class SgtShadowRingFilter : MonoBehaviour
	{
		/// <summary>The source ring texture that will be filtered.</summary>
		public Texture2D Source;

		/// <summary>The format of the generated texture.</summary>
		public TextureFormat Format = TextureFormat.ARGB32;

		/// <summary>The amount of blur iterations.</summary>
		public int Iterations = 1;

		/// <summary>Overwrite the RGB channels with the alpha?</summary>
		public bool ShareRGB;

		/// <summary>Invert the alpha channel?</summary>
		public bool Invert;

		[System.NonSerialized]
		private Texture2D generatedTexture;

		[System.NonSerialized]
		private SgtShadowRing cachedShadowRing;

		[System.NonSerialized]
		private bool cachedShadowRingSet;

		[System.NonSerialized]
		private static Color[] bufferA;

		[System.NonSerialized]
		private static Color[] bufferB;

		public Texture2D GeneratedTexture
		{
			get
			{
				return generatedTexture;
			}
		}

		public SgtShadowRing CachedShadowRing
		{
			get
			{
				if (cachedShadowRingSet == false)
				{
					cachedShadowRing    = GetComponent<SgtShadowRing>();
					cachedShadowRingSet = true;
				}

				return cachedShadowRing;
			}
		}

#if UNITY_EDITOR
		/// <summary>This method allows you to export the generated texture as an asset.
		/// Once done, you can remove this component, and set the <b>SgtShadowRing</b> component's <b>Texture</b> setting using the exported asset.</summary>
		[ContextMenu("Export Texture")]
		public void ExportTexture()
		{
			var importer = SgtHelper.ExportTextureDialog(generatedTexture, "RingShadow");

			if (importer != null)
			{
				importer.textureCompression  = TextureImporterCompression.Uncompressed;
				importer.alphaSource         = TextureImporterAlphaSource.FromInput;
				importer.wrapMode            = TextureWrapMode.Clamp;
				importer.filterMode          = FilterMode.Trilinear;
				importer.anisoLevel          = 16;
				importer.alphaIsTransparency = true;

				importer.SaveAndReimport();
			}
		}
#endif

		[ContextMenu("Update Texture")]
		public void UpdateTexture()
		{
			if (Source == null)
			{
				Source = CachedShadowRing.Texture as Texture2D;
			}

			if (Source != null)
			{
				var width = Source.width;
#if UNITY_EDITOR
				SgtHelper.MakeTextureReadable(Source);
#endif
				// Destroy if invalid
				if (generatedTexture != null)
				{
					if (generatedTexture.width != width || generatedTexture.height != 1 || generatedTexture.format != Format)
					{
						generatedTexture = SgtHelper.Destroy(generatedTexture);
					}
				}

				// Create?
				if (generatedTexture == null)
				{
					generatedTexture = SgtHelper.CreateTempTexture2D("Ring Shadow (Generated)", width, 1, Format);

					generatedTexture.wrapMode = TextureWrapMode.Clamp;

					ApplyTexture();
				}

				if (bufferA == null || bufferA.Length != width)
				{
					bufferA = new Color[width];
					bufferB = new Color[width];
				}

				for (var x = 0; x < width; x++)
				{
					bufferA[x] = bufferB[x] = Source.GetPixel(x, 0);
				}

				if (Invert == true)
				{
					for (var x = 0; x < width; x++)
					{
						var a = bufferA[x];

						bufferA[x] = bufferB[x] = new Color(1.0f - a.r, 1.0f - a.g, 1.0f - a.b, 1.0f - a.a);
					}
				}

				if (ShareRGB == true)
				{
					for (var x = 0; x < width; x++)
					{
						var a = bufferA[x].a;

						bufferA[x] = bufferB[x] = new Color(a, a, a, a);
					}
				}

				for (var i = 0 ; i < Iterations; i++)
				{
					SwapBuffers();

					for (var x = width - 2; x >= 1; x--)
					{
						WritePixel(x);
					}
				}

				for (var x = 0; x < width; x++)
				{
					generatedTexture.SetPixel(x, 0, bufferB[x]);
				}

				generatedTexture.SetPixel(        0, 0, Color.white);
				generatedTexture.SetPixel(width - 1, 0, Color.white);

				generatedTexture.Apply();
			}
		}

		[ContextMenu("Apply Texture")]
		public void ApplyTexture()
		{
			if (generatedTexture != null)
			{
				CachedShadowRing.Texture = generatedTexture;
			}
			else
			{
				CachedShadowRing.Texture = Source;
			}
		}

		protected virtual void OnEnable()
		{
			UpdateTexture();
			ApplyTexture();
		}

		protected virtual void OnDisable()
		{
			CachedShadowRing.Texture = Source;
		}

		protected virtual void OnDestroy()
		{
			SgtHelper.Destroy(generatedTexture);
		}

		private void WritePixel(int x)
		{
			var a = bufferA[x - 1];
			var b = bufferA[x    ];
			var c = bufferA[x + 1];

			bufferB[x] = (a + b + c) / 3.0f;
		}

		private void SwapBuffers()
		{
			var bufferT = bufferA;

			bufferA = bufferB;
			bufferB = bufferT;
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(SgtShadowRingFilter))]
	public class SgtShadowRingFilter_Editor : SgtEditor<SgtShadowRingFilter>
	{
		protected override void OnInspector()
		{
			var updateTexture = false;

			BeginError(Any(t => t.Source == null));
				DrawDefault("Source", ref updateTexture, "The source ring texture that will be filtered.");
			EndError();
			DrawDefault("Format", ref updateTexture, "The format of the generated texture.");

			Separator();

			BeginError(Any(t => t.Iterations <= 0));
				DrawDefault("Iterations", ref updateTexture, "The amount of blur iterations.");
			EndError();
			DrawDefault("ShareRGB", ref updateTexture, "Overwrite the RGB channels with the alpha?");
			DrawDefault("Invert", ref updateTexture, "Invert the alpha channel?");

			if (updateTexture == true) DirtyEach(t => t.UpdateTexture());
		}
	}
}
#endif