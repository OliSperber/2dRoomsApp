using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string route = "/account/register";
        string data = JsonUtility.ToJson(user);

        Debug.Log(data);

        if (WebClient.instance != null)
        {
           return await WebClient.instance.SendPostRequest(route, data);
        }
        else
        {
            Debug.LogError("WebClient singleton is not initialized.");
            return null;
        }
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string route = "/account/login/jwt";
        string data = JsonUtility.ToJson(user);

        IWebRequestReponse response = await WebClient.instance.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
    }

    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                WebClient.instance.SetToken(token);
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }

}

