﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRenderer : UIManager
{

    #region attributes
    [Header("Object References")]
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Button RestartQuizButton;
    [SerializeField]
    GameObject skipButtonObject;
    Button skipButton;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text pageText;
    [SerializeField]
    Text finalScoreText;
    [SerializeField]
    Text timerText;
    [SerializeField]
    Text finalTimeText;
    [SerializeField]
    Text statusText;
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    Button resumeButton;
    [SerializeField]
    GameObject flashcardContainer;
    [SerializeField]
    //Animator transitionAnimator;

    int animationStateHash;
    IEnumerator IE_transition;

    [Header("Controller")]
    [SerializeField]
    TrainingController controller;

    Flashcard flashcard;
    #endregion
    #region private methods
    // Start is called before the first frame update
    void Start()
    {
        animationStateHash = Animator.StringToHash("state");
        if (controller == null)
        {
            controller = FindObjectOfType<TrainingController>();
        }

        flashcard = flashcardContainer.GetComponent<Flashcard>();
        skipButtonObject.gameObject.SetActive(false);
        skipButton = skipButtonObject.GetComponent<Button>();
        skipButton.onClick.AddListener(SkipQuestion);
        RestartQuizButton.onClick.AddListener(RestartQuiz);
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region public methods
    public void UpdateQuestionNumText(int questionNum, int numOfQuestions)
    {
        pageText.text = string.Format("Kuis {0} dari {1}", questionNum + 1, numOfQuestions);
    }

    public void SkipQuestion()
    {
        controller.Skip();
    }

    public void UpdateFlashcard(HandSign sign)
    {
        if (flashcard == null)
        {
            flashcard = flashcardContainer.GetComponent<Flashcard>();
        }
        flashcard.SetSign(sign);
        scoreText.text = "Mengaktifkan Pengenal";
        scoreText.color = Color.black;
    }

    public void ScoreStandby()
    {
        scoreText.text = "Silahkan berisyarat";
    }

    public void UpdateClassifierScore(float score)
    {
        
        if (score >= 100)
        {
            scoreText.text = "Tangan tidak terbaca dengan baik!";
            scoreText.color = Color.black;
        }
        else
        {
            if (score <= 3f)
            {
                scoreText.text = "Ya!";
                scoreText.color = Color.green;
            }
            else
            {
                scoreText.text = "Salah!";
                scoreText.color = Color.red;
            }
        }
    }

    public void UpdateTimer(float time,float timer)
    {
        timerText.text = string.Format("Waktu tersisa \n {0:0} detik", time);
        float percentTime = time / timer;
        if(percentTime >= 0.5)
        {
            timerText.color = Color.green;
        }else if(percentTime <= 0.499 && percentTime >= 0.3)
        {
            timerText.color = Color.yellow;
        }
        else if(percentTime <= 0.299)
        {
            timerText.color = Color.red;
        }
    }

    public void GoToEndScreen(int score, int questionNum,float time)
    {
        finalScoreText.text = string.Format("Skor anda : {0}/{1}", score, questionNum);
        if(time < 1)
        {
            finalTimeText.text = "Anda kehabisan waktu!";
        }
        else
        {
            finalTimeText.text = string.Format("Waktu tersisa \n {0:0} detik", time);
        }
        SwitchUI(UIType.EndScreen);
    }

    public void DisplayTransition()
    {
        //transitionAnimator.SetInteger(animationStateHash, 2);
        if(IE_transition != null)
        {
            StopCoroutine(IE_transition);
        }

        IE_transition = TimedTransition();
        StartCoroutine(IE_transition);
    }

    IEnumerator TimedTransition()
    {
        SwitchUI(UIType.transitionScreen);
        yield return new WaitForSeconds(3);
        SwitchUI(UIType.MainMenu);

        controller.NextSign();
        //transitionAnimator.SetInteger(animationStateHash, 1);

    }

    public void UpdateRecorderState(LeapGestureRecognition.DGRecorderState state)
    {
        switch (state)
        {
            case LeapGestureRecognition.DGRecorderState.WaitingForHands:
                statusText.text = "Menunggu tangan";
                break;
            case LeapGestureRecognition.DGRecorderState.WaitingToStart:
                statusText.text = "Menunggu Posisi Awal";
                break;
            case LeapGestureRecognition.DGRecorderState.InStartPosition:
                statusText.text = "Memulai merekam gestur";
                break;
            case LeapGestureRecognition.DGRecorderState.RecordingGesture:
                statusText.text = "Merekam gestur";
                break;
            case LeapGestureRecognition.DGRecorderState.InEndPosition:
                statusText.text = "Mengakhiri rekaman gestur";
                break;
            case LeapGestureRecognition.DGRecorderState.RecordingJustFinished:
                statusText.text = "Rekaman selesai";
                break;
        }
    }

    public void GoToNextQuestion()
    {
        DisplayTransition();
    }

    public void RestartQuiz()
    {
        SwitchUI(UIType.MainMenu);
        controller.RestartQuiz();
    }

    public void HoldState()
    {
        statusText.text = "Tahan bentuk tangan";
    }

    public void Pause()
    {
        controller.Pause();
    }

    public void Resume()
    {
        controller.Resume();
    }
    #endregion
}
