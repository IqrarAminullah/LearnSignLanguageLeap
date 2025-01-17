﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeapGestureRecognition
{
	public enum FeatureName
	{
		// Left
		// Hand specific
		LeftPalmPosition,
		LeftYaw,
		LeftPitch,
		LeftRoll,
		LeftPalmSphereRadius,
		LeftPalmSphereCenter,
		// Finger specific
		LeftFingerTipPositions,
		LeftFingersExtended,


		// Right
		// Hand specific
		RightPalmPosition,
		RightYaw,
		RightPitch,
		RightRoll,
		RightPalmSphereRadius,
		RightPalmSphereCenter,
		// Finger specific
		RightFingerTipPositions,
		RightFingersExtended,

		// Position of hands relative to each other (LeftHandPosition - RightHandPosition)
		LeftToRightHand,
		HandConfiguration
	};
}
