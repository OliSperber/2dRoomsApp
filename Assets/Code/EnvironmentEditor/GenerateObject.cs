using System;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GenerateObject : MonoBehaviour
{
    public Vector3 Position;
    public Object ObjectPrefab;

    public Button SafeButton;

    public void Generato(int prefabNumber)
    {
        Object newObject = Instantiate(ObjectPrefab, Position, Quaternion.identity);

        newObject.GetComponent<Object>().environmentHeight = SceneDataEnvironmentEditor.environment.maxHeight;
        newObject.GetComponent<Object>().environmentWidth = SceneDataEnvironmentEditor.environment.maxWidth;
        newObject.GetComponent<Object>().SafeButton = this.SafeButton;
        newObject.GetComponent<Object>().DisplayedObject = new Object2D()
        {
            prefabId = prefabNumber,
            scaleX = 1,
            scaleY = 1,
            positionX = 2,
            positionY = 2
        };
        newObject.GetComponent<Object>().EditAble = true;
        newObject.GetComponent<Object>().PlaceInLayout();
    }

}
