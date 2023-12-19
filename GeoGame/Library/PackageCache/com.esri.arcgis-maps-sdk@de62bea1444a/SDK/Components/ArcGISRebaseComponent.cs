// COPYRIGHT 1995-2022 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Attn: Contracts and Legal Department
// Environmental Systems Research Institute, Inc.
// 380 New York Street
// Redlands, California 92373
// USA
//
// email: legal@esri.com
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(HPTransform))]
	[AddComponentMenu("ArcGIS Maps SDK/ArcGIS Rebase")]
	public class ArcGISRebaseComponent : MonoBehaviour
	{
		private ArcGISMapComponent arcGISMapComponent;
		private HPTransform hpTransform;
		private double3 lastPosition;

		void OnEnable()
		{
			hpTransform = GetComponent<HPTransform>();

			UpdateMapComponent();

#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				enabled = false;
			}
#endif
		}

		void Start()
		{
			if (arcGISMapComponent == null)
			{
				Debug.LogError("An ArcGISMapComponent could not be found. Please make sure this GameObject is a child of a GameObject with an ArcGISMapComponent attached");

				enabled = false;
				return;
			}
		}

		private void LateUpdate()
		{
			if (arcGISMapComponent == null)
			{
				return;
			}

			if (arcGISMapComponent.View.SpatialReference == null)
			{
				// Defer until we have a spatial reference
				return;
			}

			if (!lastPosition.Equals(hpTransform.UniversePosition))
			{
				lastPosition = hpTransform.UniversePosition;

				// Avoid conversion to geographic coordinates
				arcGISMapComponent.UniversePosition = lastPosition;
				arcGISMapComponent.SyncPositionWithHPRoot();
			}
		}

		void OnTransformParentChanged()
		{
			UpdateMapComponent();
		}

		private void UpdateMapComponent()
		{
			arcGISMapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();
		}
	}
}
