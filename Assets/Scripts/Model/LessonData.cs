using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessonData
{
    #region attributes
    public List<LessonMaterial> lesson { get; set; }
    public int materialCount { get; set; }
    private JSONIO jsonUtility;
    #endregion
    // Start is called before the first frame update
    public LessonData()
    {
        jsonUtility = new JSONIO();
        lesson = new List<LessonMaterial>();
        materialCount = 1;
    }

    public void LoadLesson(string filename)
    {
        if(jsonUtility == null)
        {
            jsonUtility = new JSONIO();
        }
        lesson = jsonUtility.LoadJSON<List<LessonMaterial>>(filename);
        materialCount = lesson.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
