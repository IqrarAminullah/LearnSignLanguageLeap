using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRenderer : MonoBehaviour
{

    #region attributes
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Button RestartQuizButton;
    [SerializeField]
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


    Flashcard flashcard;

    [SerializeField]
    TrainingController controller;
    [SerializeField]
    UIManager UIManager;
    #endregion
    #region private methods
    // Start is called before the first frame update
    void Start()
    {
        if (UIManager == null)
        {
            UIManager = FindObjectOfType<UIManager>();
        }
        if (controller == null)
        {
            controller = FindObjectOfType<TrainingController>();
        }

        flashcard = flashcardContainer.GetComponent<Flashcard>();
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
    }

    public void UpdateClassifierScore(float score)
    {
        if (score >= 100)
        {
            scoreText.text = "Tangan tidak terbaca!";
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
        UIManager.SwitchUI(UIType.EndScreen);
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
        controller.NextSign();
    }

    public void RestartQuiz()
    {
        UIManager.SwitchUI(UIType.MainMenu);
        controller.RestartQuiz();
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
