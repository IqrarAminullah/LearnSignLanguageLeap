using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LearnRenderer : UIManager
{
    #region Attributes
    [Header("Object References")]
    [SerializeField]
    Text remainingUnknowns;
    [SerializeField]
    Text remainingKnowns;
    [SerializeField]
    Text masteredText;
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text pageText;
    [SerializeField]
    Text statusText;
    [SerializeField]
    GameObject flashcardContainer;
    [SerializeField]
    GameObject knowButton;
    [SerializeField]
    GameObject dontKnowButton;
    [SerializeField]
    GameObject nextButton;
    [SerializeField]
    GameObject relearnButtonGameObject;
    Button reLearnButton;

    [Header("Controller")]
    [SerializeField]
    LearnController controller;

    bool giveUp;
    bool know;
    Flashcard flashcard;
    Button flashcardButton;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        if(controller == null)
        {
            controller = FindObjectOfType<LearnController>();
        }
        flashcard = flashcardContainer.GetComponent<Flashcard>();
        dontKnowButton.GetComponent<Button>().onClick.AddListener(FlipFlashcard);
        nextButton.GetComponent<Button>().onClick.AddListener(SkipFlashcard);
        knowButton.GetComponent<Button>().onClick.AddListener(NextFlashcard);
        menuButton.onClick.AddListener(Pause);
        reLearnButton = relearnButtonGameObject.GetComponent<Button>();
        reLearnButton.onClick.AddListener(ReLearn);
        flashcardButton = flashcardContainer.GetComponentInChildren<Button>();
        Debug.Log(controllerList.Count);

    }

    private void Update()
    {
    }

    #region public methods
    public void UpdateScore(float score)
    {
        if (!giveUp)
        {
            if (score >= 100)
            {
                scoreText.text = "Tangan tidak terbaca dengan baik";
                scoreText.color = Color.black;
            }
            else
            {
                if (!know)
                {
                    scoreText.text = "Akurasi : " + score.ToString();
                    if (score <= 3.0f)
                    {
                        //scoreText.text = "Ya!";
                        scoreText.color = Color.green;
                    }
                    else
                    {
                        //scoreText.text = "Bukan!";
                        scoreText.color = Color.red;
                    }
                }
            }
        }
    }

    public void ScoreStandby()
    {
        scoreText.text = "Silahkan berisyarat";
    }

    public void GoToEndScreen(int remainingSigns, int unknowns, int knowns, int masters)
    {
        SwitchUI(UIType.EndScreen);
        Debug.Log(knowns);
        remainingUnknowns.text = "Jumlah belum diketahui"+ "\n" + unknowns;
        remainingKnowns.text = "Jumlah sudah diketahui" + "\n" + knowns;
        masteredText.text = "Jumlah dikuasai" + "\n" + masters;
        if(remainingSigns == 0)
        {
            relearnButtonGameObject.gameObject.SetActive(false);
        }
    }

    public void UpdateFlashcard(HandSign sign)
    {
        if(flashcard == null)
        {
            flashcard = flashcardContainer.GetComponent<Flashcard>();
        }
        flashcard.SetSign(sign);

        knowButton.gameObject.SetActive(false);
        giveUp = false;
        know = false;
        flashcardButton.interactable = false;
        scoreText.text = "Memulai Pengenal";
        scoreText.color = Color.black;
        //knowButton.GetComponent<Button>().interactable = false;
        nextButton.gameObject.SetActive(false);
    }

    public void UpdatePageText(int currentPageNumber, int numOfPages)
    {
        pageText.text = string.Format("Isyarat {0} dari {1}", currentPageNumber+ 1, numOfPages);
    }

    public void Pause()
    {

    }

    public void ReLearn()
    {
        SwitchUI(UIType.MainMenu);
        controller.ReLearn();
    }

    public void FlipFlashcard()
    {
        flashcard.FlipCard();
        //scoreText.gameObject.SetActive(false);
        knowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        flashcardButton.interactable = true;
    }

    public void EnableKnowButton()
    {
        scoreText.text = "Ya!";
        know = true;
        knowButton.SetActive(true);
    }

    public void SkipFlashcard()
    {
        controller.UnknownNext();
    }

    public void NextFlashcard()
    {
        controller.KnownNext();
    }

    public void GoToNextSign()
    {
        StartCoroutine("NextSignTransition");
    }

    public void UpdateStatus(GestureType type, LeapGestureRecognition.DGRecorderState state)
    {
        if(type == GestureType.Static){
            statusText.text = "Gestur statis";
        }
        else
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
    }

    IEnumerator NextSignTransition()
    {
        yield return null;
        controller.NextSign();
    }
    #endregion
}
