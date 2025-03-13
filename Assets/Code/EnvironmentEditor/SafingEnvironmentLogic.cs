using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SafingEnvironmentLogic : MonoBehaviour
{
    public ErrorMessage Error;
    public LoadingOverlay Loading;

    public Environment2DApiClient EnvironmentClient;
    public Object2DApiClient ObjectClient;

    public Object objectPrefab;

    public async Awaitable SafeEnvironment(List<Object> objects)
    {
        Loading.On();



        Object[] gameObjectsInScene = GameObject.FindObjectsOfType<Object>();

        Debug.Log($"Total of objects found: {gameObjectsInScene.Length}");

        string environmentId = SceneDataEnvironmentEditor.environment.id;

        // Deleting all old objects before safing new ones
        await DeleteOldObjects(environmentId);

        // Safing new object
        if (gameObjectsInScene.Length == 0)
        {
            foreach (Object object2d in objects)
            {
                Debug.Log("In Foreach loop");
                await SafeObject(ParseObjectToObject2D(object2d, environmentId));
            }
        }
        else
        {
            foreach (Object object2d in gameObjectsInScene)
            {
                Debug.Log("In Foreach loop");
                await SafeObject(ParseObjectToObject2D(object2d, environmentId));
            }
        }


            Loading.Off();
    }

    private async Awaitable SafeObject(Object2D object2d)
    {
        IWebRequestReponse response = await ObjectClient.CreateObject2D(object2d);

        Debug.Log("Saving object");

        // Validating WebResponse
        switch (response)
        {
            case WebRequestData<string> webRequestResponse:
                Debug.Log("Successfully safed object");
                break;
            case WebRequestError errorResponse:
                Error.SetErrorMessage(errorResponse.ErrorMessage);
                Debug.Log(errorResponse.ErrorMessage);
                break;
        }
    }

    private Object2D ParseObjectToObject2D(Object object2d, string environment2dId)
    {
        Object2D newObject = new()
        {
            id = Guid.NewGuid().ToString(),
            prefabId = object2d.DisplayedObject.prefabId,
            positionX = object2d.transform.position.x + 0.5f,
            positionY = object2d.transform.position.y + 0.5f,
            scaleX = object2d.DisplayedObject.scaleX,
            scaleY = object2d.DisplayedObject.scaleY,
            rotationZ = 0,
            sortingLayer = 1,
            environmentId = environment2dId
        };

        Debug.Log("Parsed object");

        return newObject;
    }

    private async Awaitable DeleteOldObjects(string environmentId)
    {
        _ = ObjectClient.DeleteObject2Ds(environmentId);

        await Task.Delay(1500);

        Debug.Log("Reached End of deleting");
    }
}
