using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateNewEnvironment : MonoBehaviour
{
    public TMP_InputField Name;
    public TMP_InputField MaxWidth;
    public TMP_InputField MaxHeight;

    public LoadingOverlay Loading;
    public ErrorMessage Error;

    public Environment2DApiClient ApiClient;

    public Button Create;
    public Button Cancel;

    void Start()
    {
        Cancel.onClick.AddListener(CloseMenu);
        Create.onClick.AddListener(() => _ = CreateClicked());
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    private void CloseMenu()
    {
        Error.DeActivateErrorMessage();
        gameObject.SetActive(false);
    }

    private async Awaitable CreateClicked()
    {
        Environment2D newEnvironment = new Environment2D()
        {
            name = Name.text,
            maxWidth = Convert.ToInt32(MaxWidth.text),
            maxHeight = Convert.ToInt32(MaxHeight.text)
        };

        Loading.On();

        IWebRequestReponse response = await ApiClient.CreateEnvironment(newEnvironment);

        switch (response)
        {
            case WebRequestData<string> webRequestResponse:
                break;
            case WebRequestError errorResponse:
                Error.SetErrorMessage(errorResponse.ErrorMessage);
                break;
        }

        Loading.Off();
        SceneManager.LoadScene("EnvironmentsOverview");
    }
}
