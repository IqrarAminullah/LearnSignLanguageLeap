using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity;
using UnityEngine;
using Newtonsoft.Json;

public class JSONIO
{
    #region constants
    string filepath = Application.dataPath + "/Resources/JSON/";
    #endregion
    #region private methods
    private string RemoveFileExtension(string filePath)
    {
        string newFilePath = filePath.Replace(".json", "");
        return newFilePath;
    }
    #endregion
    #region public methods
    public T LoadJSON<T>(string file)
    {
        string filename = RemoveFileExtension(file);
        var targetFile = Resources.Load<TextAsset>("JSON/" + filename);
        if(targetFile != null)
        {
            T obj = JsonConvert.DeserializeObject<T>(targetFile.text);
            if (obj == null)
            {
                Debug.LogError("Failed to Load JSON " + file);
                return default(T);
            }
            else
            {
                Debug.Log("Loading JSON " + file);
                return obj;
            }
        }
        else
        {
            Debug.LogError("File path "+ filename+" is empty");
            return default(T);
        }
    }

    public void SaveJson<T>(string file, T obj)
    {
        if(file == string.Empty)
        {
            Debug.LogError("JSON Write Error : Path is empty");
        }
        else
        {
            using (StreamWriter writer = File.CreateText(filepath + file))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
    
        }
    }
    #endregion
}
