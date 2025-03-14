using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnvironmentViewerLogic : MonoBehaviour
{
    public GameObject environmentPrefab;
    public CameraController cameraController;  // Reference to CameraController
    public Environment2DApiClient EnvironmentClient;
    public Object2DApiClient ObjectClient;

    public Object ObjectPrefab;
    public Vector3 Position;

    public ErrorMessage Error;
    public LoadingOverlay Loading;

    private string environmentId;
    private Environment2D environment;

    void Start()
    {
        environment = SceneDataEnvironmentViewer.environment;  // Retrieve the ID
        Debug.Log(environment.id.ToString());
        GenerateMap();
    }

    private int frameCount = 0;

    private async void Update()
    {
        frameCount++;



        if (frameCount == 2) // Trigger on second frame
        {

            Debug.Log("Second frame Reached");
            List<Object2D> objects2d = await GetAllObjects(environment.id);

            foreach (Object2D object2d in objects2d)
            {
                InstantiateObject(object2d);
            }
        }
    }

    private async Task<List<Object2D>> GetAllObjects(string id)
    {
        List<Object2D> objects = new List<Object2D>();

        Loading.On();
        IWebRequestReponse response = await ObjectClient.ReadObject2Ds(id);

        // Validating WebResponse
        switch (response)
        {
            case WebRequestData<List<Object2D>> webRequestResponse:
                objects = webRequestResponse.Data;
                Debug.Log("Successfully received objects: " + objects.Count);
                break;
            case WebRequestError errorResponse:
                Error.SetErrorMessage(errorResponse.ErrorMessage);
                break;
            case WebRequestData<string> webRequestResponse:
                Debug.Log(webRequestResponse.Data);
                break;
        }   

        Loading.Off();
        return objects;
    }


    void GenerateMap()
    {
        // Example: Instantiate with specific dimensions
        GameObject environmentInstance = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity);

        // Initialize the layout with custom width and height
        EnvironmentLayout layout = environmentInstance.GetComponent<EnvironmentLayout>();
        layout.InitializeLayout(environment.maxWidth, environment.maxHeight);

        // Initialize the camere movement
        cameraController.Initialize(environment.maxWidth, environment.maxHeight);
    }

    // New method to instantiate an Object in the environment
    public void InstantiateObject(Object2D object2D)
    {
        Object newObject = Instantiate(ObjectPrefab, Position, Quaternion.identity);

        newObject.DisplayedObject = object2D;
        newObject.PlaceInLayout();
    }

    public void BackButton()
    {
        SceneManager.LoadScene("EnvironmentsOverview");
    }
}
