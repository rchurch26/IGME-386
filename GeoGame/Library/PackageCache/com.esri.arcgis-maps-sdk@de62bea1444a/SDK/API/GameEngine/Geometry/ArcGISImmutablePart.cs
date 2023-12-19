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
    /// Represents a single part of a multipart geometry (polygon or polyline).
    /// </summary>
    /// <remarks>
    /// A collection of <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> objects that together represent a part in a <see cref="GameEngine.Geometry.ArcGISMultipart">ArcGISMultipart</see> geometry. You
    /// can also access the <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> objects that represent the vertices of the geometry (that is, the ends of each
    /// segment), using point-based helpers such as <see cref="GameEngine.Geometry.ArcGISImmutablePart.GetPoint">ArcGISImmutablePart.GetPoint</see>.
    /// 
    /// Prior to v100.12, the only supported segment type was <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>. If the underlying geometry contained
    /// curve segments (<see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see> is true) then the curve information was lost when iterating through
    /// the segments in that part.
    /// 
    /// From v100.12, curve segments may be returned from <see cref="GameEngine.Geometry.ArcGISImmutablePart.GetSegment">ArcGISImmutablePart.GetSegment</see>. A part may contain
    /// a mix of linear and curve segments.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISImmutablePart
    {
        #region Properties
        /// <summary>
        /// The end point of the last segment in the part.
        /// Returns null if the collection is empty.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint EndPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getEndPoint(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// Indicates if the part contains any curve segments.
        /// </summary>
        /// <remarks>
        /// Prior to v100.12, if this property returned true, there was no way to access the curve segment information
        /// contained by the part. Retrieving the <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> instances of the part would return only <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>
        /// instances.
        /// 
        /// From v100.12, when this property returns true, curve segments may be returned from 
        /// <see cref="GameEngine.Geometry.ArcGISImmutablePart.GetSegment">ArcGISImmutablePart.GetSegment</see>. A part may contain a mix of linear and curve segments.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</seealso>
        /// <since>1.0.0</since>
        public bool HasCurves
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getHasCurves(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Indicates if the part contains no segments.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The count of points in the part.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// Segments can share a point if the end point of one segment matches the start point of the next.
        /// </remarks>
        /// <since>1.0.0</since>
        public ulong PointCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getPointCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The count of segments in the part.
        /// </summary>
        /// <since>1.0.0</since>
        public ulong SegmentCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getSegmentCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The spatial reference for the immutable part.
        /// </summary>
        /// <remarks>
        /// If the collection does not have a spatial reference null is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getSpatialReference(Handle, errorHandler);
                
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
        /// The start point of the first segment in the part.
        /// Returns null if the collection is empty.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint StartPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ImmutablePart_getStartPoint(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// For a segment at a specified segment_index the method returns the point index of the segment's end point.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// A point index
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetEndPointIndexFromSegmentIndex(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getEndPointIndexFromSegmentIndex(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Returns a point at a specified point index.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// Segments can share a point if the end point of one segment matches the start point of the next.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISPoint GetPoint(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getPoint(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns all the points that are the vertexes of the part.
        /// </summary>
        /// <returns>
        /// A collections of points in the immutable part.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISImmutablePointCollection GetPoints()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ImmutablePart_getPoints(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISImmutablePointCollection localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISImmutablePointCollection(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets a segment at a specified segment index.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISSegment GetSegment(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getSegment(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSegment localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Segment_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISSegmentType.CubicBezierSegment:
                        localLocalResult = new ArcGISCubicBezierSegment(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISSegmentType.EllipticArcSegment:
                        localLocalResult = new ArcGISEllipticArcSegment(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISSegmentType.LineSegment:
                        localLocalResult = new ArcGISLineSegment(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISSegment(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Converts from a point index to a segment index that uses the given point as an end point.
        /// If the point is not an end point then a value equivalent to -1 value is returned.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// The segment index containing the end point. If the point index is not an end point a value equivalent to -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetSegmentIndexFromEndPointIndex(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getSegmentIndexFromEndPointIndex(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Converts from a point index to a segment index of the start point and another segment index containing the end point.
        /// If the point is not a start or end point a value equivalent to -1 is set.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="outStartPointSegmentIndex">This is set to the segment index using the point as a start point. Can be null.</param>
        /// <param name="outEndPointSegmentIndex">This is set to the segment index using the point as an end point. Can be null.</param>
        /// <since>1.0.0</since>
        public void GetSegmentIndexFromPointIndex(ulong pointIndex, IntPtr outStartPointSegmentIndex, IntPtr outEndPointSegmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_ImmutablePart_getSegmentIndexFromPointIndex(Handle, localPointIndex, outStartPointSegmentIndex, outEndPointSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Converts from a point index to a segment index that uses the given point as a start point.
        /// If the point is not a start point then a value equivalent to -1 value is returned.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// The segment index containing the start point. If point is not a start point a value equivalent to -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetSegmentIndexFromStartPointIndex(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getSegmentIndexFromStartPointIndex(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// For a segment at a specified segment_index the method returns the point index of the segment's start point.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// A point index
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetStartPointIndexFromSegmentIndex(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_ImmutablePart_getStartPointIndexFromSegmentIndex(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISImmutablePart(IntPtr handle) => Handle = handle;
        
        ~ArcGISImmutablePart()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ImmutablePart_destroy(Handle, errorHandler);
                
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
        internal static extern IntPtr RT_ImmutablePart_getEndPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ImmutablePart_getHasCurves(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ImmutablePart_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getPointCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getSegmentCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePart_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePart_getStartPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getEndPointIndexFromSegmentIndex(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePart_getPoint(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePart_getPoints(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ImmutablePart_getSegment(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getSegmentIndexFromEndPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ImmutablePart_getSegmentIndexFromPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr outStartPointSegmentIndex, IntPtr outEndPointSegmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getSegmentIndexFromStartPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ImmutablePart_getStartPointIndexFromSegmentIndex(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ImmutablePart_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}