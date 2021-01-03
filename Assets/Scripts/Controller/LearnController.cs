﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnController : MonoBehaviour
{
    #region attributes
    List<HandSign> trainingSigns { get; set; }
    int signCount;

    private List<HandSign> unknownFlashcards { get; set; }
    private List<HandSign> knownFlashcards { get; set; }
    private List<HandSign> masteredFlashcards { get; set; }

    private InverseWeightedList<HandSign> weightedUnknownFlashcards;
    [SerializeField]
    const float distanceThreshold = 3;
    float distanceScore;

    [SerializeField]
    GestureRecognition classifier;
    [SerializeField]
    LearnRenderer viewRenderer;

    private JSONIO jsonUtility;
    public bool debugMode;
    public bool randomized;

    private int currentSignIdx;
    private HandSign currentSign;
    public bool isCalculating;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
        if (debugMode)
        {
            DebugTraining();
        }
        RunFlashcards(AppData.LoadFilePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCalculating)
        {
            StartCoroutine("CalculateScore");
        }
    }
    void DebugTraining()
    {
        List<HandSign> s = new List<HandSign>();
        s.Add(new HandSign("SIBI_A", "Huruf A", "Sprites/SIBI_A", "Sprites/Huruf_A", "Huruf A dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_B", "Huruf B", "Sprites/SIBI_B", "Sprites/Huruf_B", "Huruf B dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf C", "Sprites/SIBI_C", "Sprites/Huruf_C", "Huruf C dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_D", "Huruf D", "Sprites/SIBI_D", "Sprites/Huruf_D", "Huruf D dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_E", "Huruf E", "Sprites/SIBI_E", "Sprites/Huruf_E", "Huruf E dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_F", "Huruf F", "Sprites/SIBI_F", "Sprites/Huruf_F", "Huruf F dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_G", "Huruf G", "Sprites/SIBI_G", "Sprites/Huruf_G", "Huruf G dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_H", "Huruf H", "Sprites/SIBI_H", "Sprites/Huruf_H", "Huruf H dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_I", "Huruf I", "Sprites/SIBI_I", "Sprites/Huruf_I", "Huruf I dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_J", "Huruf J", "Sprites/SIBI_J", "Sprites/Huruf_J", "Huruf J dalam SIBI", GestureType.Dynamic));
        s.Add(new HandSign("SIBI_K", "Huruf K", "Sprites/SIBI_K", "Sprites/Huruf_K", "Huruf K dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_L", "Huruf L", "Sprites/SIBI_L", "Sprites/Huruf_L", "Huruf L dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_M", "Huruf M", "Sprites/SIBI_M", "Sprites/Huruf_M", "Huruf M dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_N", "Huruf N", "Sprites/SIBI_N", "Sprites/Huruf_N", "Huruf N dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_O", "Huruf O", "Sprites/SIBI_O", "Sprites/Huruf_O", "Huruf O dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_P", "Huruf P", "Sprites/SIBI_P", "Sprites/Huruf_P", "Huruf P dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_Q", "Huruf Q", "Sprites/SIBI_Q", "Sprites/Huruf_Q", "Huruf Q dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_R", "Huruf R", "Sprites/SIBI_R", "Sprites/Huruf_R", "Huruf R dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_S", "Huruf S", "Sprites/SIBI_S", "Sprites/Huruf_S", "Huruf S dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_T", "Huruf T", "Sprites/SIBI_T", "Sprites/Huruf_T", "Huruf T dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_U", "Huruf U", "Sprites/SIBI_U", "Sprites/Huruf_U", "Huruf U dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_V", "Huruf V", "Sprites/SIBI_V", "Sprites/Huruf_V", "Huruf V dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_W", "Huruf W", "Sprites/SIBI_W", "Sprites/Huruf_W", "Huruf W dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_X", "Huruf X", "Sprites/SIBI_X", "Sprites/Huruf_X", "Huruf X dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_Y", "Huruf Y", "Sprites/SIBI_Y", "Sprites/Huruf_Y", "Huruf Y dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_Z", "Huruf Z", "Sprites/SIBI_Z", "Sprites/Huruf_Z", "Huruf Z dalam SIBI", GestureType.Dynamic));

        jsonUtility.SaveJson("Flashcard/Alfabet.json", s);

        RunFlashcards("Flashcard/Alfabet.json");
    }
    private void Init()
    {
        trainingSigns = new List<HandSign>();
        if(jsonUtility == null)
        {
            jsonUtility = new JSONIO();
        }
    }

    #region public methods
    public void RunFlashcardsWeighted(string filename)
    {
        weightedUnknownFlashcards = new InverseWeightedList<HandSign>();
        List<HandSign> handSigns = jsonUtility.LoadJSON<List<HandSign>>(filename);

        foreach (HandSign h in handSigns)
        {
            double randomWeight = Random.Range(1, 10);
            Debug.Log(h.sign_name + " " + randomWeight);
            weightedUnknownFlashcards.AddObject(h, randomWeight);
        }

        for (int i = 0; i <= 5000; i++)
        {
            Debug.Log(weightedUnknownFlashcards.getRandom());
        }

    }
    public void LoadFlashcards(string file)
    {
        trainingSigns = jsonUtility.LoadJSON<List<HandSign>>(file);
        signCount = trainingSigns.Count;
        unknownFlashcards = new List<HandSign>();
        knownFlashcards = new List<HandSign>();
        masteredFlashcards = new List<HandSign>();
    }

    public void RunFlashcards(string file)
    {
        Init();
        LoadFlashcards(file);

        currentSignIdx = -1;
        NextSign();

        classifier.LoadDatabase("lgr_db.sqlite");
        classifier.StartClassifier();
    }

    public void NextSign()
    {
        Debug.Log(unknownFlashcards.Count + "," + knownFlashcards.Count + "," +masteredFlashcards.Count);
        currentSignIdx = currentSignIdx + 1;
        if (currentSignIdx < signCount)
        {
            viewRenderer.UpdatePageText(currentSignIdx, trainingSigns.Count);
            viewRenderer.UpdateFlashcard(trainingSigns[currentSignIdx]);
            currentSign = trainingSigns[currentSignIdx];
            if (!classifier.Active)
            {
                classifier.StartClassifier();
            }
            if (trainingSigns[currentSignIdx].type == GestureType.Static)
            {
                classifier.ChangeToStaticMode();
            }
            else
            {
                classifier.ChangeToDynamicMode();
            }
        }
        else
        {
            viewRenderer.GoToEndScreen(unknownFlashcards.Count,knownFlashcards.Count, masteredFlashcards.Count);
        }
    }


    public void SetSign(HandSign sign)
    {
        viewRenderer.UpdateFlashcard(sign);
    }

    public void ReLearn()
    {
        Debug.Log("Relear");
        currentSignIdx = -1;
        NextSign();
    }
    public void KnownNext()
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
        NextSign();
    }

    public void UnknownNext()
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
        NextSign();
    }


    IEnumerator CalculateScore()
    {
        isCalculating = true;
        distanceScore = Mathf.Infinity;
        float deltaTime = 0f;
        bool finish = false;
        while (classifier.Active && !finish)
        {
            if(classifier.Mode == GestureType.Static)
            {
                distanceScore = classifier.GetGestureDistance(currentSign.sign_name);
                viewRenderer.UpdateScore(distanceScore);
                if (distanceScore != Mathf.Infinity)
                {
                    if (distanceScore <= distanceThreshold)
                    {
                        deltaTime = deltaTime + Time.time;
                        if (deltaTime > 2.0f)
                        {
                            viewRenderer.EnableKnowButton();
                            finish = true;
                        }
                        isCalculating = false;
                    }
                }
            }
            else
            {
                if (classifier.FinishRecordingDynamicGesture())
                {
                    distanceScore = classifier.GetGestureDistance(currentSign.sign_name);
                    viewRenderer.UpdateScore(distanceScore);
                    if (distanceScore != Mathf.Infinity)
                    {
                        if (distanceScore <= distanceThreshold)
                        {
                            deltaTime = deltaTime + Time.time;
                            if (deltaTime > 2.0f)
                            {
                                viewRenderer.EnableKnowButton();
                                finish = true;
                            }
                            isCalculating = false;
                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion
}