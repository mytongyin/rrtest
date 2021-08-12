using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScreenBoxController))]
public class ScreenBoxControllerEditor : Editor
{
    ScreenBoxController sbc;

    private void OnEnable()
    {
        sbc = (ScreenBoxController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            sbc.ChangeScreenBox(sbc.initialScreenBox);
            ScreenBox[] screenBoxesInScene = FindObjectsOfType<ScreenBox>();

            foreach(ScreenBox sb in screenBoxesInScene)
            {
                sb.SetMainCamera();
                //SceneView.RepaintAll();
            }
        }
    }
}
