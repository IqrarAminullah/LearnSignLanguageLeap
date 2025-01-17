﻿using Leap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LeapGestureRecognition
{
	[DataContract]
	public class Feature
	{
		public Feature() { }

		public Feature(FeatureName name, object value, int weight = 1)
		{
			Name = name;
			Value = value;
			Weight = weight;
		}

		[DataMember]
		public FeatureName Name { get; set; }
		[DataMember]
		public object Value { get; set; }
		[DataMember]
		public int Weight { get; set; }
	}

}
