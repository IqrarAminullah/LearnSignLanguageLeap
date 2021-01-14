using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingController : MonoBehaviour
{
    #region private attributes

    [Header("Variables")]
    [SerializeField]
    const float distanceThreshold = 3;
    [SerializeField]
    const float quizTimer = 120f;
    float timeRemaining;
    float distanceScore;

    Flashcard flashcard { get; set; }

    

    private bool skip;
    private bool inPause;
    private int quizScore;
    private int quizQuestionNumber;
    private int currentSignIdx;

    private TrainingData dataModel;
    [Header("References")]
    GestureRecognition classifier;
    [SerializeField]
    private TrainingRenderer viewRenderer;
    
    #endregion

    #region public attributes
    public bool debugMode;
    public bool debugFlashcards;
    public bool isCalculating;
    #endregion

    #region private methods
    void Start()
    {
        Init();
        if (debugMode == true)
        {
            DebugTraining();
        }

        RunTraining(AppData.loadFilePath);
    }
    private void Update()
    {
        if (!inPause)
        {
            if (!inPause)
            {
                timeRemaining = timeRemaining - Time.deltaTime;
                if (timeRemaining >= 0)
                {
                    viewRenderer.UpdateTimer(timeRemaining, quizTimer);
                }
                else
                {
                    viewRenderer.GoToEndScreen(quizScore, quizQuestionNumber, timeRemaining);
                }
            }
        }
    }
    void DebugTraining()
    {
        List<HandSign> s = new List<HandSign>();
        s.Add(new HandSign("SIBI_A", "Huruf A", "", "", "Huruf A dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_B", "Huruf B", "", "", "Huruf B dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf C", "", "", "Huruf C dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_D", "Huruf D", "", "", "Huruf D dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_E", "Huruf E", "", "", "Huruf E dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_F", "Huruf F", "", "", "Huruf F dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_G", "Huruf G", "", "", "Huruf G dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_H", "Huruf H", "", "", "Huruf H dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_I", "Huruf I", "", "", "Huruf I dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_J", "Huruf J", "", "", "Huruf J dalam SIBI", GestureType.Dynamic));
    }
    private void Init()
    {
        dataModel = new TrainingData();
        classifier = new GestureRecognition();
    }
    #endregion

    #region coroutines
    IEnumerator CalculateScore()
    {
        isCalculating = true;
        distanceScore = Mathf.Infinity;
        float deltaTime = 0f;
        bool finish = false;
        yield return new WaitForSeconds(1);
        viewRenderer.ScoreStandby();
        while (classifier.Active && !finish && !skip)
        {
            if (!inPause)
            {
                if (classifier.Mode == GestureType.Static)
                {
                    distanceScore = classifier.GetGestureDistance(dataModel.trainingSigns[currentSignIdx].sign_name);
                    viewRenderer.UpdateClassifierScore(distanceScore);
                    if (distanceScore != Mathf.Infinity)
                    {
                        if (distanceScore <= distanceThreshold)
                        {
                            viewRenderer.HoldState();
                            deltaTime = deltaTime + Time.deltaTime;
                            if (deltaTime > 2.5f)
                            {
                                quizScore = quizScore + 1;
                                viewRenderer.GoToNextQuestion();
                                finish = true;

                                isCalculating = false;
                            }
                        }
                        else
                        {
                            deltaTime = 0f;
                        }
                    }
                }
                else
                {
                    viewRenderer.UpdateRecorderState(classifier.CurrentState);
                    if (classifier.HasFinishedRecordingDynamicGesture())
                    {
                        distanceScore = classifier.GetGestureDistance(dataModel.trainingSigns[currentSignIdx].sign_name);
                        viewRenderer.UpdateClassifierScore(distanceScore);
                        if (distanceScore != Mathf.Infinity)
                        {
                            if (distanceScore <= distanceThreshold)
                            {
                                viewRenderer.GoToNextQuestion();
                                quizScore = quizScore + 1;
                                finish = true;
                                isCalculating = false;
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion

    #region public methods
    public void RunTraining(string filename)
    {
        Init();
        quizQuestionNumber = 10;
        dataModel.LoadTraining(filename, quizQuestionNumber);

        currentSignIdx = -1;
        quizScore = 0;
        Debug.Log("load database" + " " + AppData.databasePath);
        classifier.LoadDatabase(AppData.databasePath);

        NextSign();

        if(!classifier.Active)
        {
            classifier.StartClassifier();
        }
        inPause = false;
        timeRemaining = quizTimer;
    }
    public void Skip()
    {
        skip = true;
        //StopAllCoroutines();
        NextSign();
    }

    public void RestartQuiz()
    {
        RunTraining(AppData.loadFilePath);
    }

    public void NextSign()
    {
        skip = false;
        currentSignIdx = currentSignIdx + 1;
        if (currentSignIdx < dataModel.currentNumSigns)
        {
            viewRenderer.UpdateFlashcard(dataModel.trainingSigns[currentSignIdx]);
            viewRenderer.UpdateQuestionNumText(currentSignIdx, quizQuestionNumber);
            if (!classifier.Active)
            {
                classifier.StartClassifier();
            }
            if (dataModel.trainingSigns[currentSignIdx].type == GestureType.Static)
            {
                classifier.ChangeToStaticMode();
            }
            else
            {
                classifier.ChangeToDynamicMode();
            }
            StartCoroutine("CalculateScore");
        }
        else
        {
            inPause = true;
            viewRenderer.GoToEndScreen(quizScore,dataModel.currentNumSigns, timeRemaining);
        }
    }

    public void Pause()
    {
        inPause = true;
    }
    public void Resume()
    {
        inPause = false;
    }
    #endregion
}
