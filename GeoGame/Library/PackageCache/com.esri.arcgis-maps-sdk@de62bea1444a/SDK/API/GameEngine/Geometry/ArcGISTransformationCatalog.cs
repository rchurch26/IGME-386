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
    /// Allows discovery and management of the transformations used to convert coordinates between different datums.
    /// </summary>
    /// <remarks>
    /// The transformation catalog class provides a mechanism for discovering available <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> objects.
    /// You can retrieve the default transformation used internally by Runtime to convert coordinates, and find other 
    /// available transformations. An area of interest may be taken into account.
    /// 
    /// This class also allows you to set the location where Projection Engine data files needed for some 
    /// transformations are found on local storage. The <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see> property 
    /// must be set before using any geometry classes.
    /// 
    /// The default transformation used when reprojecting geometries is the most suitable available transformation 
    /// across the area covered by the input and output spatial references. You can sometimes get a more suitable 
    /// transformation for work within a smaller area by specifying the extent of your specific work area, using 
    /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISTransformationCatalog
    {
        #region Properties
        /// <summary>
        /// The directory of the Projection Engine files on the local file system.
        /// </summary>
        /// <remarks>
        /// This method should only be called immediately upon process startup before any other API method calls.
        /// </remarks>
        /// <since>1.0.0</since>
        public static string ProjectionEngineDirectory
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_TransformationCatalog_getProjectionEngineDirectory(errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_TransformationCatalog_setProjectionEngineDirectory(value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Returns the best usable (default) transformation to transform between the input and output spatial
        /// references, without taking into account an area of interest.
        /// </summary>
        /// <remarks>
        /// The default transformation is the one used in cases such as calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        /// without specifying a transformation.
        /// </remarks>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> instance that represents the best choice given the parameters. Always returns a
        /// usable transformation where <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is false. Returns null if
        /// no usable transformation is available for the given input parameters, or if no transformation is required.
        /// 
        /// Prior to version 100.9, this method returned a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see>.
        /// 
        /// From version 100.9 onwards, if both inputSpatialReference and outputSpatialReference have a vertical 
        /// coordinate system, this method returns a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> instance. If either 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> does not have a vertical coordinate system, a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> is returned.
        /// To replicate the former behavior, use the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>
        /// method instead with 'ignoreVertical = true'.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</seealso>
        /// <since>1.0.0</since>
        public static ArcGISDatumTransformation GetTransformation(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformation(localInputSpatialReference, localOutputSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISDatumTransformation localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_DatumTransformation_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISDatumTransformationType.GeographicTransformation:
                        localLocalResult = new ArcGISGeographicTransformation(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISDatumTransformationType.HorizontalVerticalTransformation:
                        localLocalResult = new ArcGISHorizontalVerticalTransformation(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISDatumTransformation(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the best usable transformation to transform between the input and output spatial references,
        /// taking into account the area of interest, if specified.
        /// </summary>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <param name="areaOfInterest">The bounding box of coordinates to be transformed, or null to consider the entire world extent.</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> instance that represents the best choice given the parameters. Always returns a
        /// usable transformation where <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is false. Returns null if
        /// no usable transformation is available for the given input parameters, or if no transformation is required.
        /// 
        /// Prior to version 100.9, this method returned a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see>.
        /// 
        /// From version 100.9 onwards, if both inputSpatialReference and outputSpatialReference have a vertical 
        /// coordinate system, this method returns a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> instance. If either 
        /// SpatialReference does not have a vertical coordinate system, a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> is returned.
        /// To replicate the former behavior, use the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>
        /// method instead with 'ignoreVertical = true'.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISDatumTransformation GetTransformation(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference, ArcGISEnvelope areaOfInterest)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            var localAreaOfInterest = areaOfInterest == null ? System.IntPtr.Zero : areaOfInterest.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformationWithAreaOfInterest(localInputSpatialReference, localOutputSpatialReference, localAreaOfInterest, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISDatumTransformation localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_DatumTransformation_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISDatumTransformationType.GeographicTransformation:
                        localLocalResult = new ArcGISGeographicTransformation(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISDatumTransformationType.HorizontalVerticalTransformation:
                        localLocalResult = new ArcGISHorizontalVerticalTransformation(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISDatumTransformation(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the best usable transformation used to transform between the input and output spatial references,
        /// taking into account the area of interest, if specified. Optionally disregards any vertical transformations.
        /// </summary>
        /// <remarks>
        /// Use this method to determine whether or not any vertical coordinate systems set on the
        /// spatial reference parameters should be accounted for in the returned transformation.
        /// This method can be used to replicate the former (prior to version 100.9.0) behavior of
        /// the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see> and
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>
        /// methods:
        /// * Calling this method with 'ignoreVertical = false' is equivalent to calling
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>
        /// * Calling this method with 'ignoreVertical = true' is equivalent to calling
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformation">ArcGISTransformationCatalog.GetTransformation</see>
        /// in releases prior to 100.9.0
        /// </remarks>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <param name="areaOfInterest">The bounding box of coordinates to be transformed, or null to consider the entire world extent.</param>
        /// <param name="ignoreVertical">True if <see cref="GameEngine.Geometry.ArcGISTransformationCatalog">ArcGISTransformationCatalog</see> should ignore any vertical coordinate system set on the  inputSpatialReference or outputSpatialReference, and only consider horizontal (geographic)  transformations; false otherwise.</param>
        /// <returns>
        /// A <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> instance that represents the best choice given the parameters. Always returns a
        /// usable transformation where <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is false. The specific
        /// type returned depends on the given value of the ignoreVertical parameter.
        /// 
        /// Returns null if no transformation is required for the given input parameters, or if no usable transformation
        /// is available.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISDatumTransformation GetTransformation(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference, ArcGISEnvelope areaOfInterest, bool ignoreVertical)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            var localAreaOfInterest = areaOfInterest == null ? System.IntPtr.Zero : areaOfInterest.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformationWithAreaOfInterestAndIgnoreVertical(localInputSpatialReference, localOutputSpatialReference, localAreaOfInterest, ignoreVertical, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISDatumTransformation localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_DatumTransformation_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISDatumTransformationType.GeographicTransformation:
                        localLocalResult = new ArcGISGeographicTransformation(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISDatumTransformationType.HorizontalVerticalTransformation:
                        localLocalResult = new ArcGISHorizontalVerticalTransformation(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISDatumTransformation(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns a list representing the datum transformations suitable for projecting
        /// between spatial references without taking into account an area of interest.
        /// </summary>
        /// <remarks>
        /// The list is ordered in descending order by suitability, with the most suitable being first in the list.
        ///             
        /// Some transformations require a supporting Projection Engine (PE) dataset to function correctly, and ArcGIS
        /// Runtime must be able to locate these datasets. If the datasets cannot be located, the transformation is not
        /// currently usable, and <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is true. The list may include
        /// transformations like this. To use such transformations, ensure
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see> is set correctly, and that the required dataset is
        /// available within that location. Use <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</see> and
        /// <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</see> to determine the dataset required for a
        /// specific transformation instance.
        /// </remarks>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISIntermediateImmutableArray<T>">ArcGISIntermediateImmutableArray<T></see> containing <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> objects, each suitable for transforming between the given
        /// input and output spatial references.
        /// 
        /// Prior to version 100.9, this method only considered geographic coordinate systems, and the returned array
        /// of DatumTransformation objects were of the subtype <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see>. A geographic 
        /// transformation is not required when input and output spatial references have the same underlying geographic
        /// coordinate system, in which case an empty list was returned.
        /// 
        /// From version 100.9 onwards, if both inputSpatialReference and outputSpatialReference have a vertical 
        /// coordinate system, this method returns a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> instance. If either 
        /// SpatialReference does not have a vertical coordinate system, a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> is returned.
        /// A geographic transformation is not needed when input and output spatial references have the same underlying
        /// geographic (horizontal) coordinate system. A vertical transformation is not needed if both datums (for 
        /// ellipsoidal heights) or vertical datums (for gravity-related heights) are the same. If neither type of 
        /// transformation is needed, an empty collection is returned.
        /// To replicate the former behavior, use the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// method instead with 'ignoreVertical = true'.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISDatumTransformation> GetTransformationsBySuitability(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformationsBySuitability(localInputSpatialReference, localOutputSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISDatumTransformation> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISDatumTransformation>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns a list representing the datum transformations suitable for projecting
        /// between spatial references and takes into account the area of interest.
        /// </summary>
        /// <remarks>
        /// The list is ordered in descending order by suitability, with the most suitable being first in the list. The
        /// given area of interest can affect the number and order of transformations returned.
        /// 
        /// Some transformations require a supporting Projection Engine (PE) dataset to function correctly, and ArcGIS
        /// Runtime must be able to locate these datasets. If the datasets cannot be located, the transformation is not
        /// currently usable, and <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is true. The list may include
        /// transformations like this. To use such transformations, ensure
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see> is set correctly, and that the required dataset is
        /// available within that location. Use <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</see> and
        /// <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</see> to determine the dataset required for a
        /// specific transformation instance.
        /// </remarks>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <param name="areaOfInterest">The bounding box of coordinates to be transformed, or null to consider the entire world extent.</param>
        /// <returns>
        /// An <see cref="Standard.ArcGISIntermediateImmutableArray<T>">ArcGISIntermediateImmutableArray<T></see> containing <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> objects, each suitable for transforming between the given
        /// input and output spatial references, taking in to account the given area of interest.
        /// 
        /// Prior to version 100.9, this method only considered geographic coordinate systems, and the returned array
        /// of DatumTransformation objects were of the subtype <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see>. A geographic 
        /// transformation is not required when input and output spatial references have the same underlying geographic
        /// coordinate system, in which case an empty list was returned.
        /// 
        /// From version 100.9 onwards, if both inputSpatialReference and outputSpatialReference have a vertical 
        /// coordinate system, this method returns a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> instance. If either 
        /// SpatialReference does not have a vertical coordinate system, a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> is returned.
        /// A geographic transformation is not needed when input and output spatial references have the same underlying
        /// geographic (horizontal) coordinate system. A vertical transformation is not needed if both datums (for 
        /// ellipsoidal heights) or vertical datums (for gravity-related heights) are the same. If neither type of 
        /// transformation is needed, an empty collection is returned.
        /// To replicate the former behavior, use the <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// method instead with 'ignoreVertical = true'.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISDatumTransformation> GetTransformationsBySuitability(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference, ArcGISEnvelope areaOfInterest)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            var localAreaOfInterest = areaOfInterest == null ? System.IntPtr.Zero : areaOfInterest.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformationsBySuitabilityWithAreaOfInterest(localInputSpatialReference, localOutputSpatialReference, localAreaOfInterest, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISDatumTransformation> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISDatumTransformation>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns a collection of transformations suitable for projecting between the input and output spatial 
        /// references, taking into account the area of interest, if specified. Optionally checks for suitable 
        /// vertical transformations.
        /// </summary>
        /// <remarks>
        /// The collection is ordered in descending order by suitability, with the most suitable being first in the
        /// list. The given area of interest can affect the number and order of transformations returned.
        /// 
        /// A geographic transformation is not needed when input and output spatial references
        /// have the same underlying geographic coordinate system, in which case an empty list
        /// is returned. A vertical transformation is not needed if both datums (for ellipsoidal
        /// heights) or vertical datums (for gravity-related heights) are the same. If neither
        /// type of transformation is needed, an empty collection is returned.
        /// 
        /// Use this method to determine whether or not any vertical coordinate systems set on the
        /// spatial reference parameters should be accounted for in the returned list of
        /// transformations. This method can be used to replicate the former (prior to version
        /// 100.9.0) behavior of the
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// and
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// methods:
        /// * Calling this method with 'ignoreVertical = false' is equivalent to calling
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// * Calling this method with 'ignoreVertical = true' is equivalent to calling
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.GetTransformationsBySuitability">ArcGISTransformationCatalog.GetTransformationsBySuitability</see>
        /// in releases prior to 100.9.0
        /// 
        /// Some transformations require a supporting Projection Engine (PE) dataset to function correctly, and ArcGIS
        /// Runtime must be able to locate these datasets. If the datasets cannot be located, the transformation is not
        /// currently usable, and <see cref="GameEngine.Geometry.ArcGISDatumTransformation.IsMissingProjectionEngineFiles">ArcGISDatumTransformation.IsMissingProjectionEngineFiles</see> is true. The list may include
        /// transformations like this. To use such transformations, ensure
        /// <see cref="GameEngine.Geometry.ArcGISTransformationCatalog.ProjectionEngineDirectory">ArcGISTransformationCatalog.ProjectionEngineDirectory</see> is set correctly, and that the required dataset is
        /// available within that location. Use <see cref="GameEngine.Geometry.ArcGISGeographicTransformationStep.ProjectionEngineFilenames">ArcGISGeographicTransformationStep.ProjectionEngineFilenames</see> and
        /// <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames">ArcGISHorizontalVerticalTransformationStep.ProjectionEngineFilenames</see> to determine the dataset required for a
        /// specific transformation instance.
        /// </remarks>
        /// <param name="inputSpatialReference">The spatial reference to use as the input.</param>
        /// <param name="outputSpatialReference">The spatial reference to use as the output.</param>
        /// <param name="areaOfInterest">The bounding box of coordinates to be transformed, or null to consider the entire world extent.</param>
        /// <param name="ignoreVertical">True if <see cref="GameEngine.Geometry.ArcGISTransformationCatalog">ArcGISTransformationCatalog</see> should ignore any vertical coordinate systems set on the  inputSpatialReference or outputSpatialReference, and only consider horizontal (geographic)  transformations; false otherwise.</param>
        /// <returns>
        /// A collection of <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> objects suitable for the given parameters, ordered by suitability,
        /// or an empty collection if no transformation is required.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISDatumTransformation> GetTransformationsBySuitability(ArcGISSpatialReference inputSpatialReference, ArcGISSpatialReference outputSpatialReference, ArcGISEnvelope areaOfInterest, bool ignoreVertical)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localInputSpatialReference = inputSpatialReference.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            var localAreaOfInterest = areaOfInterest == null ? System.IntPtr.Zero : areaOfInterest.Handle;
            
            var localResult = PInvoke.RT_TransformationCatalog_getTransformationsBySuitabilityWithAreaOfInterestAndIgnoreVertical(localInputSpatialReference, localOutputSpatialReference, localAreaOfInterest, ignoreVertical, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISDatumTransformation> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISDatumTransformation>(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getProjectionEngineDirectory(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_TransformationCatalog_setProjectionEngineDirectory([MarshalAs(UnmanagedType.LPStr)]string projectionEngineDirectory, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformation(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformationWithAreaOfInterest(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr areaOfInterest, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformationWithAreaOfInterestAndIgnoreVertical(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr areaOfInterest, [MarshalAs(UnmanagedType.I1)]bool ignoreVertical, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformationsBySuitability(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformationsBySuitabilityWithAreaOfInterest(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr areaOfInterest, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_TransformationCatalog_getTransformationsBySuitabilityWithAreaOfInterestAndIgnoreVertical(IntPtr inputSpatialReference, IntPtr outputSpatialReference, IntPtr areaOfInterest, [MarshalAs(UnmanagedType.I1)]bool ignoreVertical, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}