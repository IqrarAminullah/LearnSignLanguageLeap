using System.Collections;
using System.Collections.Generic;

public class HandSign
{
    #region attributes
    public string sign_name { get; set; }
    public string sign_word { get; set; }
    public string sign_image_path { get; set; }
    public string image_path { get; set; }
    public string meaning { get; set; }
    #endregion

    #region public methods
    public HandSign()
    {
        sign_name = "";
        sign_word = "";
        sign_image_path = "";
        image_path = "";
        meaning = "";
    }

    public HandSign(string _name, string _word, string _sign_image, string _image, string _meaning)
    {
        sign_name = _name;
        sign_word = _word;
        sign_image_path = _sign_image;
        image_path = _image;
        meaning = _meaning;
    }
    #endregion
}
