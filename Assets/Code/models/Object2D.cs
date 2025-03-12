using System;

[Serializable]
public class Object2D
{
    // Ensure that the id can be set from the JSON or auto-generated if not present
    public string id = Guid.NewGuid().ToString(); // Default to new GUID

    public int prefabId;

    public float positionX;

    public float positionY;

    public float scaleX;

    public float scaleY;

    public float rotationZ = 0;

    public int sortingLayer = 1;

    public string environmentId;
}
