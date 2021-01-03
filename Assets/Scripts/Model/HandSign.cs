using System.Collections;
using System.Collections.Generic;

public enum GestureType
{
    Static,
    Dynamic
}
public class HandSign
{
    #region attributes
    public string sign_name { get; set; }
    public string sign_word { get; set; }
    public string sign_image_path { get; set; }
    public GestureType type { get; set; }
    public string image_path { get; set; }
    public string meaning { get; set; }
    #endregion

    #region public methods
    public HandSign()
    {
        sign_name = "";
        sign_word = "";
        sign_image_path = "";
        type = GestureType.Static;
        image_path = "";
        meaning = "";
    }

    public HandSign(string _name, string _word, string _sign_image, string _image, string _meaning, GestureType gType)
    {
        sign_name = _name;
        sign_word = _word;
        sign_image_path = _sign_image;
        image_path = _image;
        meaning = _meaning;
        type = gType;
    }

    public override string ToString()
    {
        return string.Format("Name : {0}({1}), sign image : {2}, rep image : {3}, meaning : {4}, gesutype : {5}", sign_word, sign_name, sign_image_path, image_path, meaning, type);
    }
    #endregion
}
