using UnityEngine;

public class EnvironmentLayout : MonoBehaviour
{
    public float width = 20f;
    public float height = 25f;
    void Start()
    {
        AdjustSpriteSize();
        AlignBottomLeft();
    }

    // Method to set the desired size for the prefab
    public void InitializeLayout(float customWidth, float customHeight)
    {
        width = customWidth;
        height = customHeight;

        // Adjust size and alignment after initialization
        AdjustSpriteSize();
        AlignBottomLeft();
    }

    void AlignBottomLeft()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Calculate bottom-left offset (from center to bottom-left)
        Vector3 bottomLeftOffset = new Vector3(sr.bounds.extents.x, sr.bounds.extents.y, 0);

        // Move object so that its bottom-left corner is at (0,0)
        transform.position = bottomLeftOffset;
    }

    void AdjustSpriteSize()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Calculate the new size based on desired width and height
        Vector2 newSize = new Vector2(width, height);

        // Set the size of the SpriteRenderer (this will modify the sprite's rendered size without affecting the transform scale)
        sr.size = newSize;
    }
}
