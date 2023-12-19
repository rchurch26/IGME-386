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
    /// A compose render command
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISComposeCommandParameters
    {
        /// <summary>
        /// The source 1 parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public uint SourceId1;
        
        /// <summary>
        /// The source 2 parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public uint SourceId2;
        
        /// <summary>
        /// The alpha parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public float Alpha;
        
        /// <summary>
        /// Id of the render target to be used as the output of the compose.
        /// </summary>
        /// <since>1.0.0</since>
        public uint TargetId;
        
        /// <summary>
        /// The region parameter of this render command
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Math.ArcGISVector4 Region;
    }
}