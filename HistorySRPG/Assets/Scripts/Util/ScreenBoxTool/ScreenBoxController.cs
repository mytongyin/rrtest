using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ScreenBoxController : MonoBehaviour
{
    private static ScreenBoxController _instance;
    public static ScreenBoxController instance;

    public Camera mainCamera;
    public ScreenBox initialScreenBox;

    Vector2 curMinBounds;
    Vector2 curMaxBounds;
    ScreenBox curScreenBox;

    private void Start()
    {
        if(mainCamera == null)
        {
            Debug.LogWarning("Warning: No main camera set. Attempting to set camera tagged as 'Main Camera' to the main camera on the ScreenBoxController.");
            mainCamera = Camera.main;
        }
        ChangeScreenBox(initialScreenBox);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
        instance = _instance;
    }

    private void Reset()
    {
        CheckForDuplicateSBC();
    }

    private void CheckForDuplicateSBC()
    {
        ScreenBoxController duplicate = FindObjectOfType<ScreenBoxController>();

        if (duplicate)
        {
            Debug.LogWarning("Warning: You already have another ScreenBoxController in the scene. Only one should exist.");
        }
    }

    private void LateUpdate()
    {
        TestOutOfCamBounds();
    }

    private void TestOutOfCamBounds()
    {
        Vector3 camPos = mainCamera.transform.position;
        

        if (camPos.x <= curMinBounds.x) { camPos.x = curMinBounds.x; }
        if (camPos.x >= curMaxBounds.x) { camPos.x = curMaxBounds.x; }
        if (camPos.y <= curMinBounds.y) { camPos.y = curMinBounds.y; }
        if (camPos.y >= curMaxBounds.y) { camPos.y = curMaxBounds.y; }

        mainCamera.transform.position = camPos;
    }

    public void ChangeScreenBox(ScreenBox newScreenBox)
    {
        if(newScreenBox == null) { return; }

        if (curScreenBox != null)
        {
            curScreenBox.isCurScreenBox = false;
        }

        curScreenBox = newScreenBox;
        curScreenBox.isCurScreenBox = true;

        curMinBounds = curScreenBox.camBounds.min;
        curMaxBounds = curScreenBox.camBounds.max;
    }
}
