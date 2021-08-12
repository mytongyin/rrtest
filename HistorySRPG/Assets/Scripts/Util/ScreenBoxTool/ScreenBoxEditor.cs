using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

[CustomEditor(typeof(ScreenBox))]
public class ScreenBoxEditor : Editor
{
    ScreenBox screenBox;

    BoxBoundsHandle boundsHandle = new BoxBoundsHandle();

    bool graphicallySettingBounds = false;
    bool manuallySettingBounds = false;

    private void OnEnable()
    {
        screenBox = (ScreenBox)target;

        Rect curBounds = screenBox.currrentControlType == BoundControlState.cameraBounds ? screenBox.camBounds : screenBox.screenBounds;
        screenBox.controllingBounds = new Bounds(curBounds.center, new Vector3(curBounds.width, curBounds.height));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            Rect curBounds = screenBox.currrentControlType == BoundControlState.cameraBounds ? screenBox.camBounds : screenBox.screenBounds;
            screenBox.controllingBounds = new Bounds(curBounds.center, new Vector3(curBounds.width, curBounds.height));
        }

        string editButtonText = graphicallySettingBounds ? "Done Editing Boundary" : "Graphically Edit Boundary";

        if (GUILayout.Button(editButtonText))
        {
            manuallySettingBounds = false;
            graphicallySettingBounds = !graphicallySettingBounds;
            boundsHandle.center = screenBox.controllingBounds.center;
            boundsHandle.size = screenBox.controllingBounds.size;
            SceneView.RepaintAll();
        }

        manuallySettingBounds = EditorGUILayout.Foldout(manuallySettingBounds, "Specify Values");
        if (manuallySettingBounds)
        {
            graphicallySettingBounds = false;

            float left, right, top, bot;

            top = EditorGUILayout.FloatField("Top Edge", screenBox.controllingBounds.max.y);
            right = EditorGUILayout.FloatField("Right Edge", screenBox.controllingBounds.max.x);
            bot = EditorGUILayout.FloatField("Bottom Edge", screenBox.controllingBounds.min.y);
            left = EditorGUILayout.FloatField("Left Edge", screenBox.controllingBounds.min.x);

            screenBox.controllingBounds.min = new Vector2(left, bot);
            screenBox.controllingBounds.max = new Vector2(right, top);

            if (screenBox.camBounds.size.x < 0f || screenBox.camBounds.size.y < 0f)
            {
                screenBox.CameraError(true);
            }
            else
            {
                screenBox.CameraError(false);
            }

            SceneView.RepaintAll();
        }
    }

    void OnSceneGUI()
    {
        if (!graphicallySettingBounds) { return; }

        boundsHandle.center = screenBox.controllingBounds.center;
        boundsHandle.size = screenBox.controllingBounds.size;

        EditorGUI.BeginChangeCheck();
        boundsHandle.DrawHandle();
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(screenBox, "Change Screen Box Bounds");

            Bounds newBounds = new Bounds();
            newBounds.center = boundsHandle.center;
            newBounds.size = boundsHandle.size;

            screenBox.controllingBounds = newBounds;

            if(screenBox.camBounds.size.x < 0f || screenBox.camBounds.size.y < 0f)
            {
                screenBox.CameraError(true);
            }
            else
            {
                screenBox.CameraError(false);
            }
        }
    }
}
