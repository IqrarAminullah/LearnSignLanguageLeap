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
    Text remianingKnowns;
    [SerializeField]
    Text masteredText;
    [SerializeField]
    Button menuButton;
    [SerializeField]
    Button reLearnButton;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text pageText;
    [SerializeField]
    GameObject flashcardContainer;
    [SerializeField]
    GameObject knowButton;
    [SerializeField]
    GameObject dontKnowButton;
    [SerializeField]
    GameObject nextButton;
    
    [Header("Controller")]
    [SerializeField]
    LearnController controller;

    Flashcard flashcard;
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
        reLearnButton.onClick.AddListener(ReLearn);
        Debug.Log(controllerList.Count);

    }

    #region public methods
    public void UpdateScore(float score)
    {
        if(score >= 100)
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
                scoreText.text = "Bukan!";
                scoreText.color = Color.red;
            }
        }
    }

    public void GoToEndScreen(int unknowns, int knowns, int masters)
    {
        SwitchUI(UIType.EndScreen);
        remainingUnknowns.text = "Jumlah belum diketahui"+ "\n" + unknowns;
        remianingKnowns.text = "Jumlah sudah diketahui" + "\n" + knowns;
        masteredText.text = "Jumlah dikuasai" + "\n" + masters;
    }

    public void UpdateFlashcard(HandSign sign)
    {
        if(flashcard == null)
        {
            flashcard = flashcardContainer.GetComponent<Flashcard>();
        }
        flashcard.SetSign(sign);

        knowButton.gameObject.SetActive(true);
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
        controller.ReLearn();
        SwitchUI(UIType.MainMenu);
    }

    public void FlipFlashcard()
    {
        flashcard.FlipCard();
        knowButton.GetComponent<Button>().interactable = true;
        knowButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
    }

    public void EnableKnowButton()
    {
        knowButton.GetComponent<Button>().interactable = true;
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

    IEnumerator NextSignTransition()
    {
        yield return null;
        controller.NextSign();
    }
    #endregion
}
