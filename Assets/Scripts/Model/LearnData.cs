using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnData
{
    #region attributes
    List<HandSign> trainingSigns { get; set; }
    public int signCount { get; set; }
    public List<HandSign> unknownFlashcards { get; set; }
    public List<HandSign> knownFlashcards { get; set; }
    public List<HandSign> masteredFlashcards { get; set; }
    private JSONIO jsonUtility;
    public int currentSignIdx { get; set; }
    public HandSign currentSign { get; set; }
    #endregion

    #region public methods
    public LearnData()
    {
        jsonUtility = new JSONIO();
        trainingSigns = new List<HandSign>();
        signCount = trainingSigns.Count;
        unknownFlashcards = new List<HandSign>();
        knownFlashcards = new List<HandSign>();
        masteredFlashcards = new List<HandSign>();
    }

    public void LoadData(string filename)
    {
        trainingSigns = jsonUtility.LoadJSON<List<HandSign>>(filename);
        signCount = trainingSigns.Count;

        unknownFlashcards = new List<HandSign>();
        knownFlashcards = new List<HandSign>();
        masteredFlashcards = new List<HandSign>();
    }
    public void UpgradeCurrentSign()
    {
        if (!masteredFlashcards.Contains(currentSign))
        {
            //if previously unknown, remove from unknown
            if (unknownFlashcards.Contains(currentSign))
            {
                Debug.Log(currentSign.sign_name);
                unknownFlashcards.Remove(currentSign);
            }
            //search for sign in known list
            int idx = knownFlashcards.IndexOf(currentSign);
            //if previously known, remove from unknown, add to master, remove from sign list
            if (idx >= 0)
            {
                masteredFlashcards.Add(knownFlashcards[idx]);
                knownFlashcards.Remove(currentSign);
            }
            else
            {
                knownFlashcards.Add(currentSign);
            }
        }
    }

    public void DowngradeCurrentSign()
    {
        //if previously mastered, remove one step to known
        if (masteredFlashcards.Contains(currentSign))
        {
            masteredFlashcards.Remove(currentSign);
            knownFlashcards.Add(currentSign);
        }
        else
        {
            //if previously known, remove from known list
            if (knownFlashcards.Contains(currentSign))
            {
                knownFlashcards.Remove(currentSign);
            }
            //if current sign not in unknown list, add to unknown list
            if (!unknownFlashcards.Contains(currentSign))
            {
                unknownFlashcards.Add(currentSign);
            }
        }
    }

    public void SetCurrentSign(int idx)
    {
        currentSign = trainingSigns[idx];
    }
    #endregion
}
