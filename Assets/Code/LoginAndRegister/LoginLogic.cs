using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginLogic : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;

    public UserApiClient userApiClient;

    public ErrorMessage ErrorMessage;
    public LoadingOverlay loading;

    public async void Login()
    {
        // Making login request
        loading.On();
        IWebRequestReponse response = await userApiClient.Login(new User()
        {
            email = Email.text,
            password = Password.text
        });
        loading.Off();

        // Validating WebResponse
        switch (response)
        {
            // Registration succesful
            case WebRequestData<string> webRequestResponse:
                SceneManager.LoadScene("EnvironmentsOverview");
                break;
            // Registration unsuccesful
            case WebRequestError errorResponse:
                LoginErrorHandler(errorResponse);
                break;
        }
    }

    private void LoginErrorHandler(WebRequestError errorResponse)
    {
        ErrorMessage.SetErrorMessage($"Login failed: User doesn't exist or credentials don't match");
    }

    public void OpenRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }
}
