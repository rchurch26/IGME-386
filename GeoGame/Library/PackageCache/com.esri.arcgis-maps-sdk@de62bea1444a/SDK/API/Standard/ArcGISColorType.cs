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
    /// <summary>
    /// The different types of colors.
    /// </summary>
    /// <remarks>
    /// Each of different supported color types. Can get the type by calling <see cref="Standard.ArcGISColorType">ArcGISColorType</see>.
    /// </remarks>
    /// <seealso cref="Standard.ArcGISColor.ObjectType">ArcGISColor.ObjectType</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISColorType
    {
        /// <summary>
        /// RGBA color.
        /// </summary>
        /// <since>1.0.0</since>
        RGBColor = 0,
        
        /// <summary>
        /// An empty color.
        /// </summary>
        /// <since>1.0.0</since>
        Unknown = -1
    };
}