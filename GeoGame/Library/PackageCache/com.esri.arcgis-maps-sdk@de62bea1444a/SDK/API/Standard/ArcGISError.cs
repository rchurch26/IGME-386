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

namespace Esri.Standard
{
    /// <summary>
    /// An object that represents an error.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISError
    {
        #region Properties
        /// <summary>
        /// A string with additional information about the error.
        /// </summary>
        /// <remarks>
        /// The additional message is also a part of the dictionary returned by <see cref="">Error.additionalInformation</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        internal string AdditionalMessage
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Error_getAdditionalMessage(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The error's code.
        /// </summary>
        /// <since>1.0.0</since>
        internal int Code
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Error_getCode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The error's message.
        /// </summary>
        /// <since>1.0.0</since>
        internal string Message
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Error_getMessage(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The error's user defined failure.
        /// </summary>
        /// <since>1.0.0</since>
        internal IntPtr UserDefinedFailure
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Error_getUserDefinedFailure(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISError(IntPtr handle) => Handle = handle;
        
        ~ArcGISError()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Error_destroy(Handle, errorHandler);
                
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
        internal static extern IntPtr RT_Error_getAdditionalMessage(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_Error_getCode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Error_getMessage(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Error_getUserDefinedFailure(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Error_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}