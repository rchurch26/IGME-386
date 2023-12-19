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

namespace Esri.GameEngine.Layers.Base
{
    /// <summary>
    /// Abstract class layer, base for layers
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISLayer :
        GameEngine.ArcGISLoadable
    {
        #region Constructors
        /// <summary>
        /// Creates a new layer.
        /// </summary>
        /// <remarks>
        /// Creates a new layer.
        /// </remarks>
        /// <param name="source">Layer source</param>
        /// <param name="type">Layer type definition.</param>
        /// <param name="APIKey">API Key used to load data.</param>
        /// <since>1.0.0</since>
        public ArcGISLayer(string source, ArcGISLayerType type, string APIKey)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_ArcGISLayer_create(source, type, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// API Key will be sended on loading process to match the new credit system.
        /// </summary>
        /// <since>1.0.0</since>
        public string APIKey
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getAPIKey(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The full extent of this layer, which is the extent where all layer data is contained.
        /// </summary>
        /// <remarks>
        /// You can use this to zoom
        /// to all of the data contained in this layer.
        /// </remarks>
        /// <since>1.0.0</since>
        public GameEngine.Extent.ArcGISExtentRectangle Extent
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getExtent(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Extent.ArcGISExtentRectangle localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Extent.ArcGISExtentRectangle(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// Layer visible true or false
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsVisible
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getIsVisible(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISLayer_setIsVisible(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// This property will help the user to identify the layer on his application.
        /// </summary>
        /// <since>1.0.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISLayer_setName(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Defines what type of layer is it. Is read only and it will be setup on the constructor
        /// </summary>
        /// <since>1.0.0</since>
        internal ArcGISLayerType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Layer Opacity
        /// </summary>
        /// <since>1.0.0</since>
        public float Opacity
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getOpacity(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISLayer_setOpacity(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Source property will be a read only, it will be setup on the constructor
        /// </summary>
        /// <since>1.0.0</since>
        public string Source
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getSource(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The spatial reference of the layer.
        /// </summary>
        /// <remarks>
        /// <see cref="GameEngine.Layers.ArcGISImageLayer">ArcGISImageLayer</see> <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> must match the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// <see cref="GameEngine.Layers.ArcGISImageLayer">ArcGISImageLayer</see> tiling scheme must be compatible with the tiling scheme of the <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// If any of the above constraints are violated, a <see cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</see> error is generated.
        /// </remarks>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region GameEngine.ArcGISLoadable
        public Exception LoadError
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getLoadError(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISError(new Standard.ArcGISError(localResult));
            }
        }
        
        public GameEngine.ArcGISLoadStatus LoadStatus
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISLayer_getLoadStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        public void CancelLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISLayer_cancelLoad(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void Load()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISLayer_load(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void RetryLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISLayer_retryLoad(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public GameEngine.ArcGISLoadableDoneLoadingEvent DoneLoading
        {
            get
            {
                return _doneLoadingHandler.Delegate;
            }
            set
            {
                if (_doneLoadingHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _doneLoadingHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISLayer_setDoneLoadingCallback(Handle, GameEngine.ArcGISLoadableDoneLoadingEventHandler.HandlerFunction, _doneLoadingHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISLayer_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, errorHandler);
                    
                    _doneLoadingHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        public GameEngine.ArcGISLoadableLoadStatusChangedEvent LoadStatusChanged
        {
            get
            {
                return _loadStatusChangedHandler.Delegate;
            }
            set
            {
                if (_loadStatusChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _loadStatusChangedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISLayer_setLoadStatusChangedCallback(Handle, GameEngine.ArcGISLoadableLoadStatusChangedEventHandler.HandlerFunction, _loadStatusChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISLayer_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, errorHandler);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // GameEngine.ArcGISLoadable
        
        #region Internal Members
        internal ArcGISLayer(IntPtr handle) => Handle = handle;
        
        ~ArcGISLayer()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_doneLoadingHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISLayer_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, IntPtr.Zero);
                    
                    _doneLoadingHandler.Dispose();
                }
                
                if (_loadStatusChangedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISLayer_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, IntPtr.Zero);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISLayer_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        internal GameEngine.ArcGISLoadableDoneLoadingEventHandler _doneLoadingHandler = new GameEngine.ArcGISLoadableDoneLoadingEventHandler();
        
        internal GameEngine.ArcGISLoadableLoadStatusChangedEventHandler _loadStatusChangedHandler = new GameEngine.ArcGISLoadableLoadStatusChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, ArcGISLayerType type, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getAPIKey(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getExtent(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ArcGISLayer_getIsVisible(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_setIsVisible(IntPtr handle, [MarshalAs(UnmanagedType.I1)]bool isVisible, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_setName(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string name, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISLayerType RT_ArcGISLayer_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern float RT_ArcGISLayer_getOpacity(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_setOpacity(IntPtr handle, float opacity, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getSource(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.ArcGISLoadable P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISLayer_getLoadError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.ArcGISLoadStatus RT_ArcGISLayer_getLoadStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_cancelLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_load(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_retryLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_setDoneLoadingCallback(IntPtr handle, GameEngine.ArcGISLoadableDoneLoadingEventInternal doneLoading, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISLayer_setLoadStatusChangedCallback(IntPtr handle, GameEngine.ArcGISLoadableLoadStatusChangedEventInternal loadStatusChanged, IntPtr userData, IntPtr errorHandler);
        #endregion // GameEngine.ArcGISLoadable P-Invoke Declarations
    }
}