using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

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
		public Dictionary<SGClassWrapper, float> GetDistancesFromAllClasses(SGInstance gestureInstance)
		{
			lock (_staticGestureClasses)
			{
				var gestureDistances = new Dictionary<SGClassWrapper, float>();
				foreach (var gestureClass in _staticGestureClasses)
				{
					gestureDistances.Add(gestureClass, gestureClass.Gesture.DistanceTo(gestureInstance));
				}
				return gestureDistances;
			}
		}

		public Dictionary<DGClassWrapper, float> GetDistancesFromAllClasses(DGInstance gestureInstance)
		{
			lock (_dynamicGestureClasses)
			{
				var gestureDistances = new Dictionary<DGClassWrapper, float>();
				foreach (var gestureClass in _dynamicGestureClasses)
				{
					gestureDistances.Add(gestureClass, gestureClass.Gesture.DistanceTo(gestureInstance, UseDTW));
				}
				return gestureDistances;
			}
		}
		#endregion

	}
}
