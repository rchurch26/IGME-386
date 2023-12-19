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
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Map;

namespace Esri.ArcGISMapsSDK.Utils
{
	public static class AtmosphereHelpers
	{
		public static float CalculateGlobalViewSkyAtmosphereOffsetFrom(double altitude, double pointOfViewToGlobeCenterDistance, ArcGISSpatialReference spatialReference)
		{
			var ellipsoidRadius = pointOfViewToGlobeCenterDistance - altitude;
			var deviationMajorSemiAxisEllipsoidRadius = spatialReference.SpheroidData.MajorSemiAxis - ellipsoidRadius;

			return (float)(-pointOfViewToGlobeCenterDistance - deviationMajorSemiAxisEllipsoidRadius);
		}

		public static float CalculateFogMeanFreePathPropertyFrom(double altitude, ArcGISSpatialReference spatialReference)
		{
			double farClip = FrustumHelpers.CalculateFarPlaneDistance(altitude, ArcGISMapType.Global, spatialReference);

			return (float)(altitude + farClip * 0.15);
		}
	}
}
