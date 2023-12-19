using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Extent;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]

public class GeoGameScript : MonoBehaviour
{
    public string APIKey = "AAPK926a4882e21249c394acbaf7f319bbddtAWHpk9n2nNITsQLttXhPH59KEffP1zRBXjpBlKGvQDFzQKMOqt8OaZWYlALA3BU";

    private ArcGISMapComponent map;

    const float lng = -77.6038695f;
    const float lat = 43.1547372f;
    const float alt = 1000f;

    private ArcGISPoint geoCoord = new ArcGISPoint(lng, lat, alt, ArcGISSpatialReference.WGS84());

    // This sample event is used in conjunction with a Sample3DAttributes component
    // It passes a layer to a listener to process its attributes
    // The Sample3DAttributes component is not required, so you are free to remove this event and its invocations in both scripts
    // See ArcGISMapsSDK/Samples/Scripts/3DAttributesSample/Sample3DAttributesComponent.cs for more info
    public delegate void SetLayerAttributesEventHandler(Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer layer);
    public event SetLayerAttributesEventHandler OnSetLayerAttributes;

    private ArcGISCameraComponent cam;

    // Start is called before the first frame update
    void Start()
    {
        CreateArcGISMapComponent();
        CreateArcGISCamera();
        //CreateSkyComponent();
        CreateArcGISMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateArcGISMapComponent()
    {
        map = FindObjectOfType<ArcGISMapComponent>();
        if(!map)
        {
            GameObject mapGameObject = new GameObject("ArcGISMap");
            map = mapGameObject.AddComponent<ArcGISMapComponent>();
        }
        map.OriginPosition = geoCoord;
        map.MapType = Esri.GameEngine.Map.ArcGISMapType.Local;
        map.MapTypeChanged += new ArcGISMapComponent.MapTypeChangedEventHandler(CreateArcGISMap);
    }

    public void CreateArcGISMap()
    {
        Esri.GameEngine.Map.ArcGISMap arcGISMap = new Esri.GameEngine.Map.ArcGISMap(map.MapType);
        arcGISMap.Basemap = Esri.GameEngine.Map.ArcGISBasemap.CreateImagery(APIKey);
        arcGISMap.Elevation = new Esri.GameEngine.Map.ArcGISMapElevation(new Esri.GameEngine.Elevation.ArcGISImageElevationSource("https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer", "Elevation", ""));

        //Esri.GameEngine.Layers.ArcGISImageLayer layer1 = new Esri.GameEngine.Layers.ArcGISImageLayer("https://tiles.arcgis.com/tiles/")
        Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer buildingLayer = new Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer("https://tiles.arcgis.com/tiles/RQcpPaCpMAXzUI5g/arcgis/rest/services/GeoGameSceneLayer/SceneServer", "Building Layer", 1.0f, true, "");
        arcGISMap.Layers.Add(buildingLayer);

        // This call invokes an event used by the Sample3DAttributes component
        if (OnSetLayerAttributes != null)
        {
            OnSetLayerAttributes(buildingLayer);
        }

        Esri.GameEngine.Geometry.ArcGISPoint extentCenter = new Esri.GameEngine.Geometry.ArcGISPoint(lat, lng, alt, ArcGISSpatialReference.WGS84());
        ArcGISExtentCircle extent = new ArcGISExtentCircle(extentCenter, 1000);
        arcGISMap.ClippingArea = extent;

        map.View.Map = arcGISMap;
    }

    private void CreateArcGISCamera()
    {
        cam = Camera.main.gameObject.GetComponent<ArcGISCameraComponent>();
        if(!cam)
        {
            GameObject camGameObject = Camera.main.gameObject;
            camGameObject.transform.SetParent(map.transform, false);
            cam = camGameObject.AddComponent<ArcGISCameraComponent>();
            camGameObject.AddComponent<ArcGISCameraControllerComponent>();
            camGameObject.AddComponent<ArcGISRebaseComponent>();
        }

        ArcGISLocationComponent camLocationComponent = cam.GetComponent<ArcGISLocationComponent>();
        if(!camLocationComponent)
        {
            camLocationComponent = cam.gameObject.AddComponent<ArcGISLocationComponent>();
            camLocationComponent.Position = geoCoord;
            camLocationComponent.Rotation = new ArcGISRotation(55, 58, 0);
        }
    }
}
