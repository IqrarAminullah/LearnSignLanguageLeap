using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InverseWeightedList<T>
{
    #region attributes
    private Dictionary<T, double> weightedDict = new Dictionary<T, double>();
    private double accumulatedWeight;
    System.Random rand = new System.Random();
    #endregion

    #region public methods
    public void AddObject(T obj, double weight)
    {
        accumulatedWeight = weight + accumulatedWeight;
        weightedDict.Add(obj, accumulatedWeight);
    }

    public T getRandom()
    {
        double r = rand.NextDouble() * accumulatedWeight;
        foreach(KeyValuePair<T,double> entry in weightedDict)
        {
            if (entry.Value >= r)
            {
                return entry.Key;
            }
        }
        return default(T); //should only happen when there are no entries
    }
    #endregion
}

