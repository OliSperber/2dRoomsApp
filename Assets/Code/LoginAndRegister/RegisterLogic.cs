using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterLogic : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;
    public TMP_InputField ConfirmPassword;

    public UserApiClient userApiClient;




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
