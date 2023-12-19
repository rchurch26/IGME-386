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
using Esri.ArcGISMapsSDK.Renderer.GPUResources;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.View;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUComputing
{
	internal class NormalMapGeneratorPS : INormalMapGenerator
	{
		private readonly Material globalViewModeMaterial = null;
		private readonly Material localViewModeMaterial = null;
		private readonly ArcGISView view = null;

		public NormalMapGeneratorPS(ArcGISView view)
		{
			globalViewModeMaterial = new Material(Resources.Load<Shader>("Shaders/Utils/PS/GlobalViewModeComputeNormals"));
			localViewModeMaterial = new Material(Resources.Load<Shader>("Shaders/Utils/PS/LocalViewModeComputeNormals"));
			this.view = view;
		}

		public void Compute(IGPUResourceTexture2D inputElevation, Vector4 tileExtent, Vector4 textureExtent, IGPUResourceRenderTexture output)
		{
			var sr = view.SpatialReference;
			if (sr.IsGeographic)
			{
				double globeRadius = sr.SpheroidData.MajorSemiAxis;
				double circleLongitude = 2.0 * System.Math.PI * globeRadius;
				double minLatitude = tileExtent.y * MathUtils.DegreesToRadians;
				double latitudeAngleDelta = tileExtent.w / output.Height * MathUtils.DegreesToRadians;
				double longitudeArc = System.Math.Abs(tileExtent.z) / 360.0;

				ArcGISGeodeticDistanceResult result = ArcGISGeometryEngine.DistanceGeodetic(
					new Esri.GameEngine.Geometry.ArcGISPoint(tileExtent.x, tileExtent.y, sr),
					new Esri.GameEngine.Geometry.ArcGISPoint(tileExtent.x, tileExtent.y + tileExtent.w, sr),
					null,
					null,
					ArcGISGeodeticCurveType.Geodesic);

				// Assumption - this is always positive, since it's named distance
				double latitudeLength = result.Distance;
				globalViewModeMaterial.SetTexture("Input", inputElevation.NativeTexture);
				globalViewModeMaterial.SetFloat("MinLatitude", (float)minLatitude);
				globalViewModeMaterial.SetFloat("LatitudeAngleDelta", (float)latitudeAngleDelta);
				globalViewModeMaterial.SetFloat("LongitudeArc", (float)longitudeArc);
				globalViewModeMaterial.SetFloat("LatitudeLength", (float)latitudeLength);
				globalViewModeMaterial.SetFloat("CircleLongitude", (float)circleLongitude);
				globalViewModeMaterial.SetFloat("EarthRadius", (float)globeRadius);
				globalViewModeMaterial.SetVector("InputOffsetAndScale", new Vector4(textureExtent.x, textureExtent.y, textureExtent.z, textureExtent.w));

				UnityEngine.Graphics.Blit(null, ((GPUResourceRenderTexture)output).NativeRenderTexture, globalViewModeMaterial);
			}
			else
			{
				localViewModeMaterial.SetTexture("Input", inputElevation.NativeTexture);
				localViewModeMaterial.SetFloat("LatitudeLength", tileExtent.w);
				localViewModeMaterial.SetFloat("LongitudeLength", tileExtent.z);
				localViewModeMaterial.SetVector("InputOffsetAndScale", new Vector4(textureExtent.x, textureExtent.y, textureExtent.z, textureExtent.w));
				UnityEngine.Graphics.Blit(null, ((GPUResourceRenderTexture)output).NativeRenderTexture, localViewModeMaterial);
			}
		}
	}
}
