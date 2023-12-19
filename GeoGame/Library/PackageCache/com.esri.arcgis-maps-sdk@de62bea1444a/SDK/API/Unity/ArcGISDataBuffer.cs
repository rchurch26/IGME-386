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
using System.Runtime.InteropServices;

namespace Esri.Unity
{
	[StructLayout(LayoutKind.Sequential)]
	public class ArcGISDataBuffer<T> where T : struct
	{
		private GameEngine.ArcGISIntermediateDataBuffer<T> intermediateDataBuffer;

		internal IntPtr Handle
		{
			get
			{
				return intermediateDataBuffer.Handle;
			}
			set
			{
				intermediateDataBuffer.Handle = value;
			}
		}

		internal ulong ItemSize
		{
			get
			{
				return intermediateDataBuffer.ItemSize;
			}
		}

		internal ArcGISDataBuffer(IntPtr handle)
		{
			intermediateDataBuffer = new GameEngine.ArcGISIntermediateDataBuffer<T>(handle);
		}

		internal IntPtr Data
		{
			get
			{
				return intermediateDataBuffer.Data;
			}
		}

		public ulong SizeBytes
		{
			get
			{
				return intermediateDataBuffer.SizeBytes;
			}
		}

		public static bool operator ==(ArcGISDataBuffer<T> lhs, ArcGISDataBuffer<T> rhs)
		{
			IntPtr lhsHandle = (object)lhs == null ? IntPtr.Zero : lhs.Handle;
			IntPtr rhsHandle = (object)rhs == null ? IntPtr.Zero : rhs.Handle;

			return lhsHandle == rhsHandle;
		}

		public static bool operator !=(ArcGISDataBuffer<T> lhs, ArcGISDataBuffer<T> rhs)
		{
			IntPtr lhsHandle = (object)lhs == null ? IntPtr.Zero : lhs.Handle;
			IntPtr rhsHandle = (object)rhs == null ? IntPtr.Zero : rhs.Handle;

			return lhsHandle != rhsHandle;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISDataBuffer<T> buffer && (ArcGISDataBuffer<T>)obj == this;
		}

		public override int GetHashCode()
		{
			int hashCode = 732208100;
			hashCode = hashCode * -1521134295 + Handle.GetHashCode();
			return hashCode;
		}
	}
}
