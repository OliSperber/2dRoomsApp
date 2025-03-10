using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime;

public static class JsonHelper
{
    public static List<T> ParseJsonArray<T>(string jsonArray)
    {
        string extendedJson = "{\"list\":" + jsonArray + "}";
        JsonList<T> parsedList = JsonUtility.FromJson<JsonList<T>>(extendedJson);
        return parsedList.list;
    }

    public static string ExtractToken(string data)
    {
        Token token = JsonUtility.FromJson<Token>(data);
        return token.token;
    }
}

[Serializable]
public class JsonList<T>
{
    public List<T> list;
}