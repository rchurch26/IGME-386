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
using System.Runtime.InteropServices;

namespace Esri.GameEngine.RenderCommandQueue.Parameters
{
    /// <summary>
    /// A generate normal texture render command
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISGenerateNormalTextureCommandParameters
    {
        /// <summary>
        /// The elevation parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public uint ElevationId;
        
        /// <summary>
        /// The tile extent parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Math.ArcGISVector4 TileExtent;
        
        /// <summary>
        /// The texture extent parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Math.ArcGISVector4 TextureExtent;
        
        /// <summary>
        /// The target parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public uint TargetId;
    }
}