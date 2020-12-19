using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LessonController : MonoBehaviour
{
    #region Attributes
    private List<LessonMaterial> lesson;
    private int currentMaterial;
    private int materialCount;

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

    [SerializeField]
    private GameObject videoPrefab;
    [SerializeField]
    private GameObject imagePrefab;
    [SerializeField]
    private GameObject textPrefab;

    private JSONIO jsonUtility;
    public bool debugMode;
    #endregion


    #region private methods
    // Start is called before the first frame update
    void Start()
    {
        jsonUtility = new JSONIO();
        prevButton.onClick.AddListener(prevMaterial);
        nextButton.onClick.AddListener(nextMaterial);
        
        if(debugMode == true)
        {
            DebugLesson();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void DebugLesson()
    {
        List<LessonMaterial> n = new List<LessonMaterial>();
        n.Add(new LessonMaterial("Alpabet", "HUUUUUUUUUUUUUUUUUUUUUUUUU AAAAAAAAAAAAAAAA", "Sprites/A_Alphabet", MediaType.Image, "AAAAAAAAAAA"));
        n.Add(new LessonMaterial("Ayam", "AAAAAAAAAAAAAAAA", "Video/Test Video", MediaType.Video, "AAAAAAAAAAA"));
        jsonUtility.SaveJson("testLesson.json", n);

        StartLesson(jsonUtility.LoadJSON<List<LessonMaterial>>("testLesson.json"));
    }

    private void displayMaterial(LessonMaterial m)
    {
        foreach (Transform child in contentContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (currentMaterial == 0)
        {
            prevButton.interactable = false;
        }else
        {
            prevButton.interactable = true;
        }
        if(currentMaterial == materialCount)
        {
            nextButton.interactable = false;
        }
        else
        {
            nextButton.interactable = true;
        }
        pageText.text = string.Format("page {0} of {1}", currentMaterial+1, materialCount);
        titleText.text = m.title;
        if (m.text != string.Empty)
        {
            GameObject textContainer = Instantiate(textPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
            textContainer.GetComponentInChildren<Text>().text = m.text;
        }
        if (m.mediaType != MediaType.None)
        {
            Debug.Log("Loading Media : " + m.mediaFilename);
            if(m.mediaType == MediaType.Video)
            {
                GameObject videoContainer = Instantiate(videoPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
                videoContainer.GetComponentInChildren<VideoController>().PlayVideoOnClip(Resources.Load<VideoClip>(m.mediaFilename));
            }
            if(m.mediaType == MediaType.Image)
            {
                GameObject imageContainer = Instantiate(imagePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), contentContainer.transform);
                imageContainer.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(m.mediaFilename);
            }
        }
    }
    #endregion

    #region public methods
    public void StartLesson(List<LessonMaterial> _lesson)
    {
        lesson = _lesson;
        materialCount = lesson.Count;
        currentMaterial = 0;
        displayMaterial(lesson[currentMaterial]);
    }
    public void nextMaterial()
    {
        currentMaterial = currentMaterial + 1;
        if(currentMaterial == materialCount)
        {

        }
        else
        {
            displayMaterial(lesson[currentMaterial]);
        }
    }
    public void prevMaterial()
    {
        currentMaterial = currentMaterial - 1;
        displayMaterial(lesson[currentMaterial]);
    }

    public void jumpToMaterial(int page)
    {
        currentMaterial = page;
        displayMaterial(lesson[currentMaterial]);
    }
    #endregion
}
