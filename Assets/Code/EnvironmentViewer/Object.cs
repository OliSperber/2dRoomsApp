using System;
using UnityEngine;
using UnityEngine.UI;

public class Object : MonoBehaviour
{
    public Transform trans;

    public Button SafeButton;

    public bool isDragging = false;

    public bool EditAble = false;

    private Object2D _displayedObject;

    public float environmentHeight;
    public float environmentWidth;

    public Object2D DisplayedObject
    {
        get { return _displayedObject; }
        set
        {
            _displayedObject = value;
            SelectSprite(Items[value.prefabId]);
        }
    }

    public Sprite[] Items;

    private const float MinScale = 1f;
    private const float MaxScale = 3f;

    public void PlaceInLayout()
    {
        // Get the SpriteRenderer component of the object
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Set the size (scale) based on _displayedObject.scaleX and _displayedObject.scaleY
        sr.size = new Vector2(_displayedObject.scaleX, _displayedObject.scaleY);
        sr.sortingOrder = 1;

        // Get the actual size of the sprite from its bounds
        Vector2 spriteSize = sr.bounds.size;

        // Calculate the offset from the center of the sprite to the bottom-left corner
        Vector3 bottomLeftOffset = new Vector3(spriteSize.x * 0.5f, spriteSize.y * 0.5f, 0);

        // Adjust the position of the sprite after scaling
        float positionX = _displayedObject.positionX; // Adjust with actual data source
        float positionY = _displayedObject.positionY; // Adjust with actual data source

        // Update the object's position to place the bottom-left corner at the specified coordinates
        transform.position = new Vector3(positionX, positionY, 0) - bottomLeftOffset;

        // Get the BoxCollider2D component
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            // Set the collider size to match the sprite size
            collider.size = spriteSize;
            collider.offset = new Vector2(0, 0); // Optional: Set the collider's offset, if necessary
        }
    }

    public void Update()
    {
        if (EditAble)
        {
            if (isDragging)
            {
                ColorBlock colors = SafeButton.colors;
                colors.normalColor = new Color32(95, 166, 255, 255);
                SafeButton.colors = colors;

                // Snapping the object to the grid while dragging
                trans.position = GetMousePositionSnapped();

                // Check for scale increase and decrease input (keys + and -)
                if (Input.GetKeyDown(KeyCode.Equals)) // '+' key
                {
                    ChangeScale(true); // Scale up
                }
                if (Input.GetKeyDown(KeyCode.Minus)) // '-' key
                {
                    ChangeScale(false); // Scale down
                }

                // Check if the Backspace key is pressed, and delete the object if dragging is true
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    DeleteObject();
                }
            }
        }
    }

    private void SelectSprite(Sprite item)
    {
        // Assuming there's a SpriteRenderer component attached to the object
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = item;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the object.");
        }
    }

    private void OnMouseUpAsButton()
    {
        isDragging = !isDragging;

        if (!isDragging)
        {
            trans.position = GetMousePositionSnapped();
        }
    }

    private Vector3 GetMousePosition()
    {
        Vector3 positionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        positionInWorld.z = 0;
        return positionInWorld;
    }

    // Snapping the mouse position to the grid and ensuring it stays within bounds
    private Vector3 GetMousePositionSnapped()
    {
        Vector3 mousePos = GetMousePosition();

        // Snap position to nearest tile (0.5 units)
        float snappedX = (Mathf.Round(mousePos.x / 1) * 1) + 0.5f;
        float snappedY = (Mathf.Round(mousePos.y / 1) * 1) + 0.5f;

        // Ensure the object stays within the environment bounds
        snappedX = Mathf.Clamp(snappedX, 0.5f, environmentWidth - 0.5f);
        snappedY = Mathf.Clamp(snappedY, 0.5f, environmentHeight - 0.5f);

        return new Vector3(snappedX, snappedY, 0);
    }

    private void ChangeScale(bool scaleUp)
    {
        // Only change the scale of the currently dragged object
        if (isDragging)
        {
            // Change scale based on the current scale
            if (scaleUp)
            {
                // If currently at 1, jump to 2. If at 2, jump to 3
                if (_displayedObject.scaleX == 1f)
                {
                    _displayedObject.scaleX = 2f;
                    _displayedObject.scaleY = 2f;
                }
                else if (_displayedObject.scaleX == 2f)
                {
                    _displayedObject.scaleX = 3f;
                    _displayedObject.scaleY = 3f;
                }
            }
            else
            {
                // If currently at 3, jump to 2. If at 2, jump to 1
                if (_displayedObject.scaleX == 3f)
                {
                    _displayedObject.scaleX = 2f;
                    _displayedObject.scaleY = 2f;
                }
                else if (_displayedObject.scaleX == 2f)
                {
                    _displayedObject.scaleX = 1f;
                    _displayedObject.scaleY = 1f;
                }
            }

            // Apply the new scale to the SpriteRenderer and BoxCollider2D
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.size = new Vector2(_displayedObject.scaleX, _displayedObject.scaleY);

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
                collider.size = new Vector2(_displayedObject.scaleX, _displayedObject.scaleY);

        }
    }

    private void DeleteObject()
    {
        // Destroy the current game object when Backspace is pressed and dragging is true
        Destroy(gameObject);
    }
}
