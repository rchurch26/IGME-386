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

namespace Esri.GameEngine.Attributes
{
    /// <summary>
    /// Information about the visualization attribute
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISVisualizationAttribute
    {
        #region Properties
        /// <summary>
        /// The visualization attribute data for a particular node.
        /// </summary>
        /// <remarks>
        /// Data is only valid during the scope of <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public global::Unity.Collections.NativeArray<byte> Data
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VisualizationAttribute_getData(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISByteArrayStruct(localResult);
            }
        }
        
        /// <summary>
        /// The visualization attribute name
        /// </summary>
        /// <since>1.0.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VisualizationAttribute_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The type of the attribute.
        /// </summary>
        /// <seealso cref="GameEngine.Attributes.ArcGISVisualizationAttributeType">ArcGISVisualizationAttributeType</seealso>
        /// <since>1.0.0</since>
        public ArcGISVisualizationAttributeType VisualizationAttributeType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VisualizationAttribute_getVisualizationAttributeType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISVisualizationAttribute(IntPtr handle) => Handle = handle;
        
        ~ArcGISVisualizationAttribute()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_VisualizationAttribute_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern Standard.ArcGISIntermediateByteArrayStruct RT_VisualizationAttribute_getData(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VisualizationAttribute_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISVisualizationAttributeType RT_VisualizationAttribute_getVisualizationAttributeType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_VisualizationAttribute_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}