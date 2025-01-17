﻿using System.Collections;
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
    #region attributes

    [SerializeField]
    private MainMenuRenderer viewRenderer;
    private MainMenuData dataModel;
    #endregion

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(viewRenderer == null)
        {
            viewRenderer = FindObjectOfType<MainMenuRenderer>();
        }
        Init();
        AppData.currentActivity = ActivityType.Menu;
    }
    public void Init()
    {
        dataModel = new MainMenuData();
        LoadData();
    }

    #region public methods

    public void LoadData()
    {
        dataModel.LoadData();
        switch (AppData.currentActivity)
        {
            case ActivityType.Lesson:
                viewRenderer.CreateMenuList(dataModel.lessonList, MenuType.Lesson);
                viewRenderer.SwitchUI(UIType.EndScreen);
                break;
            case ActivityType.Learn:
                viewRenderer.CreateMenuList(dataModel.flashcardList, MenuType.Learn);
                viewRenderer.SwitchUI(UIType.EndScreen);
                break;
            case ActivityType.Quiz:
                viewRenderer.CreateMenuList(dataModel.quizList, MenuType.Quiz);
                viewRenderer.SwitchUI(UIType.EndScreen);
                break;
            default:
                break;
        }
    }

    public void DisplayLessons()
    {
        viewRenderer.CreateMenuList(dataModel.lessonList, MenuType.Lesson);
    }

    public void DisplayFlashcards()
    {
        viewRenderer.CreateMenuList(dataModel.flashcardList, MenuType.Learn);
    }

    public void DisplayQuizzes()
    {
        viewRenderer.CreateMenuList(dataModel.quizList, MenuType.Quiz);
    }

    public void LoadAppData(string jsonPath, string dbPath, ActivityType type)
    {
        Debug.Log(dbPath);
        AppData.loadFilePath = jsonPath;
        AppData.databasePath = dbPath;
        AppData.currentActivity = type;

    }

    public void QuitAplication()
    {
        Application.Quit();
    }
    #endregion
}
