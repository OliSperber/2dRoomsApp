using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnvironmentsOverview : MonoBehaviour
{
    public Environment2DApiClient ApiClient;
    public TMP_Text AmountOfEnvironments;

    public LoadingOverlay Loading;
    public ErrorMessage Error;

    public GameObject EnvironmentObjectPrefab; // Prefab for EnvironmentObject
    public Transform ParentContainer; // Parent container for spawned objects

    private int frameCount = 0;

    private void Update()
    {
        frameCount++;

        if (frameCount == 2) // Trigger on second frame
        {
            _ = GetAllEnvironments();
            enabled = false; // Disable Update() after execution
        }
    }
    public void LogOut()
    {
        SceneManager.LoadScene("LoginScene");
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
        // Clear existing objects
        foreach (Transform child in ParentContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < environment2Ds.Count; i++)
        {
            Environment2D environment = environment2Ds[i];

            // Instantiate EnvironmentObject prefab
            GameObject obj = Instantiate(EnvironmentObjectPrefab, ParentContainer);
            EnvironmentObject envObj = obj.GetComponent<EnvironmentObject>();

            if (envObj != null)
            {
                envObj.Init(environment, i, ApiClient, Loading, Error);
            }
        }

        AmountOfEnvironments.text = $"{environment2Ds.Count} / 5 Environments";
    }
}
