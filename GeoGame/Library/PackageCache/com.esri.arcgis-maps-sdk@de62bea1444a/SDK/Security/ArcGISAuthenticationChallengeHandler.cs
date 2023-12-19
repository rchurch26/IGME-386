// COPYRIGHT 1995-2021 ESRI
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
using Esri.GameEngine.Security;
using System;

namespace Esri.ArcGISMapsSDK.Security
{
	public abstract class ArcGISAuthenticationChallengeHandler : IDisposable
	{
		public void Dispose()
		{

		}

		public abstract ArcGISAuthenticationChallengeType GetAuthenticationChallengeType();
		public abstract void HandleChallenge(ArcGISAuthenticationChallenge authenticationChallenge);
	}
}
