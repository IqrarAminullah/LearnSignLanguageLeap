using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingController : MonoBehaviour
{
    #region private attributes

    [Header("Variables")]
    [SerializeField]
    const float distanceThreshold = 7;
    [SerializeField]
    const float quizTimer = 10.5f;
    float timeRemaining;
    float distanceScore;

    Flashcard flashcard { get; set; }
    JSONIO jsonUtility;

    

    private bool skip;
    private bool inPause;
    private int quizScore;
    private int quizQuestionNumber;
    private int currentSignIdx;
    private HandSign currentSign;

    private TrainingData dataModel;
    [Header("References")]
    [SerializeField]
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

        RunTraining(AppData.LoadFilePath);
    }
    private void Update()
    {
        if (!inPause)
        {
            if (!classifier.Active)
            {
                classifier.StartClassifier();
            }
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
        jsonUtility.SaveJson("Dummy.json",s);
    }
    private void Init()
    {
        dataModel = new TrainingData();
    }
    #endregion

    #region coroutines
    IEnumerator CalculateScore()
    {
        isCalculating = true;
        distanceScore = Mathf.Infinity;
        float deltaTime = 0f;
        bool finish = false;
        while (classifier.Active && !finish && !skip)
        {
            if (classifier.Mode == GestureType.Static)
            {
                distanceScore = classifier.GetGestureDistance(dataModel.trainingSigns[currentSignIdx].sign_name);
                viewRenderer.UpdateClassifierScore(distanceScore);
                if (distanceScore != Mathf.Infinity)
                {
                    if (distanceScore <= distanceThreshold)
                    {
                        deltaTime = deltaTime + Time.time;
                        if (deltaTime > 2.0f)
                        {
                            quizScore = quizScore + 1;
                            viewRenderer.GoToNextQuestion();
                            finish = true;
                        }
                        isCalculating = false;
                    }
                }
            }
            else
            {
                viewRenderer.UpdateRecorderState(classifier.CurrentState);
                if (classifier.HasFinishedRecordingDynamicGesture())
                {
                    distanceScore = classifier.GetGestureDistance(currentSign.sign_name);
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
        classifier.LoadDatabase("lgr_db.sqlite");

        NextSign();

        StartCoroutine("CalculateScore");
        if (!classifier.Active)
        {
            classifier.StartClassifier();
        }
        inPause = false;
        timeRemaining = quizTimer;
    }
    public void Skip()
    {
        skip = true;
        StopAllCoroutines();
        NextSign();
    }

    public void RestartQuiz()
    {
        RunTraining(AppData.LoadFilePath);
    }

    public void NextSign()
    {
        skip = false;
        currentSignIdx = currentSignIdx + 1;
        if (currentSignIdx < dataModel.currentNumSigns)
        {
            viewRenderer.UpdateFlashcard(dataModel.trainingSigns[currentSignIdx]);
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
            viewRenderer.UpdateQuestionNumText(currentSignIdx,quizQuestionNumber);
        }
        else
        {
            Debug.Log("seleesee");
            viewRenderer.GoToEndScreen(quizScore,quizQuestionNumber, timeRemaining);
        }
    }

    public void Pause()
    {
        inPause = true;
        classifier.Active = false;
    }
    public void Resume()
    {
        inPause = false;
        classifier.Active = true;
    }
    #endregion
}
