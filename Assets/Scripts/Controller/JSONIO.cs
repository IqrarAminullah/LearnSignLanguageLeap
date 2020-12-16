using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity;
using UnityEngine;
using Newtonsoft.Json;

public class JSONIO
{
    #region constants
    string filepath = Application.dataPath + "/Data/JSON/";
    #endregion
    #region private methods
    private void RemoveFileExtension()
    {

    }
    #endregion
    #region public methods
    public T LoadJSON<T>(string file)
    {
        if (File.Exists(filepath + file))
        {
            using (StreamReader reader = File.OpenText(filepath+file))
            {
                JsonSerializer serializer = new JsonSerializer();
                T obj = (T) serializer.Deserialize(reader, typeof(T));

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
        }
        else
        {
            Debug.Log("File path is empty");
            return default(T);
        }
    }

    public void SaveJson<T>(string file, T obj)
    {
        if(filepath == string.Empty)
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
            Debug.Log("JSON saved : " + filepath + file);
        }
    }
    #endregion
}
