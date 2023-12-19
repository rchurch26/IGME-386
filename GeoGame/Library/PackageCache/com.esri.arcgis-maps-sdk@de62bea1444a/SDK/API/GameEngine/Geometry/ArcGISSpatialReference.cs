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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// A spatial reference object.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSpatialReference
    {
        #region Constructors
        /// <summary>
        /// Creates a spatial reference based on WKID.
        /// </summary>
        /// <remarks>
        /// The method will create a spatial reference that has only horizontal coordinate system and does not have vertical
        /// coordinate system associated with it.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a positive value.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(int WKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_create(WKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference based on WKID for the horizontal coordinate system and vertical coordinate system.
        /// </summary>
        /// <remarks>
        /// The method creates a spatial reference that has both horizontal and vertical coordinate systems.
        /// When the vertical WKID is 0, the method is equivalent to calling <see cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</see>,
        /// and does not define a vertical coordinate system part.
        /// </remarks>
        /// <param name="WKID">The well-known ID of the horizontal coordinate system. Must be a positive value.</param>
        /// <param name="verticalWKID">The well-known ID of the vertical  coordinate system. Must be a non negative value.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(int WKID, int verticalWKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_createVerticalWKID(WKID, verticalWKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a spatial reference based on well-known text.
        /// </summary>
        /// <param name="wkText">The well-known text of the spatial reference to create.</param>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference(string wkText)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SpatialReference_createFromWKText(wkText, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// If the given spatial reference is a projected coordinate system, then this returns the geographic coordinate system of that system.
        /// </summary>
        /// <remarks>
        /// If the spatial reference is a projected coordinate system, then a spatial reference object representing the underlying geographic
        /// coordinate system is returned. Every projected coordinate system has an underlying geographic coordinate system. If the
        /// spatial reference is a geographic coordinate system, then a reference to itself is returned.
        /// If the spatial reference is a local spatial reference, a null is returned with an error.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference BaseGeographic
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getBaseGeographic(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The full world extent for the spatial reference.
        /// </summary>
        /// <remarks>
        /// The envelope defines the valid range of coordinates for the spatial reference.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</seealso>
        /// <since>1.0.0</since>
        public ArcGISEnvelope FullWorldExtent
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getFullWorldExtent(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISEnvelope localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISEnvelope(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference has a vertical coordinate system set; false otherwise.
        /// </summary>
        /// <remarks>
        /// A spatial reference can optionally include a definition for a vertical coordinate system (VCS), which
        /// can be used to interpret the z-values of a geometry. A VCS defines linear units of measure, the origin of
        /// z-values, and whether z-values are "positive up" (representing heights above a surface) or "positive down"
        /// (indicating that values are depths below a surface).
        /// 
        /// A <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> may have a VCS set, for example by calling the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</see> constructor. <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalWKID">ArcGISSpatialReference.VerticalWKID</see>,
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.WKText">ArcGISSpatialReference.WKText</see>, and <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</see> provide more information about the
        /// specific VCS set on this instance.
        /// 
        /// VCSs are used when projecting geometries using a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see>.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalWKID">ArcGISSpatialReference.VerticalWKID</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalUnit">ArcGISSpatialReference.VerticalUnit</seealso>
        /// <since>1.0.0</since>
        public bool HasVertical
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getHasVertical(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference is a Geographic Coordinate System.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsGeographic
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsGeographic(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if coordinate system is horizontally pannable.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsPannable
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsPannable(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if spatial reference is a Projected Coordinate System.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsProjected
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getIsProjected(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spheroid data for the spatial reference
        /// </summary>
        /// <remarks>
        /// The spheroid data for the spatial reference.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpheroidData SpheroidData
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getSpheroidData(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for the horizontal coordinate system of this spatial reference.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISUnit Unit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = GameEngine.Geometry.PInvoke.RT_Unit_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case GameEngine.Geometry.ArcGISUnitType.AngularUnit:
                            localLocalResult = new ArcGISAngularUnit(localResult);
                            break;
                        case GameEngine.Geometry.ArcGISUnitType.AreaUnit:
                            localLocalResult = new ArcGISAreaUnit(localResult);
                            break;
                        case GameEngine.Geometry.ArcGISUnitType.LinearUnit:
                            localLocalResult = new ArcGISLinearUnit(localResult);
                            break;
                        default:
                            localLocalResult = new ArcGISUnit(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for the vertical coordinate system of this spatial reference.
        /// </summary>
        /// <remarks>
        /// Is null if <see cref="GameEngine.Geometry.ArcGISSpatialReference.HasVertical">ArcGISSpatialReference.HasVertical</see> is false.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit VerticalUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISLinearUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISLinearUnit(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The well-known ID for the vertical coordinate system (VCS), or 0 if the spatial reference
        /// has no VCS or has a custom VCS.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference.ArcGISSpatialReference">ArcGISSpatialReference.ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public int VerticalWKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getVerticalWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known ID for the horizontal coordinate system, or 0 if the spatial reference
        /// has a custom horizontal coordinate system.
        /// </summary>
        /// <since>1.0.0</since>
        public int WKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The well-known text for the horizontal and vertical coordinate system.
        /// </summary>
        /// <since>1.0.0</since>
        public string WKText
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SpatialReference_getWKText(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Check if the 2 spatial references are equal.
        /// </summary>
        /// <remarks>
        /// Check spatial references to see if they are equal. Returns false if an error occurs.
        /// </remarks>
        /// <param name="right">The 2nd spatial reference to check to see if equal to the 1st.</param>
        /// <returns>
        /// True if the spatial references are equal otherwise false.
        /// </returns>
        /// <since>1.0.0</since>
        public bool Equals(ArcGISSpatialReference right)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localRight = right.Handle;
            
            var localResult = PInvoke.RT_SpatialReference_equals(Handle, localRight, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculate the grid convergence for a spatial reference at a given point.
        /// </summary>
        /// <remarks>
        /// The grid convergence is the angle between True North and Grid North
        /// at a point on a map. The grid convergence can be used to convert a
        /// horizontal direction expressed as an azimuth in a geographic
        /// coordinate system (relative to True North) to a direction expressed
        /// as a bearing in a projected coordinate system (relative to Grid
        /// North), and vice versa.
        /// 
        /// Sign convention
        /// 
        /// The grid convergence returned by this method is positive when Grid
        /// North lies east of True North. The following formula demonstrates
        /// how to obtain a bearing (b) from an azimuth (a) using the grid
        /// convergence (c) returned by this method:
        /// 
        /// b = a - c
        /// 
        /// This sign convention is sometimes named the Gauss-Bomford convention.
        /// 
        /// Other notes:
        /// * Returns 0 if the spatial reference is a geographic coordinate system
        /// * Returns NAN if the point is outside the projection's horizon or on error
        /// * If the point has no spatial reference, it is assumed to be in the
        ///   given spatial reference
        /// * If the point's spatial reference differs from the spatial
        ///   reference given, its location is transformed automatically to
        ///   the given spatial reference
        /// </remarks>
        /// <param name="point">The point</param>
        /// <returns>
        /// The grid convergence in degrees.
        /// </returns>
        /// <since>1.0.0</since>
        public double GetConvergenceAngle(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_SpatialReference_getConvergenceAngle(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Creates a spatial reference based on WGS84.
        /// </summary>
        /// <remarks>
        /// The method creates a WGS84 spatial reference.
        /// </remarks>
        /// <returns>
        /// A spatial reference. This is passed to spatial reference functions.
        /// null if an error occurs.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISSpatialReference WGS84()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_WGS84(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a spatial reference based on web Mercator.
        /// </summary>
        /// <remarks>
        /// The method creates a web Mercator spatial reference.
        /// </remarks>
        /// <returns>
        /// A spatial reference. This is passed to spatial reference functions.
        /// null if an error occurs.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISSpatialReference WebMercator()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_SpatialReference_webMercator(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSpatialReference localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISSpatialReference(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISSpatialReference(IntPtr handle) => Handle = handle;
        
        ~ArcGISSpatialReference()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SpatialReference_destroy(Handle, errorHandler);
                
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
        internal static extern IntPtr RT_SpatialReference_create(int WKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_createVerticalWKID(int WKID, int verticalWKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_createFromWKText([MarshalAs(UnmanagedType.LPStr)]string wkText, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getBaseGeographic(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getFullWorldExtent(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getHasVertical(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsGeographic(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsPannable(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_getIsProjected(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISSpheroidData RT_SpatialReference_getSpheroidData(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getVerticalUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReference_getVerticalWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_SpatialReference_getWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_getWKText(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_SpatialReference_equals(IntPtr handle, IntPtr right, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_SpatialReference_getConvergenceAngle(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_WGS84(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SpatialReference_webMercator(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SpatialReference_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}