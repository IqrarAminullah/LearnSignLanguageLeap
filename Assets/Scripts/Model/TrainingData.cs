using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingData : MonoBehaviour
{
    #region attributes

    private HandSign currentSign;
    private List<HandSign> handSigns;
    public List<HandSign> trainingSigns { get; set; }

    private JSONIO jsonUtility;

    public int totalSigns { get; set; }
    public int currentNumSigns { get; set; }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        trainingSigns = new List<HandSign>();
        jsonUtility = new JSONIO();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    #region public methods
    public void LoadTraining(string filename, int quizQuestionNumber)
    {
        if(jsonUtility == null)
        {
            jsonUtility = new JSONIO();
        }
        if(handSigns == null)
        {
            handSigns = jsonUtility.LoadJSON<List<HandSign>>(filename);
        }
        for (int i = 0; i < handSigns.Count; i++)
        {
            HandSign temp = handSigns[i];
            int randomIndex = Random.Range(i, handSigns.Count);
            handSigns[i] = handSigns[randomIndex];
            handSigns[randomIndex] = temp;
        }
        trainingSigns = new List<HandSign>();
        for (int i = 0; i < quizQuestionNumber; i++)
        {
            trainingSigns.Add(handSigns[i]);
        }
        totalSigns = handSigns.Count;
        currentNumSigns = trainingSigns.Count;
    }
    #endregion
}
