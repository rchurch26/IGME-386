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

namespace Esri.GameEngine.RenderCommandQueue
{
    /// <summary>
    /// A data buffer view
    /// </summary>
    /// <remarks>
    /// The data pointer is valid for 5 game frames, which is equivalent to 5 executions of RenderCommandServer.getRenderCommands().
    /// In a multi-threading render architecture this lifetime enables using the data pointer in different threads without having
    /// to create extra copies to keep the data valid.
    /// For example, the game thread could receive the data pointer and share it safely with the render thread, which may execute 1 or 2 frames after,
    /// because the lifetime of the pointer is 5 frames.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISDataBufferView
    {
        /// <summary>
        /// The data parameter
        /// </summary>
        /// <since>1.0.0</since>
        public IntPtr Data;
        
        /// <summary>
        /// The size parameter
        /// </summary>
        /// <since>1.0.0</since>
        public uint Size;
    }
}