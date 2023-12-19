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
namespace Esri.GameEngine.Layers.Base
{
    /// <summary>
    /// List of available types of layers
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISLayerType
    {
        /// <summary>
        /// An imagery layer
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISImageLayer = 0,
        
        /// <summary>
        /// A 3D object scene layer
        /// </summary>
        /// <since>1.0.0</since>
        ArcGIS3DObjectSceneLayer = 1,
        
        /// <summary>
        /// An integrated mesh layer
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISIntegratedMeshLayer = 2,
        
        /// <summary>
        /// An unsupported layer
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISUnsupportedLayer = 4,
        
        /// <summary>
        /// An unknown layer
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISUnknownLayer = 5
    };
}