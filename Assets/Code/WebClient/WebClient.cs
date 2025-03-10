using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebClient : MonoBehaviour
{
    // Singleton instance
    public static WebClient instance { get; private set; }

    public string baseUrl = "https://avansict2232234.azurewebsites.net";
    private string token;

    // Called when the script is initialized
    private void Awake()
    {
        // Check if there's already an instance of WebClient
        if (instance != null && instance != this)
        {
            // If so, destroy this instance to enforce singleton pattern
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, set the instance to this one
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive when loading new scenes
        }
    }

    public void SetToken(string token)
    {
        this.token = token;
    }

    public async Awaitable<IWebRequestReponse> SendGetRequest(string route)
    {
        UnityWebRequest webRequest = CreateWebRequest("GET", route, "");
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendPostRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("POST", route, data);
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendPutRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("PUT", route, data);
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendDeleteRequest(string route)
    {
        UnityWebRequest webRequest = CreateWebRequest("DELETE", route, "");
        return await SendWebRequest(webRequest);
    }

    private UnityWebRequest CreateWebRequest(string type, string route, string data)
    {
        string url = baseUrl + route;
        Debug.Log("Creating " + type + " request to " + url + " with data: " + data);

        data = RemoveIdFromJson(data); // Backend throws error if it receiving empty strings as a GUID value.
        var webRequest = new UnityWebRequest(url, type);
        byte[] dataInBytes = new UTF8Encoding().GetBytes(data);
        webRequest.uploadHandler = new UploadHandlerRaw(dataInBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");
        AddToken(webRequest);
        return webRequest;
    }

    private async Awaitable<IWebRequestReponse> SendWebRequest(UnityWebRequest webRequest)
    {
        await webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                string responseData = webRequest.downloadHandler.text;
                return new WebRequestData<string>(responseData);
            default:
                return new WebRequestError(webRequest.error);
        }
    }

    private void AddToken(UnityWebRequest webRequest)
    {
        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
    }

    private string RemoveIdFromJson(string json)
    {
        return json.Replace("\"id\":\"\",", "");
    }
}

[Serializable]
public class Token
{
    public string token;
}
