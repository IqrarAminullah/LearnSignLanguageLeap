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
    public string dbPath;

    #endregion

    #region methods
    public MenuItem()
    {
        itemFilePath = "";
        itemImage = "";
        itemName = "";
        dbPath = "";
    }

    public MenuItem(string name, string imgPath, string filePath, string databasePath)
    {
        itemName = name;
        itemImage = imgPath;
        itemFilePath = filePath;
        dbPath = databasePath;
    }
    #endregion
}
