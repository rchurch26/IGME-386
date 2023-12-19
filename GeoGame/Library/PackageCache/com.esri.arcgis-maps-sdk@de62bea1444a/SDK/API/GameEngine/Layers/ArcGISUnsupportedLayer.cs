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
using System;

namespace Esri.GameEngine.Layers
{
    /// <summary>
    /// An instance of this class represents a layer type that is not currently supported.
    /// </summary>
    /// <remarks>
    /// The layer will be persisted in the string returned by <see cref="">Map.toJSON</see>,
    /// but will not be drawn by the map view.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISUnsupportedLayer :
        GameEngine.Layers.Base.ArcGISLayer
    {
        #region Internal Members
        internal ArcGISUnsupportedLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        #endregion // P-Invoke Declarations
    }
}