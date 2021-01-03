using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MenuType
{
    Lesson,
    Learn,
    Quiz
}
public class MainMenuController : MonoBehaviour
{
    #region constants
    private string lessonListFilepath = "LessonList.json";
    private string flashcardListFilepath = "FlashcardList.json";
    #endregion
    #region attributes
    [SerializeField]
    private TrainingController trainingController;
    [SerializeField]
    private LessonController lessonController;
    [SerializeField]
    private LearnController learningController;
    /*[SerializeField]
    private GameObject flashcardButtonPrefab;
    [SerializeField]
    private GameObject lessonButtonPrefab;
    [SerializeField]
    private GameObject flashcardButtonContainer;
    [SerializeField]
    private GameObject lessonButtonContainer;
    [SerializeField]
    private GameObject quizButtonContainer;*/

    [SerializeField]
    private Button lessonButton;
    [SerializeField]
    private Button learnButton;
    [SerializeField]
    private Button quizButton;

    private List<MenuItem> lessonList;
    private List<MenuItem> notLessonList;
    private JSONIO jsonUtility;
    private UIManager UIManager;

    [SerializeField]
    private bool debugMode;
    [SerializeField]
    private MainMenuRenderer viewRenderer;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        jsonUtility = new JSONIO();
        if (debugMode)
        {
            DebugInit();
        }
    }

    void DebugInit()
    {
        Debug.Log("Main Menu Debug");
        List<MenuItem> l = new List<MenuItem>();
        List<MenuItem> f = new List<MenuItem>();

        f.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Flashcard/Alfabet"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Flashcard/Angka"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Flashcard/Alfabet"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Flashcard/Alfabet"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Flashcard/Alfabet"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Flashcard/Alfabet"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Flashcard/Angka"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Flashcard/Angka"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Flashcard/Angka"));
        f.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Flashcard/Angka"));

        l.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Lesson/testLesson"));
        l.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Lesson/testLesson")); 
        l.Add(new MenuItem("Huruf", "Sprites/Icon/Alfabet", "Lesson/testLesson"));
        l.Add(new MenuItem("Huruf", "Sprites/Icon/Angka", "Lesson/testLesson"));

        jsonUtility.SaveJson("FlashcardList.json", f);
        jsonUtility.SaveJson("LessonList.json", l);

        Init();
        /*LoadLessonList();
        LoadFlashcardList();
        LoadQuizzes();*/

    }
    public void Init()
    { 
        lessonList = jsonUtility.LoadJSON<List<MenuItem>>(lessonListFilepath);
        notLessonList = jsonUtility.LoadJSON<List<MenuItem>>(flashcardListFilepath);
        UIManager = FindObjectOfType<UIManager>();

        lessonButton.onClick.AddListener(() => DisplayList(lessonList, MenuType.Lesson));
        learnButton.onClick.AddListener(() => DisplayList(notLessonList, MenuType.Learn));
        quizButton.onClick.AddListener(() => DisplayList(notLessonList, MenuType.Quiz));

        switch (AppData.currentActivity)
        {
            case ActivityType.Lesson:
                DisplayList(lessonList, MenuType.Lesson);
                UIManager.SwitchUI(UIType.MenuList);
                break;
            case ActivityType.Learn:
                DisplayList(notLessonList, MenuType.Learn);
                UIManager.SwitchUI(UIType.MenuList);
                break;
            case ActivityType.Quiz:
                DisplayList(notLessonList, MenuType.Quiz);
                UIManager.SwitchUI(UIType.MenuList);
                break;
            default:
                break;
        }
        AppData.currentActivity = ActivityType.Menu;
    }

    public void DisplayList(List<MenuItem> list, MenuType listType)
    {
        Debug.Log("Go To " + listType + "List");
        viewRenderer.CreateMenuList(list, listType);
    }

    #region public methods
    
    #endregion
}
