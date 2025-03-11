using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentsOverview : MonoBehaviour
{
    public Environment2DApiClient ApiClient;

    public LoadingOverlay Loading;
    public ErrorMessage Error;

    private void Start()
    {
        _ = GetAllEnvironments();
    }

    private async Awaitable GetAllEnvironments()
    {
        Loading.On();
        IWebRequestReponse response = await ApiClient.ReadEnvironment2Ds();
        // Validating WebResponse
        switch (response)
        {
            case WebRequestData<List<Environment2D>> webRequestResponse:
                List<Environment2D> environment2Ds = webRequestResponse.Data;
                GenerateEnvironmentsCards(environment2Ds);
                break;
            case WebRequestError errorResponse:
                Error.SetErrorMessage(errorResponse.ErrorMessage);
                break;
        }
        Loading.Off();
    }

    private void GenerateEnvironmentsCards(List<Environment2D> environment2Ds)
    {

    }
}
