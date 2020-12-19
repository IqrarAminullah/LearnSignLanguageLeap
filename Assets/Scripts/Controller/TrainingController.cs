using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingController : MonoBehaviour
{
    #region private attributes
    List<HandSign> trainingSigns { get; set; }
    List<HandSign> correctSigns { get; set; }
    List<HandSign> remainingSigns { get; set; }

    [SerializeField]
    GameObject flashcardObject;
    Flashcard flashcard { get; set; }
    JSONIO jsonUtility;

    GestureRecognition classifier;
    private bool skip;
    private int currentSignNum;
    #endregion
    #region public attributes
    public bool debugMode;
    #endregion

    #region private methods
    void Start()
    {
        trainingSigns = new List<HandSign>();
        jsonUtility = new JSONIO();
        classifier = new GestureRecognition();
        flashcard = flashcardObject.GetComponent<Flashcard>();
        if (debugMode == true)
        {
            DebugTraining();
        }
    }

    void DebugTraining()
    {
        List<HandSign> s = new List<HandSign>();
        s.Add(new HandSign("SIBI_A", "Huruf A", "", "", "Huruf A dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_B", "Huruf B", "", "", "Huruf B dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf C", "", "", "Huruf C dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf D", "", "", "Huruf D dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf E", "", "", "Huruf E dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf F", "", "", "Huruf F dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf G", "", "", "Huruf G dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf H", "", "", "Huruf H dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf I", "", "", "Huruf I dalam SIBI", GestureType.Static));
        s.Add(new HandSign("SIBI_C", "Huruf J", "", "", "Huruf J dalam SIBI", GestureType.Dynamic));
        jsonUtility.SaveJson("Dummy.json",s);

        LoadTraining("Dummy.json");
    }
    #endregion

    #region public methods
    public void RunTraining()
    {
        for (int i = 0; i < trainingSigns.Count; i++)
        {
            HandSign temp = trainingSigns[i];
            int randomIndex = Random.Range(i, trainingSigns.Count);
            trainingSigns[i] = trainingSigns[randomIndex];
            trainingSigns[randomIndex] = temp;
        }
        foreach(HandSign sign in trainingSigns)
        {
            Debug.Log(sign.sign_word);
        }
        currentSignNum = -1;
        NextSign();
    }
    public void LoadTraining(string file)
    {
        trainingSigns = jsonUtility.LoadJSON<List<HandSign>>(file);
        foreach(HandSign sign in trainingSigns)
        {
            Debug.Log(sign.sign_name);
        }
        RunTraining();
    }

    public void SetSign(HandSign sign)
    {
        flashcard.SetSign(sign);
    }

    public void Skip()
    {
        skip = true;
        NextSign();
    }

    public void NextSign()
    {
        if(currentSignNum < trainingSigns.Count)
        {
            currentSignNum = currentSignNum + 1;
            SetSign(trainingSigns[currentSignNum]);
        }
        else
        {

        }
    }

    
    #endregion
}
