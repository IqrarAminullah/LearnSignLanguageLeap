using System.Collections;
using System.Collections.Generic;

public enum MediaType
{
    None,
    Image,
    Video
}
public class LessonMaterial
{
    #region attributes
    public string title { get; set; }
    public string text { get; set; }
    public string mediaFilename { get; set; }
    public MediaType mediaType { get; set; }
    public string mediaDescription { get; set; }
    #endregion

    #region methods
    public LessonMaterial()
    {
        title = string.Empty;
        text = string.Empty;
        mediaFilename = string.Empty;
        mediaType = MediaType.None;
        mediaDescription = string.Empty;
    }

    public LessonMaterial(string _title, string _text, string _mediaFilename, MediaType type, string desc)
    {
        title = _title;
        text = _text;
        mediaFilename = _mediaFilename;
        mediaType = type;
        mediaDescription = desc;
    }

    #endregion
}
