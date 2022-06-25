///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Ascii - Image Effect.
// Copyright (c) Digital Software/Johan Munkestam. All rights reserved.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//From ASCIIZarkov's ASCII Shader:
//http://www.digitalsoftware.se/community/thread-19.html


using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Project.Shaders.AsciiImageEffect
{
    /// <summary>
    /// Ascii - Image Effect.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Ascii Camera Effect")]
	public sealed class AsciiZarkow : MonoBehaviour
	{

		#region Public Properties

		[field: SerializeField]
		private Shader Shader { get; set; }

		[field: SerializeField] 
		private Vector2 CharSize { get; set; } = new Vector2(12f, 12f);
		[field: SerializeField, Range(0f, 1f)]
		private float CharColTransp { get; set; } = 1f;
		[field: SerializeField, Range(0f, 1f)]
		private float BgColTransp { get; set; } = 0f;
		[field: SerializeField]
		private bool Fog { get; set; } = false;
		[field: SerializeField, Range(0f, 0.25f)]
		private float FogDensity { get; set; } = .08f;
		[field: SerializeField]
		private Color FogColor { get; set; } = Color.black;


		//Light0 : the brightest ; Light9 : the darkest
		[field: SerializeField] 
		private Sprite Light0 { get; set; }
		[field: SerializeField] 
		private Sprite Light1 { get; set; }
		[field: SerializeField] 
		private Sprite Light2 { get; set; }
		[field: SerializeField] 
		private Sprite Light3 { get; set; }
		[field: SerializeField] 
		private Sprite Light4 { get; set; }
		[field: SerializeField] 
		private Sprite Light5 { get; set; }
		[field: SerializeField] 
		private Sprite Light6 { get; set; }
		[field: SerializeField] 
		private Sprite Light7 { get; set; }
		[field: SerializeField] 
		private Sprite Light8 { get; set; }
		[field: SerializeField] 
		private Sprite Light9 { get; set; }

        #endregion

        #region Private Properties

		private Material _materialToUse;
		private Material _materialToEdit;
		private Camera _camera;

		//Passed to the material
		private Texture _light0SamplerTex;
		private Texture _light1SamplerTex;
		private Texture _light2SamplerTex;
		private Texture _light3SamplerTex;
		private Texture _light4SamplerTex;
		private Texture _light5SamplerTex;
		private Texture _light6SamplerTex;
		private Texture _light7SamplerTex;
		private Texture _light8SamplerTex;
		private Texture _light9SamplerTex;

        #endregion

#if UNITY_EDITOR

        private void OnValidate()
		{
			//Pour éviter que la prefab ne change les paramètres au lieu de l'instance
			if (!PrefabModeIsActive(gameObject))
			{
				RenderSettings.fog = Fog;
				RenderSettings.fogColor = FogColor;
				RenderSettings.fogDensity = FogDensity;

				if (enabled && gameObject.activeInHierarchy)
				{
					Awake();
				}
			}
        }

        bool PrefabModeIsActive(GameObject gameobject)
		{
			bool isObjInPrefabMode = PrefabStageUtility.GetPrefabStage(gameobject) != null;
			bool isPrefabModeActive = PrefabStageUtility.GetCurrentPrefabStage() != null;
			return isObjInPrefabMode || isPrefabModeActive;
			//PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			//bool isValidPrefabStage = prefabStage != null && prefabStage.stageHandle.IsValid();
			//bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(gameObject) == PrefabInstanceStatus.Connected;
			//return isValidPrefabStage || !prefabConnected;
		}

#endif


		private void Awake()
		{
			_camera = GetComponent<Camera>();

            if (!Shader) {
				Debug.LogError ("Ascii shader not found.");
				
				this.enabled = false;
			}


			if (!_materialToEdit)
            {
				_materialToEdit = new Material(Shader);
			}

			EditMaterial();
		}

		/// <summary>
		/// Check.
		/// </summary>
		private void OnEnable()
		{
			_materialToUse = _materialToEdit;
		}
		
		/// <summary>
		/// Destroy the material.
		/// </summary>
		private void OnDisable()
		{
			_materialToUse = null;
			DestroyTextures();
		}

        /// <summary>
        /// Creates the material.
        /// </summary>
        private void EditMaterial()
		{
			DestroyTextures();

			_light0SamplerTex = LoadTexture(Light0);
			_light1SamplerTex = LoadTexture(Light1);
			_light2SamplerTex = LoadTexture(Light2);
			_light3SamplerTex = LoadTexture(Light3);
			_light4SamplerTex = LoadTexture(Light4);
			_light5SamplerTex = LoadTexture(Light5);
			_light6SamplerTex = LoadTexture(Light6);
			_light7SamplerTex = LoadTexture(Light8);
			_light8SamplerTex = LoadTexture(Light7);
			_light9SamplerTex = LoadTexture(Light9);


			SetTexturesOnMaterial();
			SetCharRenderSettingsOnMaterial();
			_materialToUse = _materialToEdit;
		}

        private Texture LoadTexture(Sprite sprite)
		{

			Texture2D tex = new((int)sprite.rect.width, (int)sprite.rect.height);
			Color[] pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
													(int)sprite.textureRect.y,
													(int)sprite.textureRect.width,
													(int)sprite.textureRect.height);
			tex.SetPixels(pixels);
			tex.Apply();

            if (tex == null)
            {
                Debug.LogError(string.Format("Texture '{0}' not found!", sprite.name));

                return null;
            }
			//Debug.Log($"Loaded '{sprite.name}'", gameObject);

            // safety, if forgotten when we added them
            tex.wrapMode = TextureWrapMode.Repeat;
			tex.filterMode = FilterMode.Point;
			tex.hideFlags = HideFlags.HideAndDontSave;

			return tex;
		}



		private void DestroyTextures()
		{
			DestroyImmediate(_light0SamplerTex);
			DestroyImmediate(_light1SamplerTex);
			DestroyImmediate(_light2SamplerTex);
			DestroyImmediate(_light3SamplerTex);
			DestroyImmediate(_light4SamplerTex);
			DestroyImmediate(_light5SamplerTex);
			DestroyImmediate(_light6SamplerTex);
			DestroyImmediate(_light7SamplerTex);
			DestroyImmediate(_light8SamplerTex);
			DestroyImmediate(_light9SamplerTex);
		}



		private void SetCharRenderSettingsOnMaterial()
		{
			_materialToEdit.SetFloat(@"charColTransp", CharColTransp);
			_materialToEdit.SetFloat(@"bgColTransp", BgColTransp);

			//We use Mathf.Abs in case we use split screen cameras
			_materialToEdit.SetFloat(@"monitorWidthMultiplier", Screen.width / CharSize.x);
			_materialToEdit.SetFloat(@"monitorHeightMultiplier", Screen.height / CharSize.y);

            //print(Screen.width + " ; " + Screen.height);
        }


		private void SetTexturesOnMaterial()
		{
			_materialToEdit.SetTexture(@"Light0Sampler", _light0SamplerTex);
			_materialToEdit.SetTexture(@"Light1Sampler", _light1SamplerTex);
			_materialToEdit.SetTexture(@"Light2Sampler", _light2SamplerTex);
			_materialToEdit.SetTexture(@"Light3Sampler", _light3SamplerTex);
			_materialToEdit.SetTexture(@"Light4Sampler", _light4SamplerTex);
			_materialToEdit.SetTexture(@"Light5Sampler", _light5SamplerTex);
			_materialToEdit.SetTexture(@"Light6Sampler", _light6SamplerTex);
			_materialToEdit.SetTexture(@"Light7Sampler", _light7SamplerTex);
			_materialToEdit.SetTexture(@"Light8Sampler", _light8SamplerTex);
			_materialToEdit.SetTexture(@"Light9Sampler", _light9SamplerTex);
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (_materialToUse)
			{
                if (!Application.isPlaying)
                {
					SetCharRenderSettingsOnMaterial();
					SetTexturesOnMaterial();
                }

				Graphics.Blit(source, destination, _materialToUse, 0);
			}
		}
	}
}
