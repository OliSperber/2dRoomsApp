using UnityEngine;

public class LoadingOverlay : MonoBehaviour
{
    private void Start()
    {
        Off();
    }

    public void On()
    {
        gameObject.SetActive(true);
    }
    public void Off()
    {
        gameObject.SetActive(false);
    }
}
