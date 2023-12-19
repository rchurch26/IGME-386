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
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Map;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Utils
{
	public static class FrustumHelpers
	{
		public static double CalculateNearPlaneDistance(double altitude, double hfieldOfView, double aspect, double maxTerrainAltitude = GeoUtils.MaxTerrainAltitude)
		{
			double vFov = 0.5 * hfieldOfView * MathUtils.DegreesToRadians;
			double hFov = System.Math.Atan(System.Math.Tan(vFov) * aspect);

			double3 frustumCornerDir;
			frustumCornerDir.x = System.Math.Cos(hFov);
			frustumCornerDir.y = System.Math.Sin(vFov);
			frustumCornerDir.z = System.Math.Sin(hFov) + System.Math.Cos(vFov);
			frustumCornerDir = math.normalize(frustumCornerDir);

			double distance = (altitude - maxTerrainAltitude) * math.dot(frustumCornerDir, math.forward());

			// Multiply highest altitude by 2 to give a buffer where we still use 0.5 as the near plane
			return System.Math.Min(2e5, altitude > maxTerrainAltitude * 2.0 ? distance : 0.5);
		}

		public static double CalculateFarPlaneDistance(double altitude, ArcGISMapType mapType, in ArcGISSpatialReference spatialReference)
		{
			double epsilon = 0.01;
			double minFarPlaneDistance = 50000.0;

			if (altitude <= 0)
			{
				return minFarPlaneDistance;
			}

			if (mapType == ArcGISMapType.Local || spatialReference == null)
			{
				return System.Math.Max(minFarPlaneDistance, System.Math.Sqrt(System.Math.Pow(altitude / epsilon, 2) - altitude * altitude));
			}
			else
			{
				var majorSemiAxis = spatialReference.SpheroidData.MajorSemiAxis;
				return System.Math.Max(epsilon, (majorSemiAxis + altitude) * System.Math.Sqrt(1.0 - System.Math.Pow(majorSemiAxis / (majorSemiAxis + altitude), 2)));
			}
		}
	}
}
