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
namespace Esri.Standard
{
    /// <since>1.0.0</since>
    public enum ArcGISRuntimeClient
    {
        /// <summary>
        /// Unity.
        /// </summary>
        /// <since>1.0.0</since>
        Unity = 1,
        
        /// <summary>
        /// Unreal Engine.
        /// </summary>
        /// <since>1.0.0</since>
        Unreal = 2,
        
        /// <summary>
        /// No runtime client.
        /// </summary>
        /// <since>1.0.0</since>
        None = -1
    };
}