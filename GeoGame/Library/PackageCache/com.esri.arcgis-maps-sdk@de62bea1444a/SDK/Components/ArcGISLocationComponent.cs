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
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Geometry;
using Esri.HPFramework;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(HPTransform))]
	[AddComponentMenu("ArcGIS Maps SDK/ArcGIS Location")]
	public class ArcGISLocationComponent : MonoBehaviour
	{
		[SerializeField]
		private ArcGISPoint position = null;

		[SerializeField]
		private ArcGISRotation rotation;

		private ArcGISMapComponent arcGISMapComponent;
		private HPTransform hpTransform;
		private bool internalHasChanged = false;
		private double3 universePosition;
		private quaternion universeRotation;
		private IntPtr lastPointHandle;

		public ArcGISPoint Position
		{
			get => position;
			set
			{
				if (position == null || !position.IsValid || !position.Equals(value))
				{
					position = value;

					internalHasChanged = true;

					SyncPositionWithHPTransform();
				}
			}
		}

		public ArcGISRotation Rotation
		{
			get => rotation;
			set
			{
				if (!value.Equals(rotation))
				{
					rotation = value;

					internalHasChanged = true;

					SyncPositionWithHPTransform();
				}
			}
		}

		private void Awake()
		{
			if (position != null && position.IsValid)
			{
				// Ensure HPTransform is sync'd from geoPosition, rather than geoPosition being sync'd from HPTransform
				internalHasChanged = true;
			}
		}

		private void Initialize()
		{
			arcGISMapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();

			if (arcGISMapComponent == null)
			{
				Debug.LogError("Unable to find a parent ArcGISMapComponent.");

				enabled = false;
				return;
			}

			hpTransform = GetComponent<HPTransform>();

			arcGISMapComponent.MapTypeChanged += () =>
			{
				internalHasChanged = true;
				SyncPositionWithHPTransform();
			};

			// When SR changes recalculate from geographic position
			arcGISMapComponent.View.SpatialReferenceChanged += () =>
			{
				internalHasChanged = true;
			};
		}

		private void LateUpdate()
		{
			if (arcGISMapComponent == null)
			{
				return;
			}

			SyncPositionWithHPTransform();
		}

		private void OnEnable()
		{
			Initialize();
		}

		private void OnTransformParentChanged()
		{
			Initialize();
		}

		private void PullChangesFromHPTransform()
		{
			universePosition = hpTransform.UniversePosition;
			universeRotation = hpTransform.UniverseRotation;

			var cartesianPosition = universePosition;
			var cartesianRotation = universeRotation.ToQuaterniond();

			var newPosition = arcGISMapComponent.View.WorldToGeographic(cartesianPosition); // May result in NaN position

			if (position != null && position.IsValid)
			{
				// this try catch is necessary because the below mentioned example could try to project between 2 SR's that cannot be projected between
				try
				{
					// When creating a location component with a specific SR and then sliding it around or updating the HPTransform
					// this method can change the SR of the Location component which is strange behavior
					this.position = GeoUtils.ProjectToSpatialReference(newPosition, position.SpatialReference); // this is a no-op if the sr is already the same
				}
				catch
				{
					this.position = newPosition;
				}
			}
			else
			{
				this.position = newPosition;
			}

			this.rotation = GeoUtils.FromCartesianRotation(cartesianPosition, cartesianRotation, arcGISMapComponent.View.SpatialReference, arcGISMapComponent.View.Map.MapType);
		}

		private void PushChangesToHPTransform()
		{
			var cartesianPosition = arcGISMapComponent.View.GeographicToWorld(position);

			if (!cartesianPosition.IsValid())
			{
				// If the geographic position is not a valid cartesian position, ignore it
				PullChangesFromHPTransform(); // Reset position from current, assumed value, cartesian position

				return;
			}

			var cartesianRotation = GeoUtils.ToCartesianRotation(cartesianPosition, rotation, arcGISMapComponent.View.SpatialReference, arcGISMapComponent.View.Map.MapType);

			universePosition = cartesianPosition;
			universeRotation = cartesianRotation.ToQuaternion();

			hpTransform.UniversePosition = universePosition;
			hpTransform.UniverseRotation = universeRotation;
		}

		public static void SetPositionAndRotation(GameObject gameObject, ArcGISPoint geographicPosition, ArcGISRotation geographicRotation)
		{
			var locationComponent = gameObject.GetComponent<ArcGISLocationComponent>();

			if (locationComponent)
			{
				locationComponent.Position = geographicPosition;
				locationComponent.Rotation = geographicRotation;

				return;
			}

			var hpTransform = gameObject.GetComponent<HPTransform>();

			if (!hpTransform)
			{
				throw new System.InvalidOperationException(gameObject.name + " requires an HPTransform");
			}

			var arcGISMapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();

			if (!arcGISMapComponent)
			{
				throw new System.InvalidOperationException(gameObject.name + " should a child of a ArcGISMapComponent");
			}

			var spatialReference = arcGISMapComponent.View.SpatialReference;

			if (spatialReference == null)
			{
				throw new System.InvalidOperationException("View must have a spatial reference");
			}

			var cartesianPosition = arcGISMapComponent.View.GeographicToWorld((ArcGISPoint)geographicPosition);

			hpTransform.UniversePosition = cartesianPosition;
			hpTransform.UniverseRotation = GeoUtils.ToCartesianRotation(cartesianPosition, geographicRotation, spatialReference, arcGISMapComponent.MapType).ToQuaternion();
		}

		public void SyncPositionWithHPTransform()
		{
			if (arcGISMapComponent.View.SpatialReference == null)
			{
				// Defer until we have a spatial reference
				return;
			}

			if (internalHasChanged && position != null && position.IsValid)
			{
				PushChangesToHPTransform();
			}
			else if (position == null || !position.IsValid || !universePosition.Equals(hpTransform.UniversePosition) || !universeRotation.Equals(hpTransform.UniverseRotation))
			{
				PullChangesFromHPTransform();
			}

			internalHasChanged = false;
		}
	}
}
