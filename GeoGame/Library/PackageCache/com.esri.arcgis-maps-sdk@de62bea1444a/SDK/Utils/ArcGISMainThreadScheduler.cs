// COPYRIGHT 1995-2021 ESRI
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils
{
	[ExecuteAlways]
	public class ArcGISMainThreadScheduler : MonoBehaviour
	{
		private static readonly Queue<Action> actions = new Queue<Action>();

		private static ArcGISMainThreadScheduler instance = null;
		private static readonly object instanceLock = new object();
		private static GameObject mainThreadGameObject = null;

		void Awake()
		{
			if (instance == null)
			{
				instance = this;

				if (Application.isPlaying)
				{
					DontDestroyOnLoad(this.gameObject);
				}
			}
		}

		IEnumerator ConvertToCoroutine(Action action)
		{
			action();

			yield return null;
		}

		public static ArcGISMainThreadScheduler Instance()
		{
			if (instance == null)
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						mainThreadGameObject = new GameObject();
						mainThreadGameObject.name = "ArcGISMainThreadScheduler";
						mainThreadGameObject.hideFlags = HideFlags.HideAndDontSave;

						mainThreadGameObject.AddComponent<ArcGISMainThreadScheduler>();
					}
				}
			}

			return instance;
		}

		void OnDestroy()
		{
			instance = null;
			GameObject.Destroy(mainThreadGameObject);
		}

		public void Schedule(IEnumerator action)
		{
			lock (actions)
			{
				actions.Enqueue(() =>
				{
					StartCoroutine(action);
				});
			}
		}

		public void Schedule(Action action)
		{
			Schedule(ConvertToCoroutine(action));
		}

		public void Update()
		{
			lock (actions)
			{
				while (actions.Count > 0)
				{
					actions.Dequeue().Invoke();
				}
			}
		}
	}
}
