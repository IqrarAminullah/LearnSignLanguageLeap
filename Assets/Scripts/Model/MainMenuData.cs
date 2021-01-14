using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuData
{
    #region constants
    [SerializeField]
    private string lessonListFilepath = "LessonList.json";
    [SerializeField]
    private string flashcardListFilepath = "FlashcardList.json";
    [SerializeField]
    private string quizListFilepath = "FlashcardList.json";

    #endregion

    #region attributes
    public List<MenuItem> lessonList { get; set; }
    public List<MenuItem> flashcardList { get; set; }
    public List<MenuItem> quizList { get; set; }

    private JSONIO jsonUtility;
    #endregion

    #region public methods
    public MainMenuData()
    {
        jsonUtility = new JSONIO();
        LoadData();
    }

    public void LoadData()
    {
        lessonList = jsonUtility.LoadJSON<List<MenuItem>>(lessonListFilepath);
        flashcardList = jsonUtility.LoadJSON<List<MenuItem>>(flashcardListFilepath);
        quizList = jsonUtility.LoadJSON<List<MenuItem>>(quizListFilepath);
    }
    #endregion
}
