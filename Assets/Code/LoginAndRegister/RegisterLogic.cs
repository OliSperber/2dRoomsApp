using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterLogic : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;
    public TMP_InputField ConfirmPassword;

    public UserApiClient userApiClient;

    public ErrorMessage ErrorMessage;
    public LoadingOverlay loading;

    public async void Register()
    {
        // Check if user has putted the same password twice
        if (!(Password.text == ConfirmPassword.text))
        {
            PasswordsDontMatch();
            return;
        }
        // Check if password is valid according to the rules
        else if (!IsValidPassword(Password.text))
        {
            PasswordIsNotValid();
            return;
        }

        // After validating password it registers
        User userData = new User()
        {
            email = Email.text,
            password = Password.text
        };
        loading.On();
        IWebRequestReponse response = await userApiClient.Register(userData);
        loading.Off();

        // Validating WebResponse
        switch (response)
        {
            // Registration succesful
            case WebRequestData<string> webRequestResponse:
                loading.On();
                IWebRequestReponse loginResponse = await userApiClient.Login(userData);
                loading.Off();
                if (loginResponse is WebRequestData<string>)
                {
                    SceneManager.LoadScene("EnvironmentsOverview");
                }
                else
                {
                    ToLoginPage();
                }
                break;
            // Registration unsuccesful
            case WebRequestError errorResponse:
                RegisterErrorHandler(errorResponse);
                break;
        }
    }
    
    public void ToLoginPage()
    {
        SceneManager.LoadScene("LoginScene");
    }

    private void PasswordsDontMatch()
    {
        ErrorMessage.SetErrorMessage("Passwords do not match. Please try again.");
    }

    private void PasswordIsNotValid()
    {
        ErrorMessage.SetErrorMessage("Password must meet the following requirements:\n- At least 8 characters\n- One uppercase letter\n- One lowercase letter\n- One number\n- One special character");
    }

    private void RegisterErrorHandler(WebRequestError errorResponse)
    {
        ErrorMessage.SetErrorMessage($"Registration failed: {errorResponse.ErrorMessage}");
    }

    private bool IsValidPassword(string password)
    {
        // Check if password meets the minimum length requirement
        bool isValidLength = password.Length >= 8;

        // Check if password contains at least one digit
        bool containsDigit = password.Any(char.IsDigit);

        // Check if password contains at least one lowercase letter
        bool containsLowercase = password.Any(char.IsLower);

        // Check if password contains at least one uppercase letter
        bool containsUppercase = password.Any(char.IsUpper);

        // Check if password contains at least one non-alphanumeric character
        bool containsNonAlphanumeric = password.Any(c => !char.IsLetterOrDigit(c));

        // Return true if all conditions are met
        if (isValidLength && containsDigit && containsLowercase && containsUppercase && containsNonAlphanumeric)
        {
            return true;
        }

        // If any condition fails, return false
        return false;
    }

}
