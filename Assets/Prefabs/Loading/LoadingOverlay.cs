using UnityEngine;

public class LoadingOverlay : MonoBehaviour
{
    private void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // Ensures it's on top
        }
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
