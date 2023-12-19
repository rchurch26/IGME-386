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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public static class ArcGISMapCreatorHelpTool
	{
		private static VisualElement rootElement;

		public const string URL_DocumentationHomepage = "https://developers.arcgis.com/unity/";
		public const string URL_API = "https://developers.arcgis.com/unity/api-reference/";
		public const string URL_Samples = "https://github.com/esri/arcgis-maps-sdk-unity-samples/";

		public const string URL_Forums = "https://community.esri.com/t5/arcgis-maps-sdks-for-unity-questions/bd-p/arcgis-maps-sdks-unity-questions";
		public const string URL_Support = "https://developers.arcgis.com/unity/support/";
		public const string URL_GetAPIKey = "https://developers.arcgis.com/documentation/mapping-apis-and-services/security/api-keys/";
		public const string URL_ReleaseNotes = "https://developers.arcgis.com/unity/reference/release-notes/";

		public static VisualElement CreateHelpTool()
		{
			rootElement = new VisualElement();
			rootElement.name = "ArcGISMapCreatorHelpTool";

			var templatePath = MapCreatorUtilities.FindAssetPath("HelpToolTemplate");
			var template = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(templatePath);
			template.CloneTree(rootElement);

			InitLink("link-homepage", URL_DocumentationHomepage);
			InitLink("link-api", URL_API);
			InitLink("link-samples", URL_Samples);

			InitLink("link-forums", URL_Forums);
			InitLink("link-support", URL_Support);
			InitLink("link-get-api-key", URL_GetAPIKey);
			InitLink("link-release-notes", URL_ReleaseNotes);

			return rootElement;
		}

		private static void InitLink(string labelName, string url)
		{
			Label label = rootElement.Query<Label>(name: labelName);
			label.RegisterCallback<MouseDownEvent>(evnt =>
			{
				OpenURL(url);
			});
		}

		private static void OpenURL(string url)
		{
				Application.OpenURL(url);
		}

		[MenuItem("ArcGIS Maps SDK/Documentation", false, 21)]
		private static void OpenHomepage()
		{
				OpenURL(URL_DocumentationHomepage);
		}

		[MenuItem("ArcGIS Maps SDK/API Reference", false, 21)]
		private static void OpenAPI()
		{
				OpenURL(URL_API);
		}

		[MenuItem("ArcGIS Maps SDK/Samples", false, 21)]
		private static void OpenSamples()
		{
				OpenURL(URL_Samples);
		}

		[MenuItem("ArcGIS Maps SDK/Forums", false, 41)]
		private static void OpenForums()
		{
				OpenURL(URL_Forums);
		}

		[MenuItem("ArcGIS Maps SDK/SDK Support Resources", false, 41)]
		private static void OpenSupport()
		{
				OpenURL(URL_Support);
		}

		[MenuItem("ArcGIS Maps SDK/Get API Key", false, 41)]
		private static void OpenAPIKey()
		{
				OpenURL(URL_GetAPIKey);
		}

		[MenuItem("ArcGIS Maps SDK/Release Notes", false, 41)]
		private static void OpenReleaseNotes()
		{
				OpenURL(URL_ReleaseNotes);
		}
	}
}
