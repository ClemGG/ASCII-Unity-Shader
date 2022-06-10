///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Ascii - Image Effect.
// Copyright (c) Digital Software/Johan Munkestam. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.U2D;

namespace AsciiImageEffect
{
    /// <summary>
    /// Ascii - Image Effect.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/AsciiZarkow")]
	public sealed class AsciiZarkow : MonoBehaviour
	{
		public Vector2 charSize = new Vector2(9f, 10f);
		[Range(0f, 1f)] public float charColTransp = 1f;
		[Range(0f, 1f)] public float bgColTransp = 0f;
		[Range(0f, 0.25f)] public float fogDensity = .08f;
		public Color fogColor = Color.black;

		public Sprite Bracket;
		public Sprite And;
		public Sprite Dollar;
		public Sprite R;
		public Sprite P;
		public Sprite Asterix;
		public Sprite Plus;
		public Sprite Tilde;
		public Sprite Minus;
		public Sprite Dot;

		private Shader shader;
		
		private Material material;

		private Texture BracketSamplerTexture;
		private Texture AndSamplerTexture;
		private Texture DollarSamplerTexture;
		private Texture RSamplerTexture;
		private Texture PSamplerTexture;
		private Texture AsterixSamplerTexture;
		private Texture PlusSamplerTexture;
		private Texture TildeSamplerTexture;
		private Texture MinusSamplerTexture;
		private Texture DotSamplerTexture;

#if UNITY_EDITOR

		private void OnValidate()
		{
			//Pour éviter que la prefab ne change les paramètres à chaque fois
			if (!PrefabModeIsActive(gameObject))
			{

				RenderSettings.fogColor = fogColor;
				RenderSettings.fogDensity = fogDensity;

                Awake();
                OnEnable();
            }
        }

        bool PrefabModeIsActive(GameObject gameobject)
		{
			PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
			bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(gameObject) == PrefabInstanceStatus.Connected;
			return isValidPrefabStage || !prefabConnected;
		}

#endif


		private void Awake()
		{
            shader = Resources.Load<Shader>(@"Shaders/AsciiZarkow");
            if (shader == null) {
				Debug.LogError (@"Ascii shader not found.");
				
				this.enabled = false;
			}
		}

		/// <summary>
		/// Check.
		/// </summary>
		private void OnEnable()
		{
			if (shader == null)
			{
				Debug.LogError(string.Format("'{0}' shader null.", this.GetType().ToString()));
				
				this.enabled = false;
			}
			else
			{
				CreateMaterial();
				
				if (material == null)
					this.enabled = false;
			}
		}
		
		/// <summary>
		/// Destroy the material.
		/// </summary>
		private void OnDisable()
		{
			if (material != null)
				DestroyImmediate(material);
		}

		/// <summary>
		/// Creates the material.
		/// </summary>
		private void CreateMaterial()
		{
			if (shader != null)
			{
				if (material != null)
				{
					if (Application.isEditor == true)
					{
						DestroyImmediate(material);
						DestroyImmediate(BracketSamplerTexture);
						DestroyImmediate(AndSamplerTexture);
						DestroyImmediate(DollarSamplerTexture);
						DestroyImmediate(RSamplerTexture);
						DestroyImmediate(PSamplerTexture);
						DestroyImmediate(AsterixSamplerTexture);
						DestroyImmediate(PlusSamplerTexture);
						DestroyImmediate(TildeSamplerTexture);
						DestroyImmediate(MinusSamplerTexture);
						DestroyImmediate(DotSamplerTexture);
					}
                    else
                    {
						Destroy(material);
						Destroy(BracketSamplerTexture);
						Destroy(AndSamplerTexture);
						Destroy(DollarSamplerTexture);
						Destroy(RSamplerTexture);
						Destroy(PSamplerTexture);
						Destroy(AsterixSamplerTexture);
						Destroy(PlusSamplerTexture);
						Destroy(TildeSamplerTexture);
						Destroy(MinusSamplerTexture);
						Destroy(DotSamplerTexture);
					}
				}
				
				material = new Material(shader);
				if (material == null)
				{
					Debug.LogWarning(string.Format("'{0}' material null.", this.name));
					return;
				}

                {
                    BracketSamplerTexture = LoadTexture(Bracket);
                    AndSamplerTexture = LoadTexture(And);
                    DollarSamplerTexture = LoadTexture(Dollar);
                    RSamplerTexture = LoadTexture(R);
                    PSamplerTexture = LoadTexture(P);
                    AsterixSamplerTexture = LoadTexture(Asterix);
                    PlusSamplerTexture = LoadTexture(Plus);
                    TildeSamplerTexture = LoadTexture(Minus);
                    MinusSamplerTexture = LoadTexture(Tilde);
                    DotSamplerTexture = LoadTexture(Dot);

                }

			}
		}

		private Texture LoadTexture(Sprite sprite)
		{

			Texture2D tex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
													(int)sprite.textureRect.y,
													(int)sprite.textureRect.width,
													(int)sprite.textureRect.height);
			tex.SetPixels(pixels);
			tex.Apply();

			//if (tex == null)
			//{
			//	Debug.LogError(string.Format("Texture '{0}' not found!", "Textures/" + sprite.name));

			//	return null;
			//}
			//Debug.Log("Loaded " + sprite.name);

			// safety, if forgotten when we added them
			tex.wrapMode = TextureWrapMode.Repeat;
			tex.filterMode = FilterMode.Point;
			tex.hideFlags = HideFlags.HideAndDontSave;

			return tex;
		}




		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (material != null)
			{
				material.SetFloat(@"charColTransp", charColTransp);
				material.SetFloat(@"bgColTransp", bgColTransp);
				material.SetFloat(@"monitorWidthMultiplier", Screen.width / charSize.x);
				material.SetFloat(@"monitorHeightMultiplier", Screen.height / charSize.y);

				//print(Screen.width / charSize.x + " ; " + Screen.height / charSize.y);

				material.SetTexture(@"BracketSampler", BracketSamplerTexture);
				material.SetTexture(@"AndSampler", AndSamplerTexture);
				material.SetTexture(@"DollarSampler", DollarSamplerTexture);
				material.SetTexture(@"RSampler", RSamplerTexture);
				material.SetTexture(@"PSampler", PSamplerTexture);
				material.SetTexture(@"AsterixSampler", AsterixSamplerTexture);
				material.SetTexture(@"PlusSampler", PlusSamplerTexture);
				material.SetTexture(@"TildeSampler", TildeSamplerTexture);
				material.SetTexture(@"MinusSampler", MinusSamplerTexture);
				material.SetTexture(@"DotSampler", DotSamplerTexture);
				Graphics.Blit(source, destination, material, 0);
			}
		}
	}
}
