using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FingerJoint
{
    JOINT_MCP = 0,
    JOINT_PIP = 1,
    JOINT_DIP = 2,
    JOINT_TIP = 3,
    JOINT_UNKNOWN = -1
}

public enum BoneType
{
    TYPE_METACARPAL = 0,
    TYPE_PROXIMAL = 1,
    TYPE_INTERMEDIATE = 2,
    TYPE_DISTAL = 3,
    TYPE_UNKNOWN  = -1
}
