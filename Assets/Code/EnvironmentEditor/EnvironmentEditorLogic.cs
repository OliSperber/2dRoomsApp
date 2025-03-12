using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnvironmentEditorLogic : MonoBehaviour
{
    public GameObject environmentPrefab;
    public CameraController cameraController;
    public Environment2DApiClient EnvironmentClient;
    public Object2DApiClient ObjectClient;

    public Object ObjectPrefab;
    public Vector3 Position;

    public ErrorMessage Error;
    public LoadingOverlay Loading;

    private string environmentId;
    private Environment2D environment;
    private List<Object> objects = new();

    public Button SafeButton;
    public SafingEnvironmentLogic SafeHandler;

    void Start()
    {
        environment = SceneDataEnvironmentEditor.environment;
        GenerateMap();

        SafeButton.onClick.AddListener(() => _ = SafeEnvironment());
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
        // Instantiate prefab
        GameObject environmentInstance = Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity);

        // Give custom width and height
        EnvironmentLayout layout = environmentInstance.GetComponent<EnvironmentLayout>();
        layout.InitializeLayout(environment.maxWidth, environment.maxHeight);

        // Initialize the camere movement with maxHeight and maxWidth
        cameraController.Initialize(environment.maxWidth, environment.maxHeight);
    }

    // Method to instantiate an Object in the environment
    public void InstantiateObject(Object2D object2D)
    {
        // Ensure the ObjectPrefab is a GameObject
        Object newObject = Instantiate(ObjectPrefab, Position, Quaternion.identity);

        // Now you can access GameObject-specific properties
        newObject.GetComponent<Object>().environmentHeight = environment.maxHeight;
        newObject.GetComponent<Object>().environmentWidth = environment.maxWidth;
        newObject.GetComponent<Object>().SafeButton = this.SafeButton;
        newObject.GetComponent<Object>().DisplayedObject = object2D;
        newObject.GetComponent<Object>().EditAble = true;
        newObject.GetComponent<Object>().PlaceInLayout();

        objects.Add(newObject);
    }

    private async Awaitable SafeEnvironment()
    {
        await SafeHandler.SafeEnvironment(objects);

        ColorBlock colors = SafeButton.colors;
        colors.normalColor = new Color32(241, 241, 241, 255); 
        SafeButton.colors = colors;
    }

    public void BackButton()
    {
        SceneManager.LoadScene("EnvironmentsOverview");
    }
}
