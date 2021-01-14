using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashcard : MonoBehaviour
{
    // Start is called before the first frame update

    #region attributes
    private Image signImage;
    private Image representationImage;
    private Text signName;
    private Text signMeaning;

    private Button flashcardButton;
    
    [SerializeField]
    private GameObject frontSide;
    [SerializeField]
    private GameObject backSide;


    float flipXValue;
    private HandSign sign;
    private bool frontActive;
    #endregion

    #region private methods
    private void Awake()
    {
        if (signImage == null || signName == null || signMeaning == null || representationImage == null)
        {
            Init();
        }
        Reset();
    }

    void Init()
    {
        signImage = backSide.GetComponentInChildren<Image>();
        signName = frontSide.GetComponentInChildren<Text>();
        signMeaning = backSide.GetComponentInChildren<Text>();
        representationImage = frontSide.GetComponentInChildren<Image>();
        flashcardButton = GetComponent<Button>();
        flashcardButton.onClick.AddListener(FlipCard);
        flipXValue = 1f;
    }

    private void Update()
    {

        if (flipXValue < 1)
        {
            flipXValue += 0.05f;
            this.gameObject.transform.localScale = new Vector3(flipXValue, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
        }
    }
    #endregion

    #region public methods
    public void SetSign(HandSign newSign)
    {
        if (signImage == null || signName == null || signMeaning == null || representationImage == null)
        {
            Init();
        }
        sign = newSign;
        if(sign.sign_image_path != string.Empty)
        {
            if(signImage.sprite == null)
            {
            }
            signImage.sprite = Resources.Load<Sprite>(sign.sign_image_path);
        }
        if(sign.image_path != string.Empty)
        {
            representationImage.sprite = Resources.Load<Sprite>(sign.image_path);
        }
        signName.text = sign.sign_word;
        signMeaning.text = sign.meaning;
        Reset();
    }
    public void Reset()
    {
        backSide.SetActive(false);
        frontSide.SetActive(true);
        frontActive = true;
    }

    public void FlipCard()
    {

        flipXValue = 0.2f; //Flip card.
        if (frontActive == true)
        {
            backSide.SetActive(true);
            frontSide.SetActive(false);
        }
        else
        {
            backSide.SetActive(false);
            frontSide.SetActive(true);
        }
        frontActive = !frontActive;
    }
    #endregion
}
