using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Environment2DApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestReponse> ReadEnvironment2Ds()
    {
        string route = "/api/environment2d";

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(route);
        return ParseEnvironment2DListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateEnvironment(Environment2D environment)
    {
        string route = "/api/environment2d";

        // Convert environment to JSON while excluding the id field
        var tempEnvironment = environment;
        tempEnvironment.id = null; 


        string data = JsonUtility.ToJson(tempEnvironment);


        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(route, data);
        return ParseEnvironment2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateEnvironment(Environment2D environment)
    {
        string route = "/api/environment2d";
        string data = JsonUtility.ToJson(environment);

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPutRequest(route, data);
        return ParseEnvironment2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> DeleteEnvironment(string environmentId)
    {
        string route = "/api/environment2d/" + environmentId;
        return await WebClient.instance.SendDeleteRequest(route);
    }

    private IWebRequestReponse ParseEnvironment2DResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Environment2D environment = JsonUtility.FromJson<Environment2D>(data.Data);
                WebRequestData<Environment2D> parsedWebRequestData = new WebRequestData<Environment2D>(environment);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseEnvironment2DListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Environment2D> environment2Ds = JsonHelper.ParseJsonArray<Environment2D>(data.Data);
                WebRequestData<List<Environment2D>> parsedWebRequestData = new WebRequestData<List<Environment2D>>(environment2Ds);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

