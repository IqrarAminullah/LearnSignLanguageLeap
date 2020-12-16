using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using LeapGestureRecognition;

public class Training : MonoBehaviour
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
    #endregion
    #region public attributes
    #endregion

    #region private methods
    void Start()
    {
        trainingSigns = new List<HandSign>();
        jsonUtility = new JSONIO();
        classifier = new GestureRecognition();
        CreateJSON();
        LoadTraining("Dummy.json");

        flashcard = flashcardObject.GetComponent<Flashcard>();
        flashcard.sign = trainingSigns[0];
        Debug.Log(flashcard.sign.sign_name);
    }

    void CreateJSON()
    {
        List<HandSign> s = new List<HandSign>();
        s.Add(new HandSign("SIBI_A", "Huruf A", "", "", "Huruf A dalam SIBI"));
        s.Add(new HandSign("SIBI_B", "Huruf B", "", "", "Huruf B dalam SIBI"));
        s.Add(new HandSign("SIBI_C", "Huruf C", "", "", "Huruf C dalam SIBI"));
        jsonUtility.SaveJson("Dummy.json",s);
    }
    #endregion

    #region public methods
    public void LoadTraining(string file)
    {
        trainingSigns = jsonUtility.LoadJSON<List<HandSign>>(file);
        foreach(HandSign sign in trainingSigns)
        {
            Debug.Log(sign.sign_name);
        }
    }

    
    #endregion
}
