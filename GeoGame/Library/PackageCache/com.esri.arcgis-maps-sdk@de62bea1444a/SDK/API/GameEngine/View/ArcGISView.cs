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

namespace Esri.GameEngine.View
{
    /// <summary>
    /// A view for interaction with geographic content from an <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISView
    {
        #region Constructors
        /// <summary>
        /// Create a new <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <param name="gameEngineType">Specifies the client game engine type</param>
        /// <param name="globeModel">Specifies the model used to represent a 3D globe</param>
        /// <since>1.0.0</since>
        public ArcGISView(GameEngine.ArcGISGameEngineType gameEngineType, GameEngine.MapView.ArcGISGlobeModel globeModel)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_ArcGISView_create(gameEngineType, globeModel, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The current camera
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.MapView.ArcGISCamera Camera
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISView_getCamera(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.MapView.ArcGISCamera localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.MapView.ArcGISCamera(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_ArcGISView_setCamera(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The current map document.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Map.ArcGISMap Map
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISView_getMap(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Map.ArcGISMap localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Map.ArcGISMap(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_ArcGISView_setMap(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Serves render commands
        /// </summary>
        /// <since>1.0.0</since>
        internal GameEngine.RenderCommandQueue.ArcGISRenderCommandServer RenderCommandServer
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISView_getRenderCommandServer(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.RenderCommandQueue.ArcGISRenderCommandServer localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.RenderCommandQueue.ArcGISRenderCommandServer(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The current View Spatial Reference.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISView_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The current view options for the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISViewOptions ViewOptions
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ArcGISView_getViewOptions(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISView_setViewOptions(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Transforms a geographic coordinate in a GCS or PCS to game engine world cartesian space.
        /// If the view has no spatial reference the returned coordinate will have NaN values.
        /// </summary>
        /// <remarks>
        /// The geographicCoordinate.spatialReference does not have to match the view spatial reference (SR),
        /// but the coordinate will be internally reprojected to the view SR. Depending on the
        /// SR of geographicCoordinate, the result may be inaccurate, or the reprojection may fail and return
        /// NaN values, so specifying geographicCoordinate in the view SR is preferred.
        /// </remarks>
        /// <param name="geographicCoordinate">The geographic position in a GCS or PCS</param>
        /// <returns>
        /// A <see cref="GameEngine.Math.ArcGISVector3">ArcGISVector3</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public global::Unity.Mathematics.double3 GeographicToWorld(GameEngine.Geometry.ArcGISPoint geographicCoordinate)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeographicCoordinate = geographicCoordinate.Handle;
            
            var localResult = PInvoke.RT_ArcGISView_geographicToWorld(Handle, localGeographicCoordinate, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISVector3(localResult);
        }
        
        /// <summary>
        /// Retrieve the view's view state.
        /// </summary>
        /// <returns>
        /// A <see cref="GameEngine.View.State.ArcGISViewState">ArcGISViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISViewState GetViewState()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISView_getViewViewState(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieve the elevation source's view state.
        /// </summary>
        /// <param name="elevation">A elevation object to get the view state for.</param>
        /// <returns>
        /// A <see cref="GameEngine.View.State.ArcGISElevationSourceViewState">ArcGISElevationSourceViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISElevationSourceViewState GetViewState(GameEngine.Elevation.Base.ArcGISElevationSource elevation)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevation = elevation.Handle;
            
            var localResult = PInvoke.RT_ArcGISView_getElevationSourceViewState(Handle, localElevation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISElevationSourceViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISElevationSourceViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieve the layer's view state.
        /// </summary>
        /// <param name="layer">A layer object to get the view state for.</param>
        /// <returns>
        /// A <see cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISLayerViewState GetViewState(GameEngine.Layers.Base.ArcGISLayer layer)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayer = layer.Handle;
            
            var localResult = PInvoke.RT_ArcGISView_getLayerViewState(Handle, localLayer, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISLayerViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISLayerViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Handle a platform low memory event
        /// </summary>
        /// <since>1.0.0</since>
        public void HandleLowMemoryWarning()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISView_handleLowMemoryWarning(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the quotas for system and video memory that can be used by the ArcGISView
        /// </summary>
        /// <remarks>
        /// If provided video memory quota is < 0 on mobile devices, a shared memory model will be assumed.
        /// </remarks>
        /// <param name="systemMemory">The system memory quota in MiB</param>
        /// <param name="videoMemory">The video memory quota in MiB</param>
        /// <since>1.0.0</since>
        public void SetMemoryQuotas(long systemMemory, long videoMemory)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISView_setMemoryQuotas(Handle, systemMemory, videoMemory, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the viewport size and field of view. 
        /// Either field of view angle can be set to 0 to indicate "unset".
        /// For example, if verticalFieldOfViewDegrees is 0.0, but horizontalFieldOfViewDegrees is greater than zero, then the viewport vertical field
        /// of view will be set to the appropriate value given the horizontal FOV and distortion factor. And vice versa.
        /// </summary>
        /// <param name="viewportWidthPixels">used in visible tile calculation, on the basis that DPI is 96.</param>
        /// <param name="viewportHeightPixels">used in visible tile calculation, on the basis that DPI is 96.</param>
        /// <param name="horizontalFieldOfViewDegrees">A value in degrees. The valid range is 0 to 120</param>
        /// <param name="verticalFieldOfViewDegrees">A value in degrees. The valid range is 0 to 120</param>
        /// <param name="verticalDistortionFactor">Determines how much the vertical field of view is distorted. A distortion factor of 1.0 is default. A distortion factor less than 1.0 will cause the visuals to be stretched taller in comparison to their width. A distortion factor greater than 1.0 will cause the visuals to be shrunk shorter in comparison to their width.</param>
        /// <since>1.0.0</since>
        public void SetViewportProperties(uint viewportWidthPixels, uint viewportHeightPixels, float horizontalFieldOfViewDegrees, float verticalFieldOfViewDegrees, float verticalDistortionFactor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISView_setViewportProperties(Handle, viewportWidthPixels, viewportHeightPixels, horizontalFieldOfViewDegrees, verticalFieldOfViewDegrees, verticalDistortionFactor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Transforms a world coordinate to a geographic coordinate in the spatial reference of the view.
        /// If the view has no spatial reference the returned coordinate will have NaN values.
        /// </summary>
        /// <param name="worldCoordinate">The game engine world coordinate</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISPoint WorldToGeographic(global::Unity.Mathematics.double3 worldCoordinate)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localWorldCoordinate = Unity.Convert.ToArcGISVector3(worldCoordinate);
            
            var localResult = PInvoke.RT_ArcGISView_worldToGeographic(Handle, localWorldCoordinate, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Geometry.ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Sets a callback to be invoked when the elevation source view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISElevationSourceViewStateChangedEvent ElevationSourceViewStateChanged
        {
            get
            {
                return _elevationSourceViewStateChangedHandler.Delegate;
            }
            set
            {
                if (_elevationSourceViewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _elevationSourceViewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISView_setElevationSourceViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler.HandlerFunction, _elevationSourceViewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISView_setElevationSourceViewStateChangedCallback(Handle, null, _elevationSourceViewStateChangedHandler.UserData, errorHandler);
                    
                    _elevationSourceViewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the layer view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISLayerViewStateChangedEvent LayerViewStateChanged
        {
            get
            {
                return _layerViewStateChangedHandler.Delegate;
            }
            set
            {
                if (_layerViewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _layerViewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISView_setLayerViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler.HandlerFunction, _layerViewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISView_setLayerViewStateChangedCallback(Handle, null, _layerViewStateChangedHandler.UserData, errorHandler);
                    
                    _layerViewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the View's spatial reference changes.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISViewSpatialReferenceChangedEvent SpatialReferenceChanged
        {
            get
            {
                return _spatialReferenceChangedHandler.Delegate;
            }
            set
            {
                if (_spatialReferenceChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _spatialReferenceChangedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISView_setSpatialReferenceChangedCallback(Handle, ArcGISViewSpatialReferenceChangedEventHandler.HandlerFunction, _spatialReferenceChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISView_setSpatialReferenceChangedCallback(Handle, null, _spatialReferenceChangedHandler.UserData, errorHandler);
                    
                    _spatialReferenceChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISViewStateChangedEvent ViewStateChanged
        {
            get
            {
                return _viewStateChangedHandler.Delegate;
            }
            set
            {
                if (_viewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _viewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISView_setViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISViewStateChangedEventHandler.HandlerFunction, _viewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISView_setViewStateChangedCallback(Handle, null, _viewStateChangedHandler.UserData, errorHandler);
                    
                    _viewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISView(IntPtr handle) => Handle = handle;
        
        ~ArcGISView()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_elevationSourceViewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISView_setElevationSourceViewStateChangedCallback(Handle, null, _elevationSourceViewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _elevationSourceViewStateChangedHandler.Dispose();
                }
                
                if (_layerViewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISView_setLayerViewStateChangedCallback(Handle, null, _layerViewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _layerViewStateChangedHandler.Dispose();
                }
                
                if (_spatialReferenceChangedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISView_setSpatialReferenceChangedCallback(Handle, null, _spatialReferenceChangedHandler.UserData, IntPtr.Zero);
                    
                    _spatialReferenceChangedHandler.Dispose();
                }
                
                if (_viewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISView_setViewStateChangedCallback(Handle, null, _viewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _viewStateChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISView_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        internal GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler _elevationSourceViewStateChangedHandler = new GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler _layerViewStateChangedHandler = new GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler();
        
        internal ArcGISViewSpatialReferenceChangedEventHandler _spatialReferenceChangedHandler = new ArcGISViewSpatialReferenceChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISViewStateChangedEventHandler _viewStateChangedHandler = new GameEngine.View.State.ArcGISViewStateChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_create(GameEngine.ArcGISGameEngineType gameEngineType, GameEngine.MapView.ArcGISGlobeModel globeModel, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getCamera(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setCamera(IntPtr handle, IntPtr camera, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getMap(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setMap(IntPtr handle, IntPtr map, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getRenderCommandServer(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISViewOptions RT_ArcGISView_getViewOptions(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setViewOptions(IntPtr handle, ArcGISViewOptions viewOptions, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.Math.ArcGISVector3 RT_ArcGISView_geographicToWorld(IntPtr handle, IntPtr geographicCoordinate, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getViewViewState(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getElevationSourceViewState(IntPtr handle, IntPtr elevation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_getLayerViewState(IntPtr handle, IntPtr layer, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_handleLowMemoryWarning(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setMemoryQuotas(IntPtr handle, long systemMemory, long videoMemory, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setViewportProperties(IntPtr handle, uint viewportWidthPixels, uint viewportHeightPixels, float horizontalFieldOfViewDegrees, float verticalFieldOfViewDegrees, float verticalDistortionFactor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISView_worldToGeographic(IntPtr handle, GameEngine.Math.ArcGISVector3 worldCoordinate, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setElevationSourceViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventInternal elevationSourceViewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setLayerViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISLayerViewStateChangedEventInternal layerViewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setSpatialReferenceChangedCallback(IntPtr handle, ArcGISViewSpatialReferenceChangedEventInternal spatialReferenceChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_setViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISViewStateChangedEventInternal viewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISView_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}