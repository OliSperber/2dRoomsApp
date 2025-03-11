using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnvironmentsOverviewManager : MonoBehaviour
{
    public Button LogOutButton;

    private void Start()
    {
        LogOutButton.onClick.AddListener(LogOut);
    }
    public void LogOut()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
