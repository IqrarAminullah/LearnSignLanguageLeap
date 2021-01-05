using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    #region Attributes
    private List<LessonMaterial> lesson;
    private int currentMaterial;
    private int materialCount;

    [SerializeField]
    private LessonRenderer viewRenderer;
    private JSONIO jsonUtility;
    private LessonData dataModel;
    public bool debugMode;
    #endregion


    #region private methods
    // Start is called before the first frame update
    void Start()
    {
        dataModel = new LessonData();
        if (debugMode == true)
        {
            StartLesson("testLesson.json");
        }
        StartLesson("testLesson.json");
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
    }

    #endregion

    #region public methods
    public void StartLesson(string filename)
    {
        dataModel.LoadLesson(filename);
        currentMaterial = 0;
        viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
    }
    public void nextMaterial()
    {
        if(currentMaterial == dataModel.materialCount-1)
        {
            viewRenderer.GoToEndScreen();
        }
        else
        {
            currentMaterial = currentMaterial + 1;
            viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
        }
    }
    public void prevMaterial()
    {
        currentMaterial = currentMaterial - 1;
        viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
    }

    public void jumpToMaterial(int page)
    {
        currentMaterial = page;
        viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
    }
    #endregion
}
