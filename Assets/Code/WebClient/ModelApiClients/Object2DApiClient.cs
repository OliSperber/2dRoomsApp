using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

public class Object2DApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestReponse> ReadObject2Ds(string environmentId)
    {
        string route = "/api/environment2d/" + environmentId + "/object2d";

        IWebRequestReponse webRequestResponse = await WebClient.instance.SendGetRequest(route);
        return ParseObject2DListResponse(webRequestResponse);
    }
    public async Awaitable DeleteObject2Ds(string environmentId)
    {
        string route = "/api/environment2d/" + environmentId + "/object2d";


        IWebRequestReponse webRequestResponse = await WebClient.instance.SendDeleteRequest(route);
    }



    public async Awaitable<IWebRequestReponse> CreateObject2D(Object2D object2D)
    {
        string route = "/api/environment2d/" + object2D.environmentId + "/object2d";

        // Convert environment to JSON while excluding the id field
        var tempEnvironment = object2D;
        tempEnvironment.id = null; 

        string data = JsonUtility.ToJson(tempEnvironment);

        Debug.Log(data);


        IWebRequestReponse webRequestResponse = await WebClient.instance.SendPostRequest(route, data);
        return ParseObject2DResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> UpdateObject2D(Object2D object2D)
    {
        string route = "/api/environment2d/" + object2D.environmentId + "/object2d/" + object2D.id;
        string data = JsonUtility.ToJson(object2D);

        return await WebClient.instance.SendPutRequest(route, data);
    }

    private IWebRequestReponse ParseObject2DResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                Object2D object2D = JsonUtility.FromJson<Object2D>(data.Data);
                WebRequestData<Object2D> parsedWebRequestData = new WebRequestData<Object2D>(object2D);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseObject2DListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<Object2D> environments = JsonHelper.ParseJsonArray<Object2D>(data.Data);
                WebRequestData<List<Object2D>> parsedData = new WebRequestData<List<Object2D>>(environments);
                return parsedData;
            default:
                return webRequestResponse;
        }
    }
}