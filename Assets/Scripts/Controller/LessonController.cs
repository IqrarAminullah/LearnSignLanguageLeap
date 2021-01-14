using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonController : MonoBehaviour
{
    #region Attributes
    private int currentMaterial;

    [SerializeField]
    private LessonRenderer viewRenderer;
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
            StartLesson(AppData.loadFilePath);
        }
        StartLesson(AppData.loadFilePath);
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
    public void NextMaterial()
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
    public void PrevMaterial()
    {
        currentMaterial = currentMaterial - 1;
        viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
    }

    public void JumpToMaterial(int pageNumber)
    {
        currentMaterial = pageNumber;
        viewRenderer.UpdatePage(dataModel.lesson[currentMaterial], currentMaterial, dataModel.materialCount);
    }
    #endregion
}
