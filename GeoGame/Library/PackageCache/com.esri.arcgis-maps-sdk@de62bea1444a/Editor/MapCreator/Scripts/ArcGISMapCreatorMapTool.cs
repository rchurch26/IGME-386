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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Map;
using System;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorMapTool : MonoBehaviour
	{
		private static ArcGISMapComponent mapComponent;

		private static Toggle extentToggle;

		private static Button globalButton;
		private static Button localButton;
		private static Button createMapButton;

		private static DoubleField originXPositionField;
		private static DoubleField originYPositionField;
		private static DoubleField originZPositionField;
		private static IntegerField originSRPositionField;

		private static DoubleField extentXPositionField;
		private static DoubleField extentYPositionField;
		private static IntegerField extentSRPositionField;
		private static DoubleField extentShapeXField;
		private static DoubleField extentShapeYField;
		private static EnumField shapeField;
		private static Foldout mapExtentFoldout;
		private static VisualElement extentFields;

		public static VisualElement CreateMapTool()
		{
			VisualElement rootElement = new VisualElement();
			rootElement.name = "ArcGISMapCreatorMapTool";

			var templatePath = MapCreatorUtilities.FindAssetPath("MapToolTemplate");
			var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(templatePath);
			template.CloneTree(rootElement);

			var styleSheet = MapCreatorUtilities.FindAssetPath("MapToolStyle");
			rootElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheet));

			mapComponent = GameObject.FindObjectOfType<ArcGISMapComponent>();

			InitMapExtentCategory(rootElement);
			PopulateExtentFields(rootElement);
			InitShapeChangeEvent(rootElement);
			InitMapTypeToggle(rootElement);
			InitOriginCategory(rootElement);
			InitCreateMapButton(rootElement);

			return rootElement;
		}

		private static void InitMapTypeToggle(VisualElement rootElement)
		{
			globalButton = rootElement.Query<Button>(name: "select-global-button");
			globalButton.clickable.activators.Clear();
			globalButton.RegisterCallback<MouseDownEvent>(evnt =>
			{
				ToggleMapTypeButtons(false);
				mapExtentFoldout.style.display = DisplayStyle.None;

				if (mapComponent != null)
				{
					mapComponent.MapType = ArcGISMapType.Global;
				}
			});

			localButton = rootElement.Query<Button>(name: "select-local-button");
			localButton.clickable.activators.Clear();
			localButton.RegisterCallback<MouseDownEvent>(evnt =>
			{
				ToggleMapTypeButtons(true);
				mapExtentFoldout.style.display = DisplayStyle.Flex;

				if (mapComponent != null)
				{
					mapComponent.MapType = ArcGISMapType.Local;
					extentToggle.value = mapComponent.EnableExtent;
					SwitchVisibleExtentShapeFields(mapComponent.Extent.ExtentShape);
				}
			});

			if (mapComponent != null && mapComponent.MapType == ArcGISMapType.Local)
			{
				ToggleMapTypeButtons(true);
			}
			else
			{
				ToggleMapTypeButtons(false);
			}
		}

		private static void ToggleMapTypeButtons(bool isGlobal)
		{
			globalButton.SetEnabled(isGlobal);
			localButton.SetEnabled(!isGlobal);
		}

		private static void InitOriginCategory(VisualElement rootElement)
		{
			Action<int> intFieldValueChangedCallback = (int value) =>
			{
				try
				{

					var spatialRef = new ArcGISSpatialReference(value);

					if (mapComponent != null)
					{
						mapComponent.OriginPosition = new ArcGISPoint(mapComponent.OriginPosition.X, mapComponent.OriginPosition.Y, mapComponent.OriginPosition.Z, spatialRef);
					}

					MapCreatorUtilities.UpdateArcGISPointLabels(originXPositionField, originYPositionField, originZPositionField, originSRPositionField.value);
				}
				catch
				{
				}
			};

			Action<double> XOriginChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					mapComponent.OriginPosition = new ArcGISPoint(value, mapComponent.OriginPosition.Y, mapComponent.OriginPosition.Z, mapComponent.OriginPosition.SpatialReference);
				}
			};

			Action<double> YOriginChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					mapComponent.OriginPosition = new ArcGISPoint(mapComponent.OriginPosition.X, value, mapComponent.OriginPosition.Z, mapComponent.OriginPosition.SpatialReference);
				}
			};

			Action<double> ZOriginChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					mapComponent.OriginPosition = new ArcGISPoint(mapComponent.OriginPosition.X, mapComponent.OriginPosition.Y, value, mapComponent.OriginPosition.SpatialReference);
				}
			};

			originXPositionField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-origin-x", null, XOriginChanged);
			originYPositionField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-origin-y", null, YOriginChanged);
			originZPositionField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-origin-z", null, ZOriginChanged);
			originSRPositionField = MapCreatorUtilities.InitializeIntegerField(rootElement, "map-origin-wkid", null, intFieldValueChangedCallback);

			if (mapComponent != null)
			{
				var serializedMapComponent = new SerializedObject(mapComponent);

				originXPositionField.value = mapComponent.OriginPosition.X;
				originYPositionField.value = mapComponent.OriginPosition.Y;
				originZPositionField.value = mapComponent.OriginPosition.Z;
				originSRPositionField.value = mapComponent.OriginPosition.SpatialReference.WKID;
			}
			else
			{
				originSRPositionField.value = SpatialReferenceWkid.WGS84;
			}

			MapCreatorUtilities.UpdateArcGISPointLabels(originXPositionField, originYPositionField, originZPositionField, originSRPositionField.value);
		}

		private static void InitMapExtentCategory(VisualElement rootElement)
		{
			mapExtentFoldout = rootElement.Query<Foldout>(name: "category-map-extent");
			extentFields = rootElement.Query<VisualElement>(name: "map-extent-fields");

			extentToggle = rootElement.Query<Toggle>(name: "toggle-enable-map-extent");
			extentToggle.RegisterValueChangedCallback(evnt =>
			{
				extentFields.SetEnabled(evnt.newValue);

				if (mapComponent != null)
				{
					mapComponent.EnableExtent = evnt.newValue;
				}
			});

			shapeField = rootElement.Query<EnumField>(name: "map-shape-selector");
			shapeField.RegisterValueChangedCallback(evnt =>
			{
				if (mapComponent != null)
				{
					ArcGISExtentInstanceData newExtent = mapComponent.Extent;
					newExtent.ExtentShape = (MapExtentShapes)evnt.newValue;
					mapComponent.Extent = newExtent;
				}
			});

			if (mapComponent == null || mapComponent.MapType == ArcGISMapType.Global)
			{
				mapExtentFoldout.style.display = DisplayStyle.None;
				extentFields.SetEnabled(false);
				return;
			}

			extentToggle.value = mapComponent.EnableExtent;
			extentFields.SetEnabled(extentToggle.value);
			shapeField.value = mapComponent.Extent.ExtentShape;
		}

		private static void PopulateExtentFields(VisualElement rootElement)
		{
			Action<int> intFieldValueChangedCallback = (int value) =>
			{
				try
				{

					var spatialRef = new ArcGISSpatialReference(value);

					if (mapComponent != null)
					{
						var extent = mapComponent.Extent;
						extent.GeographicCenter = new ArcGISPoint(extent.GeographicCenter.X, extent.GeographicCenter.Y, extent.GeographicCenter.Z, spatialRef);
						mapComponent.Extent = extent;
					}

					MapCreatorUtilities.UpdateArcGISPointLabels(extentXPositionField, extentYPositionField, null, extentSRPositionField.value);
				}
				catch
				{
				}
			};

			Action<double> XExtentChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					var extent = mapComponent.Extent;
					extent.GeographicCenter = new ArcGISPoint(value, extent.GeographicCenter.Y, extent.GeographicCenter.Z, extent.GeographicCenter.SpatialReference);
					mapComponent.Extent = extent;
				}
			};

			Action<double> YExtentChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					var extent = mapComponent.Extent;
					extent.GeographicCenter = new ArcGISPoint(extent.GeographicCenter.X, value, extent.GeographicCenter.Z, extent.GeographicCenter.SpatialReference);
					mapComponent.Extent = extent;
				}
			};

			Action<double> XShapeChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					var extent = mapComponent.Extent;
					extent.ShapeDimensions.x = value;
					mapComponent.Extent = extent;
				}
			};

			Action<double> YShapeChanged = (double value) =>
			{
				if (mapComponent != null)
				{
					var extent = mapComponent.Extent;
					extent.ShapeDimensions.y = value;
					mapComponent.Extent = extent;
				}
			};

			extentXPositionField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-extent-x", null, XExtentChanged);
			extentYPositionField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-extent-y", null, YExtentChanged);
			extentSRPositionField = MapCreatorUtilities.InitializeIntegerField(rootElement, "map-extent-wkid", null, intFieldValueChangedCallback);

			extentShapeXField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-shape-dimensions-x", null, XShapeChanged);
			extentShapeYField = MapCreatorUtilities.InitializeDoubleField(rootElement, "map-shape-dimensions-y", null, YShapeChanged);

			if (mapComponent != null)
			{
				var serializedMapComponent = new SerializedObject(mapComponent);

				extentXPositionField.value = mapComponent.Extent.GeographicCenter.X;
				extentYPositionField.value = mapComponent.Extent.GeographicCenter.Y;
				extentSRPositionField.value = mapComponent.Extent.GeographicCenter.SpatialReference.WKID;

				extentShapeXField.value = mapComponent.Extent.ShapeDimensions.x;
				extentShapeYField.value = mapComponent.Extent.ShapeDimensions.y;
			}
			else
			{
				extentSRPositionField.value = SpatialReferenceWkid.WGS84;
			}

			MapCreatorUtilities.UpdateArcGISPointLabels(extentXPositionField, extentYPositionField, null, extentSRPositionField.value);
		}

		private static void InitShapeChangeEvent(VisualElement rootElement)
		{
			shapeField.RegisterValueChangedCallback(evnt =>
			{
				var shape = (MapExtentShapes)evnt.newValue;
				SwitchVisibleExtentShapeFields(shape);
			});

			SwitchVisibleExtentShapeFields((MapExtentShapes)shapeField.value);
		}

		private static void SwitchVisibleExtentShapeFields(MapExtentShapes shape)
		{
			switch (shape)
			{
				case MapExtentShapes.Square:
					extentShapeXField.label = "Length";
					extentShapeYField.style.display = DisplayStyle.None;
					break;
				case MapExtentShapes.Rectangle:
					extentShapeXField.label = "X";
					extentShapeYField.style.display = DisplayStyle.Flex;
					break;
				case MapExtentShapes.Circle:
					extentShapeXField.label = "Radius";
					extentShapeYField.style.display = DisplayStyle.None;
					break;
				default:
					break;
			}
			
			shapeField.value = shape;
		}

		private static void InitCreateMapButton(VisualElement rootElement)
		{
			createMapButton = rootElement.Query<Button>(name: "button-create-map");
			createMapButton.clickable.activators.Clear();
			createMapButton.RegisterCallback<MouseDownEvent>(evnt => CreateMap(evnt, rootElement));

			if (mapComponent != null)
			{
				createMapButton.SetEnabled(false);
			}
		}

		private static void CreateMap(MouseDownEvent evnt, VisualElement rootElement)
		{
			var gameObject = new GameObject("ArcGISMap");
			mapComponent = gameObject.AddComponent<ArcGISMapComponent>();
			GameObjectUtility.EnsureUniqueNameForSibling(gameObject);
			Undo.RegisterCreatedObjectUndo(gameObject, "Create " + gameObject.name);
			var button = (Button)evnt.currentTarget;
			button.SetEnabled(false);

			mapComponent.EnableExtent = extentToggle.value;
			mapComponent.MapType = globalButton.enabledSelf == false ? ArcGISMapType.Global : ArcGISMapType.Local;

			mapComponent.OriginPosition = new ArcGISPoint(originXPositionField.value, originYPositionField.value, originZPositionField.value, new ArcGISSpatialReference(originSRPositionField.value));

			if (mapComponent.MapType == ArcGISMapType.Local && extentSRPositionField.value != 0)
			{
				mapComponent.Extent = new ArcGISExtentInstanceData()
				{
					GeographicCenter = new ArcGISPoint(extentXPositionField.value, extentYPositionField.value, 0, new ArcGISSpatialReference(extentSRPositionField.value)),
					ExtentShape = (MapExtentShapes)shapeField.value,
					ShapeDimensions = new double2(extentShapeXField.value, extentShapeYField.value)
				};
			}

			mapComponent.APIKey = ArcGISMapCreatorAuthTool.APIKey;
			mapComponent.Basemap = ArcGISMapCreatorBasemapTool.GetDefaultBasemap();
			mapComponent.BasemapType = BasemapTypes.ImageLayer;
			mapComponent.Elevation = ArcGISMapCreatorElevationTool.GetDefaultElevation();

			Selection.activeGameObject = gameObject;
		}
	}
}
