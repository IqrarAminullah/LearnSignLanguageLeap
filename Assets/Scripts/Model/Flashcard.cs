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

    [SerializeField]
    private GameObject frontSide;
    [SerializeField]
    private GameObject backSide;

    private HandSign sign;
    private bool frontActive;
    #endregion

    #region private methods
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(FlipCard);
        signImage = frontSide.GetComponentInChildren<Image>();
        signName = frontSide.GetComponentInChildren<Text>();
        signMeaning = backSide.GetComponentInChildren<Text>();
        representationImage = backSide.GetComponentInChildren<Image>();
        Reset();
    }
    #endregion

    #region public methods
    public void SetSign(HandSign newSign)
    {
        sign = newSign;
        signImage.sprite = Resources.Load<Sprite>(sign.sign_image_path);
        signName.text = sign.sign_word;
        signMeaning.text = sign.meaning;
        representationImage.sprite = Resources.Load<Sprite>(sign.image_path);
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
        if(frontActive == true)
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
