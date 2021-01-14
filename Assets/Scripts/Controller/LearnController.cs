using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LearnController : MonoBehaviour
{
    #region attributes
    LearnData dataModel;
    [SerializeField]
    const float distanceThreshold = 3;
    float distanceScore;

    [SerializeField]
    GestureRecognition classifier;
    [SerializeField]
    LearnRenderer viewRenderer;

    private int currentSignIdx;
    public bool isCalculating;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
        RunFlashcards(AppData.loadFilePath);
    }
    // Update is called once per frame
    void Update()
    {
        /*if (!isCalculating)
        {
            StartCoroutine("CalculateScore");
        }*/
    }
    private void Init()
    {
        dataModel = new LearnData();
        classifier = new GestureRecognition();
    }

    #region public methods
    public void RunFlashcards(string file)
    {
        Init();
        dataModel.LoadData(file);
        currentSignIdx = -1;
        NextSign();

        classifier.LoadDatabase(AppData.databasePath);
        if (!classifier.Active)
        {
            classifier.StartClassifier();
        }
    }

    public void NextSign()
    {
        viewRenderer.UpdateScore(100);
        currentSignIdx = currentSignIdx + 1;
        if (currentSignIdx < dataModel.signCount)
        {
            dataModel.SetCurrentSign(currentSignIdx);
            viewRenderer.UpdatePageText(currentSignIdx, dataModel.signCount);
            viewRenderer.UpdateFlashcard(dataModel.trainingSigns[currentSignIdx]);
            if (!classifier.Active)
            {
                classifier.StartClassifier();
            }
            if (dataModel.trainingSigns[currentSignIdx].type == GestureType.Static)
            {
                Debug.Log("staticgesture " + dataModel.trainingSigns[currentSignIdx].sign_name);
                classifier.ChangeToStaticMode();
            }
            else
            {
                Debug.Log("dynamicGesture " + dataModel.trainingSigns[currentSignIdx].sign_name);
                classifier.ChangeToDynamicMode();
            }

            isCalculating = true;
            StartCoroutine("CalculateScore");
        }
        else
        {

            dataModel.RemoveMastered();
            int t = dataModel.trainingSigns.Count;
            int u = dataModel.unknownFlashcards.Count;
            int k = dataModel.knownFlashcards.Count;
            int m = dataModel.masteredFlashcards.Count;
            viewRenderer.GoToEndScreen(t,u,k,m);
        }
    }

    public void KnownNext()
    {
        dataModel.UpgradeCurrentSign();
        NextSign();
    }

    public void UnknownNext()
    {
        dataModel.DowngradeCurrentSign();
        NextSign();
    }


    public void ReLearn()
    {
        Debug.Log("Relearn");
        currentSignIdx = -1;

        classifier = new GestureRecognition();
        classifier.LoadDatabase(AppData.databasePath);
        if (!classifier.Active)
        {
            classifier.StartClassifier();
        }
        NextSign();
    }
    

    IEnumerator CalculateScore()
    {

        Debug.Log("tes2");
        distanceScore = Mathf.Infinity;
        bool finish = false;
        float deltaTime = 0f;

        yield return new WaitForSeconds(1);
        viewRenderer.ScoreStandby();
        while (classifier.Active && !finish)
        {
            if (isCalculating)
            {
                if (classifier.Mode == GestureType.Static)
                {
                    distanceScore = classifier.GetGestureDistance(dataModel.trainingSigns[currentSignIdx].sign_name);
                    viewRenderer.UpdateScore(distanceScore);

                    if (distanceScore != Mathf.Infinity)
                    {
                        if (distanceScore <= distanceThreshold)
                        {
                            deltaTime = deltaTime + Time.deltaTime;
                            if (deltaTime > 0.8f)
                            {

                                isCalculating = false;
                                viewRenderer.EnableKnowButton();
                                finish = true;
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
                    viewRenderer.UpdateStatus(classifier.Mode, classifier.CurrentState);
                    if (classifier.HasFinishedRecordingDynamicGesture())
                    {
                        distanceScore = classifier.GetGestureDistance(dataModel.trainingSigns[currentSignIdx].sign_name);
                        Debug.Log("Score : " + distanceScore);
                        viewRenderer.UpdateScore(distanceScore);
                        if (distanceScore != Mathf.Infinity)
                        {
                            if (distanceScore <= distanceThreshold)
                            {

                                isCalculating = false;
                                viewRenderer.EnableKnowButton();
                                finish = true;
                            }
                        }
                    }
                }
            }
            yield return null;
        }
    }
    #endregion
}
