using UnityEngine;
using UnityEditor;

[System.Serializable]
public enum BoundControlState { cameraBounds, screenBounds }

/// <summary>
/// This script will attach to the parent object of all the objects in one particular ScreenBox
/// </summary>
[DisallowMultipleComponent]
public class ScreenBox : MonoBehaviour
{    
    public BoundControlState currrentControlType = BoundControlState.screenBounds;

    [HideInInspector] public bool isCurScreenBox;
    [HideInInspector] public Bounds controllingBounds;
    [HideInInspector] public Rect camBounds, screenBounds;

    private Vector3 difference = Vector3.zero;
    private bool camErr = false;
    private Vector2[] screenBoxCorners = new Vector2[4];
    private Camera mainCamera;

    private void Start()
    {
        SetMainCamera();
    }

    private void Reset()
    {
        SetMainCamera();

        float camHeight = mainCamera.orthographicSize * 2;
        float camWidth = camHeight * mainCamera.aspect;
        
        currrentControlType = BoundControlState.screenBounds;
        controllingBounds.center = gameObject.transform.position;
        controllingBounds.size = new Vector2(camWidth, camHeight);
        screenBounds.size = controllingBounds.size;
        screenBounds.center = controllingBounds.center;
    }

    public void SetMainCamera()
    {
        mainCamera = FindObjectOfType<ScreenBoxController>().mainCamera;
    }

    private void OnDrawGizmos()
    {
        if(mainCamera == null) {
            SetMainCamera();
            if (mainCamera == null)
            {
                return;
            }
        }

        if (!transform.hasChanged)
        {
            difference = controllingBounds.center - transform.position;
        }
        else if (transform.hasChanged)
        {
            controllingBounds.center = transform.position + difference;
            transform.hasChanged = false;
        }

        if (currrentControlType == BoundControlState.cameraBounds)
        {
            CalcCamBounds(true);
            CalcScreenBounds(false);
        }
        else if(currrentControlType == BoundControlState.screenBounds)
        {
            CalcScreenBounds(true);
            CalcCamBounds(false);
        }

        if (camErr)
        {
            DrawError();
        }

        Gizmos.color = Color.yellow;
        DrawRectBox(camBounds);

        Gizmos.color = isCurScreenBox ? Color.green : Color.red;
        DrawRectBox(screenBounds);
        
    }
    
    private void DrawRectBox(Rect boundsToDraw)
    {
        Vector2[] boundsCorners = new Vector2[4];

        boundsCorners[0] = new Vector2(boundsToDraw.min.x, boundsToDraw.max.y);
        boundsCorners[1] = new Vector2(boundsToDraw.max.x, boundsToDraw.max.y);
        boundsCorners[2] = new Vector2(boundsToDraw.max.x, boundsToDraw.min.y);
        boundsCorners[3] = new Vector2(boundsToDraw.min.x, boundsToDraw.min.y);

        for (int i = 0; i < screenBoxCorners.Length; i++)
        {
            int nextPos = (i + 1) % 4;
            Gizmos.DrawLine(boundsCorners[i], boundsCorners[nextPos]);
        }
    }

    private void DrawError()
    {
        Vector2[] boundsCorners = new Vector2[4];

        boundsCorners[0] = new Vector2(camBounds.min.x, camBounds.max.y);
        boundsCorners[1] = new Vector2(camBounds.max.x, camBounds.max.y);
        boundsCorners[2] = new Vector2(camBounds.max.x, camBounds.min.y);
        boundsCorners[3] = new Vector2(camBounds.min.x, camBounds.min.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(boundsCorners[0], boundsCorners[2]);
        Gizmos.DrawLine(boundsCorners[1], boundsCorners[3]);
    }


    private void CalcScreenBounds(bool controlled)
    {
        if (controlled)
        {
            screenBounds.center = controllingBounds.center;
            screenBounds.size = controllingBounds.size;
        }

        else
        {
            float halfCamHeight = mainCamera.orthographicSize;
            float halfCamWidth = halfCamHeight * mainCamera.aspect;

            screenBounds.xMin = camBounds.xMin - halfCamWidth;
            screenBounds.xMax = camBounds.xMax + halfCamWidth;
            screenBounds.yMin = camBounds.yMin - halfCamHeight;
            screenBounds.yMax = camBounds.yMax + halfCamHeight;
        }
    }

    private void CalcCamBounds(bool controlled)
    {
        if (controlled)
        {
            camBounds.center = controllingBounds.center;
            camBounds.size = controllingBounds.size;
        }
        else
        {
            float halfCamHeight = mainCamera.orthographicSize;
            float halfCamWidth = halfCamHeight * mainCamera.aspect;

            camBounds.xMin = screenBounds.xMin + halfCamWidth;
            camBounds.xMax = screenBounds.xMax - halfCamWidth;
            camBounds.yMin = screenBounds.yMin + halfCamHeight;
            camBounds.yMax = screenBounds.yMax - halfCamHeight;
        }
    }

    public void CameraError(bool shouldError)
    {
        if(camErr == shouldError) { return; }
        camErr = shouldError;
        if (camErr)
        {
            Debug.LogError("Error: Camera boundaries invaild.", gameObject);
        }
        else
        {
            Debug.Log("Camera bounaries now valid.", gameObject);
        }
    }
}
