using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using LeapGestureRecognition;
using Leap;

public class GestureRecognition
{
    #region attributes
    public StatisticalClassifier Classifier { get; set; }
    public List<GestureDistance> RankedStaticGestures { get; set; }
    public List<GestureDistance> RankedDynamicGestures { get; set; }
    public DGRecorder DgRecorder { get; set; }
    public DGRecorderState CurrentState { get; set; }
    public GestureType Mode { get; set; }
    public bool Active { get; set; }
    public SQLiteProvider sqliteProvider { get; set; }
    #endregion

    #region public methods
    public GestureRecognition()
    {
        List<SGClassWrapper> StaticGestures = new List<SGClassWrapper>();//sqliteProvider.GetAllStaticGestureClasses();
        List<DGClassWrapper> DynamicGestures = new List<DGClassWrapper>();//sqliteProvider.GetAllDynamicGestureClasses();
        Classifier = new StatisticalClassifier(StaticGestures, DynamicGestures);
        DgRecorder = new DGRecorder(inRecordMode: false);
        CurrentState = DgRecorder.State;
        RankedStaticGestures = new List<GestureDistance>();
        RankedDynamicGestures = new List<GestureDistance>();
        Mode = GestureType.Static; // Probably not necessary to initialize
        Active = true;
    }

    public void ProcessFrame(Frame frame)
    {
        if (Mode == GestureType.Static)
        {
            var distances = Classifier.GetDistancesFromAllClasses(new SGInstance(frame));
            RankedStaticGestures = new List<GestureDistance>(distances.OrderBy(g => g.Value).Select(g => new GestureDistance(g.Key.Name, g.Value)));
        }
        else
        {
            DgRecorder.ProcessFrame(frame);
            CurrentState = DgRecorder.State;
            switch (CurrentState)
            {
                case DGRecorderState.RecordingJustFinished:
                    if (DgRecorder.MostRecentInstance.Samples.Count == 0) break;

                    var distances = Classifier.GetDistancesFromAllClasses(DgRecorder.MostRecentInstance);
                    RankedDynamicGestures = new List<GestureDistance>(distances.OrderBy(g => g.Value).Select(g => new GestureDistance(g.Key.Name, g.Value)));
                    break;
            }
        }
    }
    #endregion
}
