// COPYRIGHT 1995-2020 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static double3 FromArcGISVector3(GameEngine.Math.ArcGISVector3 value)
		{
			return math.double3(value.X, value.Y, value.Z);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static GameEngine.Math.ArcGISVector3 ToArcGISVector3(double3 value)
		{
			GameEngine.Math.ArcGISVector3 result;

			result.X = value.x;
			result.Y = value.y;
			result.Z = value.z;

			return result;
		}
	}
}
