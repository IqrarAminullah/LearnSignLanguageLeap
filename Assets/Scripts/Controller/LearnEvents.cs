using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnEvents : MonoBehaviour
{
    public delegate void DisplayEndScreenCallback(int unknownCount, int knownCount);
    public DisplayEndScreenCallback DisplayEndScreen;

    public delegate void ScoreUpdateCallback(int score);
    public ScoreUpdateCallback ScoreUpdate;
}
