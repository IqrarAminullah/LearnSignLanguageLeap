﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using LeapGestureRecognition;
using Leap;

public class GestureRecognition
{
    #region attributes
    public Controller leapController;
    public StatisticalClassifier Classifier { get; set; }
    public Dictionary<string,float> RankedStaticGestures { get; set; }
    public Dictionary<string, float> RankedDynamicGestures { get; set; }
    public DGRecorder DgRecorder { get; set; }
    public DGRecorderState CurrentState { get; set; }
    public GestureType Mode { get; set; }
    public bool Active { get; set; }
    public SQLiteProvider sqliteProvider { get; set; }
    private string currrentDB;
    List<SGClassWrapper> StaticGestures;
    List<DGClassWrapper> DynamicGestures;
    Thread classifierThread;
    #endregion

    #region public methods
    public GestureRecognition()
    {
        leapController = new Controller();
        DgRecorder = new DGRecorder(inRecordMode: false);
        CurrentState = DgRecorder.State;
        RankedStaticGestures = new Dictionary<string, float>();
        RankedDynamicGestures = new Dictionary<string, float>();
        Mode = GestureType.Static; // Probably not necessary to initialize
        Active = false;
        classifierThread = new Thread(GestureClassifyThread);
    }

    public void LoadDatabase(string databaseName)
    {

        sqliteProvider = new SQLiteProvider(databaseName);
        StaticGestures = sqliteProvider.GetAllStaticGestureClasses();
        DynamicGestures = sqliteProvider.GetAllDynamicGestureClasses();

        Classifier = new StatisticalClassifier(StaticGestures, DynamicGestures);
    }

    public void ProcessFrame(Frame frame)
    {
        if(Classifier != null)
        {
            if (Mode == GestureType.Static)
            {
                RankedStaticGestures = Classifier.GetDistancesFromAllClasses(new SGInstance(frame));
                //List<GestureDistance> distanceList = new List<GestureDistance>(distances.OrderBy(g => g.Value).Select(g => new GestureDistance(g.Key.Name, g.Value)));
                //RankedStaticGestures = distanceList.ToDictionary(x => x.Name, x => x.Distance);
            }
            else
            {
                DgRecorder.ProcessFrame(frame);
                CurrentState = DgRecorder.State;
                switch (CurrentState)
                {
                    case DGRecorderState.RecordingJustFinished:
                        if (DgRecorder.MostRecentInstance.Samples.Count == 0) break;

                        RankedDynamicGestures = Classifier.GetDistancesFromAllClasses(DgRecorder.MostRecentInstance);
                        // RankedDynamicGestures = new List<GestureDistance>(distances.OrderBy(g => g.Value).Select(g => new GestureDistance(g.Key.Name, g.Value)));
                        break;
                }
            }
        }
    }

    

    public void ChangeToStaticMode()
    {
        Mode = GestureType.Static;
    }

    public void ChangeToDynamicMode()
    {
        Mode = GestureType.Dynamic;
    }

    public void StartClassifier()
    {

        Active = true;
        classifierThread.Start();
        //StartCoroutine(GestureClassify());
    }

    public void StopClassifier()
    {
        Active = false;
        classifierThread.Join();
        //StopCoroutine(GestureClassify());
    }

    IEnumerator GestureClassify()
    {
        while (Active)
        {
            Frame frame = leapController.Frame();
            ProcessFrame(frame);
            //Debug.Log(RankedDynamicGestures)
            //Debug.Log(string.Format("{0},{1}", RankedStaticGestures[0].Name,RankedStaticGestures[0].Distance));
            yield return null;
        }
        yield return null;
    }

    public void GestureClassifyThread()
    {
        while (Active)
        {
            if(leapController == null)
            {
                leapController = new Controller();
            }
            Frame frame = leapController.Frame();
            if(frame != null)
            {
                ProcessFrame(frame);
            }
        }
    }
    public float GetGestureDistance(string gestureName)
    {
        float f;
        if(Mode == GestureType.Static)
        {
            if (RankedStaticGestures.TryGetValue(gestureName, out f))
            {
                return f;
            }
            else
            {
                return Mathf.Infinity;
            };
        }
        else
        {
            if (RankedDynamicGestures.TryGetValue(gestureName, out f))
            {
                Debug.Log(gestureName + " " + f);
                return f;
            }
            else
            {
                return Mathf.Infinity;
            };
        }
    }

    public string GetClosestGesture()
    {
        string res = "";
        if(Mode == GestureType.Static)
        {
            //res = RankedStaticGestures[0].Name;
        }
        else
        {
            //res = RankedDynamicGestures[0].Name;
        }
        return res;
    }

    public bool HasFinishedRecordingDynamicGesture()
    {
        return CurrentState == DGRecorderState.InEndPosition;
    }


    #endregion
}
