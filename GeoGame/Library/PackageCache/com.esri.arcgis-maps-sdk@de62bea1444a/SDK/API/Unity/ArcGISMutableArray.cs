// COPYRIGHT 1995-2020 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com

using System;

namespace Esri.Unity
{
	public class ArcGISMutableArray
	{
		#region Internal Members
		public ArcGISMutableArray(IntPtr handle)
		{
			intermediateMutableArray = new Standard.ArcGISIntermediateMutableArray<object>(handle);
		}

		internal IntPtr Handle
		{
			get
			{
				if (intermediateMutableArray != null)
				{
					return intermediateMutableArray.Handle;
				}

				return IntPtr.Zero;
			}
			set
			{
				if (intermediateMutableArray != null)
				{
					intermediateMutableArray.Handle = value;
				}
			}
		}

		private Standard.ArcGISIntermediateMutableArray<object> intermediateMutableArray;
		#endregion // Internal Members
	}

	public class ArcGISMutableArray<T>
	{
		#region Internal Members
		public ArcGISMutableArray(IntPtr handle)
		{
			intermediateMutableArray = new Standard.ArcGISIntermediateMutableArray<T>(handle);
		}

		internal IntPtr Handle
		{
			get
			{
				if (intermediateMutableArray != null)
				{
					return intermediateMutableArray.Handle;
				}

				return IntPtr.Zero;
			}
			set
			{
				if (intermediateMutableArray != null)
				{
					intermediateMutableArray.Handle = value;
				}
			}
		}

		private Standard.ArcGISIntermediateMutableArray<T> intermediateMutableArray;
		#endregion // Internal Members
	}
}
