using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem
{
    #region attributes
    public string itemName;
    public string itemImage;
    public string itemFilePath;

    #endregion

    #region methods
    public MenuItem()
    {
        itemFilePath = "";
        itemImage = "";
        itemName = "";
    }

    public MenuItem(string name, string imgPath, string filePath)
    {
        itemName = name;
        itemImage = imgPath;
        itemFilePath = filePath;
    }

    public void UpdateData(string name, string imgPath, string filePath)
    {
        itemName = name;
        itemImage = imgPath;
        itemFilePath = filePath;
    }
    #endregion
}
