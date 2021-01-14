using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LeapGestureRecognition
{
	public class StatisticalClassifier
	{
		private List<SGClassWrapper> _staticGestureClasses;
		private List<DGClassWrapper> _dynamicGestureClasses;

		// If true, use DTW for DG distance calculation. If false, use lerp distance.
		public bool UseDTW { get; set; }

		public StatisticalClassifier(List<SGClassWrapper> staticGestureClasses, 
			List<DGClassWrapper> dynamicGestureClasses) 
		{
			_staticGestureClasses = staticGestureClasses;
			_dynamicGestureClasses = dynamicGestureClasses;
			UseDTW = false;
		}

		#region Public Properties
		public List<SGClassWrapper> StaticGestureClasses
		{
			get { return _staticGestureClasses; }
			set { _staticGestureClasses = value; }
		}

		public List<DGClassWrapper> DynamicGestureClasses
		{
			get { return _dynamicGestureClasses; }
			set { _dynamicGestureClasses = value; }
		}
		#endregion

		#region Public Methods
		public Dictionary<string, float> GetDistancesFromAllClasses(SGInstance gestureInstance)
		{
			lock (_staticGestureClasses)
			{
				var gestureDistances = new Dictionary<string, float>();
				foreach (var gestureClass in _staticGestureClasses)
				{
					gestureDistances.Add(gestureClass.Name, gestureClass.Gesture.DistanceTo(gestureInstance));
				}
				return gestureDistances;
			}
		}

		public Dictionary<string, float> GetDistancesFromAllClasses(DGInstance gestureInstance)
		{
			lock (_dynamicGestureClasses)
			{
				var gestureDistances = new Dictionary<string, float>();
				foreach (var gestureClass in _dynamicGestureClasses)
				{
					//float distance = gestureClass.Gesture.DistanceTo(gestureInstance, UseDTW);
					gestureDistances.Add(gestureClass.Name, gestureClass.Gesture.DistanceTo(gestureInstance, UseDTW));
				}
				return gestureDistances;
			}
		}
		#endregion

	}
}
