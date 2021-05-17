using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class SaveComponent
{
    public void saveByJson<T>(string jsonFile,T data)
    {
        string filePath = Application.dataPath + "/Resources/" + jsonFile + ".json";
        string saveJsonStr = JsonMapper.ToJson(data);

        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();
    }
    public T loadByJson<T>(string jsonFile)
    {
        string filePath = Application.dataPath + "/Resources/" + jsonFile + ".json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            T data = JsonMapper.ToObject<T>(jsonStr);
            return data;
        }
        return default(T);
    }

}
