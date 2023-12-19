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

namespace Esri.GameEngine.MapView
{
	public partial class ArcGISCamera
	{
		public ArcGISCamera(GameEngine.Geometry.ArcGISPoint locationPoint, ArcGISMapsSDK.Utils.GeoCoord.ArcGISRotation rotation)
		{
			var errorHandler = Unity.ArcGISErrorManager.CreateHandler();

			var localLocationPoint = locationPoint.Handle;

			Handle = PInvoke.RT_Camera_createWithLocationHeadingPitchRoll(localLocationPoint, rotation.Heading, rotation.Pitch, rotation.Roll, errorHandler);

			Unity.ArcGISErrorManager.CheckError(errorHandler);
		}
	}
}
