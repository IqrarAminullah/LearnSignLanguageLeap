using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LessonRenderer : UIManager
{
    #region attributes
    [Header("Object References")]
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text pageText;
    [SerializeField]
    private GameObject contentContainer;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button prevButton;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject videoPrefab;
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private GameObject textPrefab;

    [Header("Controller")]
    [SerializeField]
    private LessonController controller;
    #endregion
    // Start is called before the first frame update
    protected new void Awake()
    {
        base.Awake();
        prevButton.onClick.AddListener(controller.prevMaterial);
        nextButton.onClick.AddListener(controller.nextMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public Methods
    public void displayMaterial(LessonMaterial m)
    {
        titleText.text = m.title;
        if (m.text != string.Empty)
        {
            GameObject textContainer = Instantiate(textPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
            textContainer.GetComponentInChildren<Text>().text = m.text;
        }
        if (m.mediaType != MediaType.None)
        {
            Debug.Log("Loading Media : " + m.mediaFilename);
            if (m.mediaType == MediaType.Video)
            {
                GameObject videoContainer = Instantiate(videoPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
                videoContainer.GetComponentInChildren<VideoController>().PlayVideoOnClip(Resources.Load<VideoClip>(m.mediaFilename));
            }
            if (m.mediaType == MediaType.Image)
            {
                GameObject imageContainer = Instantiate(imagePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
                imageContainer.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(m.mediaFilename);
            }
        }
    }

    public void UpdatePage(LessonMaterial m, int currentMaterialIdx, int materialCount)
    {
        foreach (Transform child in contentContainer.transform)
        {
            Destroy(child.gameObject);
        }
        if (currentMaterialIdx == 0)
        {
            prevButton.interactable = false;
        }
        else
        {
            prevButton.interactable = true;
        }
        if (currentMaterialIdx == materialCount)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
        pageText.text = string.Format("page {0} of {1}", currentMaterialIdx + 1, materialCount);
        displayMaterial(m);
    }

    public void GoToEndScreen()
    {
        SwitchUI(UIType.EndScreen);
    }
    #endregion
}
