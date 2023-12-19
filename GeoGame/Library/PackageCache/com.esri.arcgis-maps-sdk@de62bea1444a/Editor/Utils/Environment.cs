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
using Esri.ArcGISMapsSDK.Editor.Security;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	[InitializeOnLoad]
	public class Environment
	{
		static Environment()
		{
			var authenticationChallengeHandler = new ArcGISEditorOAuthAuthenticationChallengeHandler();

			ArcGISMapsSDK.Utils.Environment.SetAuthenticationChallengeHandler(authenticationChallengeHandler);
		}
	}
}

