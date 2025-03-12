using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks; // Required for async/await

public class EnvironmentObject : MonoBehaviour
{
    public Button View;
    public Button Edit;
    public Button Delete;

    public TMP_Text EnvironmentName;

    public LoadingOverlay Loading;
    public ErrorMessage Error;

    public Environment2DApiClient ApiClient;

    private Environment2D environment;

    void Start()
    {
        // Add listeners to buttons
        View.onClick.AddListener(OnViewClicked);
        Edit.onClick.AddListener(OnEditClicked);
        Delete.onClick.AddListener(() => _ = OnDeleteClicked());
    }

    // Use this instead of a constructor
    public void Init(Environment2D environment, int position, Environment2DApiClient apiClient, LoadingOverlay loading, ErrorMessage error)
    {
        this.environment = environment;
        this.ApiClient = apiClient;
        this.Loading = loading;
        this.Error = error;

        if (EnvironmentName != null)
        {
            EnvironmentName.text = environment.name;
        }
        else
        {
            Debug.LogError("EnvironmentName is not assigned in the inspector.");
        }

        // Calculate the new Y position
        float newY = 90 - (position * 45);

        // Get the RectTransform component
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
        }
    }

    private void OnViewClicked()
    {
        SceneDataEnvironmentViewer.environment = environment;
        SceneManager.LoadScene("EnvironmentViewer");
    }

    private void OnEditClicked()
    {
        SceneDataEnvironmentEditor.environment = environment;
        SceneManager.LoadScene("EnvironmentEditor");
    }

    private async Task OnDeleteClicked()
    {
        Loading.On();

        IWebRequestReponse response = await ApiClient.DeleteEnvironment(this.environment.id);

        switch (response)
        {
            case WebRequestData<string> webRequestResponse:
                SceneManager.LoadScene("EnvironmentsOverview");
                break;

            case WebRequestError errorResponse:
                Error.SetErrorMessage(errorResponse.ErrorMessage);
                break;
        }

        Loading.Off();
    }
}
