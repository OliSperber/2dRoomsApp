using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    public Button ErrorDisplay;
    public TMP_Text ErrorText;

    private void Start()
    {
        DeActivateErrorMessage();
    }

    public void SetErrorMessage(string errorMessage)
    {
        ErrorText.text = errorMessage;
        gameObject.SetActive(true);
    }

    public void DeActivateErrorMessage()
    {
        gameObject.SetActive(false);
    }
}
